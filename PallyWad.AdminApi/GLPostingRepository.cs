using PallyWad.Services;
using PallyWad.Domain;
using System.Globalization;

namespace PallyWad.AdminApi
{
    public class GLPostingRepository
    {
        private readonly IGlAccountTransService _glAccountTransService;
        public GLPostingRepository(IGlAccountTransService glAccountTransService)
        {
            _glAccountTransService = glAccountTransService;
        }

        public string PostLoanRequest(double amount, double interest, string accno, string loanIntAcc,
         string bankacc, string userid, string desc, string chequeNo, string fullname, DateTime endOfMonth)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, amount, 0, userid,
            "BEING " + desc + " REQUEST GRANTED FOR MEMBER " + fullname,
            chequeNo, refno, endOfMonth);
            var bankAcc = BuildTransaction(bankacc, 0, amount, userid,
            "BEING " + desc + " REQUEST GRANTED FOR MEMBER " + fullname,
            chequeNo, refno, endOfMonth);
            var memberIntAcc = BuildTransaction(accno, interest, 0, userid,
            "BEING INTEREST ON " + desc + " REQUEST GRANTED FOR MEMBER " + fullname,
            chequeNo, refno, endOfMonth);
            var interestAcc = BuildTransaction(loanIntAcc, 0, interest, userid,
            "BEING INTEREST ON " + desc + " REQUEST GRANTED FOR MEMBER " + fullname,
            chequeNo, refno, endOfMonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);
            PostAccTrans(memberIntAcc);
            PostAccTrans(interestAcc);

