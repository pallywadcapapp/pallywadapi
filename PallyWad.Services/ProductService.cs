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
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddProduct(Product product)
        {
            _productRepository.Add(product);
            Save();
        }

        public List<Product> ListAllProducts()
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@tenantId", tenantId);
            var result = _productRepository.FindAll().ToList();
            return result;
        }

        public List<Product> ListAllProductType(string type)
        {
            var parameters = new DynamicParameters();
            var result = _productRepository.FindAll().Where(x => x.type == type).ToList();
            return result;
        }

        public Product GetProduct(string id)
        {
            return _productRepository.Get(x => x.Name == id);
        }

        public Product GetDefaultSavingsProduct()
        {
            var result = _productRepository.FindAll().Where(x => x.type == "Asset" && x.isDefault == true).FirstOrDefault();
            return result;
        }

        public Product GetDefaultLoanProduct()
        {
            var result = _productRepository.FindAll().Where(x => x.type == "Liability" && x.isDefault == true).FirstOrDefault();
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Update(product);
            Save();
        }
    }

    public interface IProductService
    {
        void AddProduct(Product Product);
        List<Product> ListAllProducts();
        List<Product> ListAllProductType(string type);
        Product GetProduct(string id);
        Product GetDefaultSavingsProduct();
        Product GetDefaultLoanProduct();
        void Save();
        void UpdateProduct(Product Product);
    }
}
