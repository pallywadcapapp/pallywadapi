using Dapper;
using Microsoft.AspNetCore.Http;
using PallyWad.Domain;
using PallyWad.Services.Attributes;
using PallyWad.Services.Generics;
using PallyWad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    [TransientRegistration]
    public class SmtpConfigService : BaseService, ISmtpConfigService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISmtpConfigRepository _smtpConfigRepository;

        public SmtpConfigService(ISmtpConfigRepository smtpConfigRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _smtpConfigRepository = smtpConfigRepository;
        }

        public void AddSetupSmtpConfig(SmtpConfig smtpConfig)
        {
            //smtpConfig.TenantId = tenantId;
            _smtpConfigRepository.Add(smtpConfig);
            Save();
        }

        public void DeleteSetupSmtpConfig(SmtpConfig smtpConfig)
        {
            _smtpConfigRepository.Delete(smtpConfig);
            Save();
        }

        public List<string> GetAllSetupSmtpConfig()
        {
            return ListAllSetupSmtpConfig().Select(u => u.configname).ToList();
        }

        public SmtpConfig GetSmtpConfigByName(string name)
        {
            return ListAllSetupSmtpConfig().Where(u => u.configname == name).FirstOrDefault();
        }

        public List<SmtpConfig> ListAllSetupSmtpConfig()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _smtpConfigRepository.FindAll().ToList();
            return result;
            //return _smtpConfigRepository.Query<SmtpConfig>("ListAllSetupSmtpConfig", parameters);//new { tenantId = tenantId });
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateSetupSmtpConfig(SmtpConfig smtpConfig)
        {
            //smtpConfig.TenantId = tenantId;
            _smtpConfigRepository.Update(smtpConfig);
            Save();
        }
    }

    public interface ISmtpConfigService
    {
        void Save();
        List<SmtpConfig> ListAllSetupSmtpConfig();
        List<string> GetAllSetupSmtpConfig();
        void AddSetupSmtpConfig(SmtpConfig SmtpConfig);
        void DeleteSetupSmtpConfig(SmtpConfig SmtpConfig);
        void UpdateSetupSmtpConfig(SmtpConfig SmtpConfig);
        SmtpConfig GetSmtpConfigByName(string name);
    }
}
