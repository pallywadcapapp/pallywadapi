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
    public class JournalService : BaseService, IJournalService
    {
        private readonly IJournalRepository _journalRepository;
        private readonly IUnitOfWork _unitOfWork;
        public JournalService(IJournalRepository journalRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _journalRepository = journalRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddJournal(Journal journal)
        {
            _journalRepository.Add(journal);
            Save();
        }

        public List<Journal> GetAllJournals()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _journalRepository.FindAll().ToList();
            return result;
        }

        public Journal GetJournal(string id)
        {
            return _journalRepository.Get(x => x.voucherNo == id);
        }
        public Journal GetJournalById(string receiptNo)
        {
            int id = int.Parse(receiptNo);
            var parameters = new DynamicParameters();
            parameters.Add("@Id", receiptNo);
            return _journalRepository.Get(x => x.Id == id);
        }

        public List<Journal> ListJournalByDate(string startdate, string enddate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startdate", startdate);
            parameters.Add("@enddate", enddate);
            return _journalRepository.Query<Journal>("GetAllJournalsByDate", parameters);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateJournal(Journal journal)
        {
            _journalRepository.Update(journal);
            Save();
        }
    }

    public interface IJournalService
    {
        void AddJournal(Journal Journal);
        List<Journal> GetAllJournals();
        Journal GetJournal(string id);
        Journal GetJournalById(string id);
        List<Journal> ListJournalByDate(string startdate, string enddate);
        void Save();
        void UpdateJournal(Journal Journal);
    }
}
