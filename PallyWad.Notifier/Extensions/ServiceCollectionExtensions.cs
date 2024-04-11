using PallyWad.Services;
using PallyWad.Services.Repository;

namespace PallyWad.Notifier.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<ILoanTransRepository, LoanTransRepository>();
            services.AddTransient<ILoanTransService, LoanTransService>();
            services.AddTransient<ILoanRepaymentRepository, LoanRepaymentRepository>();
            services.AddTransient<ILoanRepaymentService, LoanRepaymentService>();
            services.AddTransient<ILoanRequestRepository, LoanRequestRepository>();
            services.AddTransient<ILoanRequestService, LoanRequestService>();
            services.AddTransient<ISmtpConfigRepository, SmtpConfigRepository>();
            services.AddTransient<ISmtpConfigService, SmtpConfigService>();
            //services.AddTransient<IMailRepository, MailRepository>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<ICompanyService, CompanyService>();
            return services;
        }
    }
}
