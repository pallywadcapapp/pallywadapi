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
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public void AddPayment(Payment payment)
        {
            _paymentRepository.Add(payment);
            Save();
        }

        public List<Payment> GetAllPayments()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);

            var result = _paymentRepository.FindAll().ToList();
            return result;
        }

        public Payment GetPaymentById(string Id)
        {
            int id = int.Parse(Id);
            var parameters = new DynamicParameters();
            parameters.Add("@Id", Id);
            return _paymentRepository.Get(x => x.Id == id);
            //return _paymentRepository.Query<Payment>("GetPaymentByID", parameters).FirstOrDefault();
        }

        public Payment GetPaymentByVoucherNo(string receiptNo)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@voucherNo", receiptNo);
            return _paymentRepository.Query<Payment>("GetPaymentByVoucherNo", parameters).FirstOrDefault();
        }
        public List<Payment> ListPaymentByDate(string startdate, string enddate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startdate", startdate);
            parameters.Add("@enddate", enddate);
            return _paymentRepository.Query<Payment>("GetAllPaymentByDate", parameters);
        }

        public Payment GetPayment(string id)
        {
            return _paymentRepository.Get(x => x.voucherNo == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdatePayment(Payment payment)
        {
            _paymentRepository.Update(payment);
            Save();
        }
    }

    public interface IPaymentService
    {
        void AddPayment(Payment Payment);
        List<Payment> GetAllPayments();
        Payment GetPaymentById(string Id);
        Payment GetPaymentByVoucherNo(string receiptNo);
        List<Payment> ListPaymentByDate(string startdate, string enddate);
        Payment GetPayment(string id);
        void Save();
        void UpdatePayment(Payment Payment);
    }
}
