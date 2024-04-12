using TH_Lap3.Models;

namespace TH_Lap3.Repositories
{
    public interface IProductRepository
    {
        //***SQL
        //Task<IEnumerable<Product>> GetAllAsync();
        //Task<Product> GetByIdAsync(int id);
        //Task AddAsync(Product product);
        //Task UpdateAsync(Product product);
        //Task DeleteAsync(int id);

        Task<IEnumerable<Product>> SearchAsync(string keyword);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetProductByCategoryAsync(int id);

        //***API
        //Task<IEnumerable<Product>> GetProductsAsync();
        //Task<Product> GetProductByIdAsync(int id);
        //Task AddProductAsync(Product product);
        //Task UpdateProductAsync(Product product);
        //Task DeleteProductAsync(int id);
    }

}
