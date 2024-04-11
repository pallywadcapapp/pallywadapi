using Amazon.SimpleEmail;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using PallyWad.Domain.Entities;
using PallyWad.Infrastructure.Data;
using PallyWad.Notifier.Extensions;
using PallyWad.Notifier.Helpers;
using PallyWad.Services.Generics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));



builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<SetupDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
//builder.Services.AddApiVersioning();

builder.Services.AddAutoMapper(typeof(PallyWad.Application.AutoMapper));

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSimpleEmailService>();

builder.Services.AddCustomServices();

builder.Services.AddHangfire(_configuration => _configuration
      .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
      .UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
      .UseSqlServerStorage(configuration.GetConnectionString("ConnStr")));
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard("/pallyjobs", new DashboardOptions
{
    Authorization = new[] { new HangFireAuthorizationFilter() }
});

app.MapControllers();

app.Run();
