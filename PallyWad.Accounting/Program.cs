using Amazon.SimpleEmail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using PallyWad.Accounting.Extensions;
using PallyWad.Domain;
using PallyWad.Domain.Entities;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Extensions;
using PallyWad.Services.Generics;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.AddServiceDefaults();
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<DbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<SetupDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<AccountDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<SetupDbContext>>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSimpleEmailService>();


builder.Services.RegisterServices(builder.Configuration);

// Add services to the container.
/*builder.Host.ConfigureAppConfiguration(app => {
    app.AddAmazonSecretsManager("eu-west-1", "arn:aws:secretsmanager:eu-west-1:700639922994:secret:pallywaddb-ZcmND5");
});
builder.Services.Configure<AWSApiCredentials>(builder.Configuration);*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PallyWad Accounting API Server",
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
        Title = "PallyWad Developers Accounting API Server",
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

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
