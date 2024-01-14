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
    public class SMSConfigService : BaseService, ISMSConfigService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISMSConfigRepository _sMSConfigRepository;

        public SMSConfigService(ISMSConfigRepository sMSConfigRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _sMSConfigRepository = sMSConfigRepository;
        }

        public void AddSetupSMSConfig(SMSConfig sMSConfig)
        {
            //sMSConfig.TenantId = tenantId;
            _sMSConfigRepository.Add(sMSConfig);
            Save();
        }

        public void DeleteSetupSMSConfig(SMSConfig sMSConfig)
        {
            _sMSConfigRepository.Delete(sMSConfig);
            Save();
        }

        public List<string> GetAllSetupSMSConfig()
        {
            return ListAllSetupSMSConfig().Select(u => u.configname).ToList();
        }

        public SMSConfig GetSMSConfigByName(string name)
        {
            return ListAllSetupSMSConfig().Where(u => u.configname == name).FirstOrDefault();
        }

        public List<SMSConfig> ListAllSetupSMSConfig()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _sMSConfigRepository.FindAll().ToList();
            return result;
            //return _sMSConfigRepository.Query<SMSConfig>("ListAllSetupSMSConfig", parameters);//new { tenantId = tenantId });
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateSetupSMSConfig(SMSConfig sMSConfig)
        {
            //sMSConfig.TenantId = tenantId;
            _sMSConfigRepository.Update(sMSConfig);
            Save();
        }
    }

    public interface ISMSConfigService
    {
        void Save();
        List<SMSConfig> ListAllSetupSMSConfig();
        List<string> GetAllSetupSMSConfig();
        void AddSetupSMSConfig(SMSConfig SMSConfig);
        void DeleteSetupSMSConfig(SMSConfig SMSConfig);
        void UpdateSetupSMSConfig(SMSConfig SMSConfig);
        SMSConfig GetSMSConfigByName(string name);
    }
}
