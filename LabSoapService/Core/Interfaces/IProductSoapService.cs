using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LabSoapService.Core.Interfaces
{
    [ServiceContract]
    public interface IProductSoapService
    {
        [OperationContract]
        Task<ProductDto> GetProductById(int id);

        [OperationContract]
        Task<ProductDto[]> GetAllProducts();

        [OperationContract]
        Task AddProduct(ProductDto product);

        [OperationContract]
        Task UpdateProduct(ProductDto product);

        [OperationContract]
        Task DeleteProduct(int id);
    }
}