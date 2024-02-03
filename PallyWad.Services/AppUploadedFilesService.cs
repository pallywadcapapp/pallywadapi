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
    public class AppUploadedFilesService : BaseService, IAppUploadedFilesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppUploadedFilesRepository _appUploadedFilesRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public AppUploadedFilesService(IAppUploadedFilesRepository appUploadedFilesRepository, IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _appUploadedFilesRepository = appUploadedFilesRepository;
            _contextAccessor = httpContextAccessor;
        }

        public void AddAppUploadedFiles(AppUploadedFiles appUploadedFiles)
        {
            // appUploadedFiles.TenantId = tenantId;
            _appUploadedFilesRepository.Add(appUploadedFiles);
            Save();
        }

        public AppUploadedFiles GetUploadedFile(string filename)
        {
            return _appUploadedFilesRepository.Get(x => x.filename == filename);
        }

        public AppUploadedFiles GetUploadedFile(int Id)
        {
            return _appUploadedFilesRepository.Get(x => x.Id == Id);
        }

        public List<AppUploadedFiles> ListAllSetupAppUploadedFiles()
        {
            var parameters = new DynamicParameters();
            var result = _appUploadedFilesRepository.FindAll().ToList();
            return result;
            //return _appUploadedFilesRepository.Query<AppUploadedFiles>("ListAllSetupAppUploadedFiles", parameters);//new { tenantId = tenantId });
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateAppUploadedFiles(AppUploadedFiles appUploadedFiles)
        {
            _appUploadedFilesRepository.Update(appUploadedFiles);
            Save();
        }
    }

    public interface IAppUploadedFilesService
    {
        void Save();
        AppUploadedFiles GetUploadedFile(string filename);
        List<AppUploadedFiles> ListAllSetupAppUploadedFiles();
        void AddAppUploadedFiles(AppUploadedFiles appUploadedFiles);
        void UpdateAppUploadedFiles(AppUploadedFiles appUploadedFiles);
        AppUploadedFiles GetUploadedFile(int Id);
    }
}
