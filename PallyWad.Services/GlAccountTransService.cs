using Dapper;
using Microsoft.AspNetCore.Http;
using PallyWad.Domain;
using PallyWad.Domain.Dto;
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
    public class GlAccountTransService : BaseService, IGlAccountTransService
    {
        private readonly IGlAccountTransRepository _glAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GlAccountTransService(IGlAccountTransRepository glAccountRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _glAccountRepository = glAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddGlAccount(GL glAccount)
        {
            _glAccountRepository.Add(glAccount);
            Save();
        }

        public List<GL> GetAllGlAccounts()
        {
            var parameters = new DynamicParameters();
            //

            var result = _glAccountRepository.FindAll().ToList();
            return result;
        }

        public List<GLComp> GetAllGlCompAccounts()
        {
            var parameters = new DynamicParameters();
            return _glAccountRepository.Query<GLComp>("ListCompAccTrans", parameters);
        }

        public List<GL> GetAllTransactions(string accountno)
        {
            var result = _glAccountRepository.FindAll().Where(x => x.accountno == accountno).ToList();
            return result;
        }

        public GL GetGlAccount(string id)
        {
            int refid = int.Parse(id);
            return _glAccountRepository.Get(x => x.Id == refid);
        }


        public GL GetGlAccountByRef(string id)
        {
            return _glAccountRepository.Get(x => x.refnumber == id);
        }

        public List<GlAcc> GetMemberAccPosition(string memberId)
        {
            var list = new List<GlAcc>();
            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);

            var _result = _glAccountRepository.Query<GlAcc>("GetMemberAccPositions", parameters);
            foreach (var p in _result)
            {
                p.memberId = memberId;
                list.Add(p);
            }

            return list;
        }

        public List<GlAcc> GetMemberAccPositionByDate(string memberId, DateTime date)
        {
            var list = new List<GlAcc>();
            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);
            parameters.Add("@transdate", date);

            var _result = _glAccountRepository.Query<GlAcc>("GetMemberAccPositionsByDate", parameters);
            foreach (var p in _result)
            {
                p.memberId = memberId;
                list.Add(p);
            }

            return list;
        }

        //public List<dynamic> GetMemberAccPositionsPivotByDate(string memberId, string date)
        public dynamic GetMemberAccPositionsPivotByDate(string memberId, string date)
        {
            var list = new List<dynamic>();
            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);
            parameters.Add("@transdate", date);

            var _result = _glAccountRepository.Query<dynamic>("GetMemberAccPositionsPivotByDate", parameters).FirstOrDefault();
            /*foreach (var p in _result)
            {
                p.memberId = memberId;
                list.Add(p);
            }

            return list;*/
            return _result;
        }

        public List<GlAcc> GetMemberAccPosition(string accountno, string loanNo)
        {
            return GetMemberAccPosition(accountno, loanNo, "", "");
        }

        public List<GlAcc> GetMemberAccPosition(string accountno, string loanNo, string wallet, string softloanacc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@savingsacc", accountno);
            parameters.Add("@loanacc", loanNo);
            parameters.Add("@wallet", wallet);
            parameters.Add("@softloanacc", softloanacc);

            return _glAccountRepository.Query<GlAcc>("GetMemberAccPosition", parameters);
        }
        public List<GlAcc> GetMemberAccPosition(string accountno, string loanaccountno, string shortloanaccountno,
            string sharesaccountno, string walletaccountno, string commodityAcctno)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@savingsacc", accountno);
            parameters.Add("@loanacc", loanaccountno);
            parameters.Add("@wallet", walletaccountno);
            parameters.Add("@softloanacc", shortloanaccountno);
            parameters.Add("@shareholding", sharesaccountno);
            parameters.Add("@commodity", commodityAcctno);

            return _glAccountRepository.Query<GlAcc>("GetMemberAccPosition", parameters);
        }


        public List<GlAcc> GetMemberAccPosition(string accountno, string loanaccountno, string shortloanaccountno,
            string sharesaccountno, string walletaccountno, string commodityAcctno, string memberId)
        {
            var parameters = new DynamicParameters();
            var list = new List<GlAcc>();
            parameters.Add("@savingsacc", accountno);
            parameters.Add("@loanacc", loanaccountno);
            parameters.Add("@wallet", walletaccountno);
            parameters.Add("@softloanacc", shortloanaccountno);
            parameters.Add("@shareholding", sharesaccountno);
            parameters.Add("@commodity", commodityAcctno);

            var _result = _glAccountRepository.Query<GlAcc>("GetMemberAccPosition", parameters);
            foreach (var p in _result)
            {
                p.memberId = memberId;
                list.Add(p);
            }

            return list;
        }


        public List<GlAcc> GetMemberAccPosition(List<string> acc)
        {
            string accountno = acc[0];
            string loanNo = acc[1];
            string wallet = acc[2];
            string softloanacc = acc[3];
            var parameters = new DynamicParameters();
            parameters.Add("@savingsacc", accountno);
            parameters.Add("@loanacc", loanNo);
            parameters.Add("@wallet", wallet);
            parameters.Add("@softloanacc", softloanacc);

            return _glAccountRepository.Query<GlAcc>("GetMemberAccPosition", parameters);
        }



        public List<GlReport> GetAccountReport(string transtype)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@acctype", transtype);
            return _glAccountRepository.Query<GlReport>("GetAccountReport", parameters);
        }

        public List<GlReport> GetAllAccountReport()
        {

            var parameters = new DynamicParameters();
            return _glAccountRepository.Query<GlReport>("GetAllAccountReport", parameters);
        }

        public List<GlReport> GetAllMembersSavings()
        {

            var parameters = new DynamicParameters();
            return _glAccountRepository.Query<GlReport>("ListAllMembersSavings", parameters);
        }

        public List<GlReport> GetMembersSavings(string memberId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);
            return _glAccountRepository.Query<GlReport>("ListMembersSavings", parameters);
        }

        public List<GlReport> GetMembersLoans(string memberId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);
            return _glAccountRepository.Query<GlReport>("ListMembersLoans", parameters);
        }

        public List<GlReport> GetMembersReports(string memberId, string transtype)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);
            parameters.Add("@acctype", transtype);
            return _glAccountRepository.Query<GlReport>("ListMembersGlReport", parameters);
        }



        public List<GlReport> GetMembersShortLoans(string memberId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);
            return _glAccountRepository.Query<GlReport>("ListMembersShortLoans", parameters);
        }

        public List<GlReport> GetAllMembersCommodities(string memberId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@memberId", memberId);
            return _glAccountRepository.Query<GlReport>("ListMembersCommodities", parameters);
        }

        public List<GlReport> GetAllMembersLoans()
        {

            var parameters = new DynamicParameters();
            return _glAccountRepository.Query<GlReport>("ListAllMembersLoans", parameters);
        }

        public List<GlReport> GetAllMembersCommodities()
        {

            var parameters = new DynamicParameters();
            return _glAccountRepository.Query<GlReport>("ListAllMembersCommodities", parameters);
        }

        public List<GlReport> GetAllMembersShortLoans()
        {

            var parameters = new DynamicParameters();
            return _glAccountRepository.Query<GlReport>("ListAllMembersShortLoans", parameters);
        }
        public GlReport GetLoanSummary()
        {

            var parameters = new DynamicParameters();
            return _glAccountRepository.Query<GlReport>("GetAllLoansSummation", parameters).FirstOrDefault();
        }
        public GlReport GetSavingsSummary()
        {

            var parameters = new DynamicParameters();
            return _glAccountRepository.Query<GlReport>("GetAllSavingsSummation", parameters).FirstOrDefault();
        }
        public GlReport GetShortLoanSummary()
        {

            var parameters = new DynamicParameters();
            
            return _glAccountRepository.Query<GlReport>("GetAllShortLoansSummation", parameters).FirstOrDefault();
        }

        public List<GLComp> GetGeneralLedgers(string startdate, string enddate)
        {
            var parameters = new DynamicParameters();
            
            parameters.Add("@startdate", startdate);
            parameters.Add("@enddate", enddate);
            return _glAccountRepository.Query<GLComp>("GetGLByDateRange", parameters);
        }

        public List<GLComp> GetInternalAccountsLedgers(string startdate, string enddate)
        {
            var parameters = new DynamicParameters();
            
            parameters.Add("@startdate", startdate);
            parameters.Add("@enddate", enddate);
            return _glAccountRepository.Query<GLComp>("GetInternalAccGLByDateRange", parameters);
        }

        public List<GLComp> GetMemberAccountsLedgers(string startdate, string enddate, string memberId)
        {
            var parameters = new DynamicParameters();
            
            parameters.Add("@startdate", startdate);
            parameters.Add("@enddate", enddate);
            parameters.Add("@memberId", memberId);
            return _glAccountRepository.Query<GLComp>("GetMemberAccountsLedgers", parameters);
        }


        public List<TB> GetTrialBalance(string year)
        {
            var parameters = new DynamicParameters();
            
            parameters.Add("@year", year);
            return _glAccountRepository.Query<TB>("GetTrialBalance", parameters);
        }

        public List<CompanyBalance> GetPL(string startdate, string enddate)
        {
            var parameters = new DynamicParameters();
            
            parameters.Add("@startdate", startdate);
            parameters.Add("@enddate", enddate);
            return _glAccountRepository.Query<CompanyBalance>("PLSummary", parameters);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateGlAccount(GL glAccount)
        {
            _glAccountRepository.Update(glAccount);
            Save();
        }
    }

    public interface IGlAccountTransService
    {
        void AddGlAccount(GL GlAccount);
        List<GL> GetAllGlAccounts();
        List<GLComp> GetAllGlCompAccounts();
        List<GlReport> GetAllMembersSavings();
        List<GlReport> GetMembersSavings(string memberId);
        List<GlReport> GetMembersLoans(string memberId);
        List<GlReport> GetMembersShortLoans(string memberId);
        List<GlReport> GetAllMembersCommodities(string memberId);
        List<GlReport> GetAllMembersLoans();
        List<GlReport> GetAllMembersShortLoans();
        List<GlReport> GetAllMembersCommodities();
        List<GL> GetAllTransactions(string accountno);
        List<GLComp> GetGeneralLedgers(string startdate, string enddate);
        List<GLComp> GetInternalAccountsLedgers(string startdate, string enddate);
        GL GetGlAccount(string id);
        GL GetGlAccountByRef(string id);
        List<GlAcc> GetMemberAccPosition(string memberId);
        List<GlAcc> GetMemberAccPosition(string accountno, string loanNo);
        List<GlAcc> GetMemberAccPosition(string accountno, string loanNo, string wallet, string softloanacc);
        List<GlAcc> GetMemberAccPosition(List<string> acc);
        List<GlAcc> GetMemberAccPositionByDate(string memberId, DateTime date);
        //List<dynamic> GetMemberAccPositionsPivotByDate(string memberId, string date);
        dynamic GetMemberAccPositionsPivotByDate(string memberId, string date);
        List<GLComp> GetMemberAccountsLedgers(string startdate, string enddate, string memberId);
        List<GlAcc> GetMemberAccPosition(string accountno, string loanaccountno, string shortloanaccountno1,
            string sharesaccountno, string walletaccountno, string commodityAcctno);
        List<GlAcc> GetMemberAccPosition(string accountno, string loanaccountno, string shortloanaccountno,
            string sharesaccountno, string walletaccountno, string commodityAcctno, string memberId);
        List<TB> GetTrialBalance(string year);
        void Save();
        void UpdateGlAccount(GL GlAccount);
        GlReport GetLoanSummary();
        GlReport GetSavingsSummary();
        GlReport GetShortLoanSummary();
        List<GlReport> GetAccountReport(string transtype);
        List<GlReport> GetAllAccountReport();
        List<GlReport> GetMembersReports(string memberId, string transtype);
        List<CompanyBalance> GetPL(string startdate, string enddate);
    }
}