            return refno;
        }

        public string PostMemberSaving(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth, string fullname)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, 0, amount, userid,
            "BEING SAVINGS RECEIVED FROM MEMBER " + fullname + " ON "
            + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, amount, 0, userid,
            "BEING SAVINGS RECEIVED FROM MEMBER " + fullname + " ON "
            + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }

        public string PostMemberShares(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth, string fullname)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, 0, amount, userid,
            "BEING SHARES PURCHASE BY MEMBER " + fullname + " ON "
            + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, amount, 0, userid,
            "BEING SHARES PURCHASE BY MEMBER " + fullname + " ON "
            + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }


        public string PostMemberSaving(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, string fullname)
        {
            var endofmonth = DateTime.Now;
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, 0, amount, userid,
            "BEING SAVINGS RECEIVED FROM MEMBER " + fullname + " ON "
            + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, amount, 0, userid,
            "BEING SAVINGS RECEIVED FROM MEMBER " + fullname + " ON "
            + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }
        public string PostDeductMemberSaving(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth, string fullname)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, amount, 0, userid,
            "BEING FUNDS WITHDRAWAL BY MEMBER " + fullname,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, 0, amount, userid,
            "BEING FUNDS WITHDRAWAL BY MEMBER " + fullname,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }

        public string PostDeductChargesMemberSaving(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth, string fullname)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, amount, 0, userid,
            "BEING CHARGES ON SAVINGS ON MEMBER " + fullname,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, 0, amount, userid,
            "BEING CHARGES ON SAVINGS ON MEMBER " + fullname,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }

        public string PostInterestDeductMemberSaving(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth, string fullname)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, amount, 0, userid,
            "BEING INTEREST RECEIVED ON FUNDS WITHDRAWAL BY MEMBER " + fullname,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, 0, amount, userid,
            "BEING INTEREST RECEIVED ON FUNDS WITHDRAWAL BY MEMBER " + fullname,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }

        public string PostProcessingFeeDeductMemberSaving(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth, string fullname)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, amount, 0, userid,
            "BEING PROCESSING FEE ON LOAN REQUEST BY MEMBER " + fullname,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, 0, amount, userid,
            "BEING PROCESSING FEE ON LOAN REQUEST BY MEMBER " + fullname,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }

        public string PostInitialMemberSaving(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, 0, amount, userid,
            "BEING SAVINGS BALANCE AS AT " + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, amount, 0, userid,
            "BEING SAVINGS BALANCE FROM MEMBER AS AT " + +endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }
        public string PostInitialMemberShares(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, 0, amount, userid,
            "BEING SHARES BALANCE FROM MEMBER AS AT " + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            var bankAcc = BuildTransaction(bankacc, amount, 0, userid,
            "BEING SAVINGS SHARES FROM MEMBER AS AT " + +endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }



        public string PostInitialMemberLoan(double amount, string accno, string bankacc, string userid,
        string desc, string chequeNo, DateTime endofmonth)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, amount, 0, userid,
            "BEING LOAN BALANCE FOR MEMBER AS AT " + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);
            var bankAcc = BuildTransaction(bankacc, 0, amount, userid,
            "BEING LOAN BALALNCE FOR MEMBER AS AT " + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
            chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);
            return refno;
        }

        public string PostSavingToLoan(double amount, string accno, string memberLoanAcc, string userid,
        string desc, string chequeNo, DateTime endofmonth, string fullname, string category)
        {
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accno, amount, 0, userid,
               "BEING " + category.ToUpper() + " REPAYMENT BY MEMBER " + fullname + " ON " + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
                chequeNo, refno, endofmonth);
            var bankAcc = BuildTransaction(memberLoanAcc, 0, amount, userid,
                "BEING " + category.ToUpper() + " REPAYMENT BY MEMBER " + fullname + " ON " + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
                chequeNo, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }



        public string PostAccountClosure(double amount, string accountno, string bankaccno, string postedBy, string desc, string v3, string fullname,
        DateTime endofmonth)
        {
            //Account Termination
            var refno = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var memberAcc = BuildTransaction(accountno, amount, 0, postedBy,
               "BEING ACCOUNT TERMINATION SETTLEMENT FOR " + fullname + " ON " + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
                desc, refno, endofmonth);
            var bankAcc = BuildTransaction(bankaccno, 0, amount, postedBy,
                "BEING ACCOUNT TERMINATION SETTLEMENT FOR  " + fullname + " ON " + endofmonth.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(endofmonth.Month) + " " + endofmonth.Year,
                desc, refno, endofmonth);

            PostAccTrans(memberAcc);
            PostAccTrans(bankAcc);

            return refno;
        }
        public GL BuildTransaction(string accno, double dr, double cr, string userid,
        string desc, string chequeNo, string refno)
        {
            var accLength = accno.Length;
            var trans = new GL()
            {
                acc_name = "",
                month = DateTime.Now.Month.ToString(),
                year = DateTime.Now.Year,
                ref_trans = refno,
                accountno = accno,
                refnumber = refno,
                transdate = DateTime.Now,
                batchno = "",
                balance = cr - dr,
                description = desc,
                debitamt = dr,
                creditamt = cr,
                userid = userid,
                glaccta = accno.Substring(0, 2),
                glacctb = accno.Substring(2, 2),
                glacctc = accno.Substring(4, 2),
                glacctd = accno.Substring(6, (accLength - 6)),
                chequeno = chequeNo
            };
            return trans;
        }
        public GL BuildTransaction(string accno, double dr, double cr, string userid,
        string desc, string chequeNo, string refno, DateTime monthend)
        {
            var accLength = accno.Length;
            var trans = new GL()
            {
                acc_name = "",
                month = monthend.Month.ToString(),
                year = monthend.Year,
                ref_trans = refno,
                accountno = accno,
                refnumber = refno,
                transdate = monthend,//DateTime.Now,
                batchno = "",
                balance = cr - dr,
                description = desc,
                debitamt = dr,
                creditamt = cr,
                userid = userid,
                glaccta = accno.Substring(0, 2),
                glacctb = accno.Substring(2, 2),
                glacctc = accno.Substring(4, 2),
                glacctd = accno.Substring(6, (accLength - 6)),
                chequeno = chequeNo,
                processedDate = monthend //DateTime.Now
            };
            return trans;
        }

        private void PostAccTrans(GL accGl)
        {
            _glAccountTransService.AddGlAccount(accGl,"");
        }
    }
}
