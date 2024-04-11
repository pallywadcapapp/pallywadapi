using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PallyWad.Domain.Entities;
using PallyWad.Infrastructure.Data;
using PallyWad.Services.Generics;
using System.Text;
using PallyWad.Services.Extensions;
using Microsoft.AspNetCore.Identity;
using PallyWad.Domain;
using PallyWad.Services.Connection;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;
using PallyWad.AdminApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;
var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));



builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<DbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<SetupDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<AccountDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<DbContext>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<AppIdentityDbContext>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<SetupDbContext>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
builder.Services.AddApiVersioning();

builder.Services.AddAutoMapper(typeof(PallyWad.Application.AutoMapper));

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSimpleEmailService>();
builder.Services.RegisterServices(builder.Configuration);

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

builder.Services.AddIdentity<AppIdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddHangfire(_configuration => _configuration
      .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
      .UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
      .UseSqlServerStorage(configuration.GetConnectionString("ConnStr")));
builder.Services.AddHangfireServer();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PallyWad Admin API Server",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "PallyWad Dev",
            Email = "dev@roadally.com",
            Url = new Uri("https://pallywad.com/contact"),
        },
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "PallyWad Developers User API Server",
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

var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("JWT:Secret").Value));
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

    ValidateAudience = false, // true,
    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,

    RequireExpirationTime = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
    o.Authority = jwtAppSettingOptions[nameof(JwtIssuerOptions.Authority)];
    o.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PallyWad Admin API Server V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "PallyWad Admin API Server V2");
    });
//}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true)
    .AllowAnyHeader();
});

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard("/pallyjobs", new DashboardOptions
{
    Authorization = new[] { new HangFireAuthorizationFilter() }
});

app.MapControllers();

app.Run();
