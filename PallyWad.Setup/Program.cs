using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using PallyWad.Domain.Entities;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Extensions;
using PallyWad.Services.Generics;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<DbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<SetupDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<AccountDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<SetupDbContext>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<AppIdentityDbContext>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<AccountDbContext>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSimpleEmailService>();


builder.Services.RegisterServices(builder.Configuration);

// Register Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
  ConnectionMultiplexer.Connect(
    builder.Configuration.GetConnectionString("RedisCacheURL")));

builder.Services.AddAutoMapper(typeof(PallyWad.Application.AutoMapper));

// Add services to the container.

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
    o.TokenLifespan = TimeSpan.FromHours(3));
builder.Services.AddCors(setup =>
{
    setup.AddDefaultPolicy(policy =>
    {
        //policy.AllowCredentials();
        policy.WithOrigins("http://localhost:5135", "https://app.pallywad.com",
        "https://admin.pallywad.com", "http://localhost:8100", "https://app");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PallyWad Setup API Server",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "RoadAlly Dev",
            Email = "dev@roadally.com",
            Url = new Uri("https://pallywad.com/contact"),
        },
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "PallyWad Developers Setup API Server",
        Version = "v2",
        Description = "An example of an ASP.NET Core Web API",
        Contact = new OpenApiContact
        {
            Name = "Oduwole Oluwasegun",
            Email = "segun@impartlab.com",
            Url = new Uri("https://impartlab.com/contact"),
        },
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PallyWad Setup Server V1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "PallyWad Setup Server V2");
});
//}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true)
    .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
