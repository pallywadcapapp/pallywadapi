using Amazon.SimpleEmail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PallyWad.Application;
using PallyWad.Auth.Extensions.Provider;
using PallyWad.Auth.Helper.Extensions;
using PallyWad.Domain;
using PallyWad.Domain.Entities;
using PallyWad.Infrastructure.Data;
using PallyWad.Services;
using PallyWad.Services.Extensions;
using PallyWad.Services.Generics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr"),
    sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    }
    ));
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

builder.Services.AddTransient<LoggingDelegatingHandler>();
builder.Services.AddHttpClient<SMSConfigService>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://api.github.com");
})
.AddHttpMessageHandler<LoggingDelegatingHandler>();

// For Identity
builder.Services.AddIdentity<AppIdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders()
.AddPasswordlessLoginTotpTokenProvider();

var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Value));

builder.Services.Configure<JwtIssuerOptions>(options =>
{
    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
    options.Authority = jwtAppSettingOptions[nameof(JwtIssuerOptions.Authority)];
    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
});

// Adding Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes("Bearer", JwtBearerDefaults.AuthenticationScheme)
        //.AddAuthenticationSchemes("ADFS", JwtBearerDefaults.AuthenticationScheme)
        .Build();

    options.AddPolicy("Administrator", new AuthorizationPolicyBuilder()
        .RequireRole("Admin")
        .AddAuthenticationSchemes("Bearer", JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());
});

//builder.Services.Configure<IPWhitelistOptions>(builder.Configuration.GetSection("IPWhitelistOptions"));

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PallyWad Auth Server",
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
        Title = "PallyWad Developers Auth Server",
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
if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PallyWad Authentication Server V1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "PallyWad Authentication Server V2");
});

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true)
    .AllowAnyHeader();
});

//app.UseIPWhitelist();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
