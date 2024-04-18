using WebProject.Models;

namespace WebProject.interfaces
{
    public interface IDBService
    {
        Task AddData(string tableName, IModel data);
        Task DeleteData(string tableName, IModel user);

        Task EditData(IModel model, string tableName);

        Task<List<TModel>?> GetData<TModel>(string tableName, string condition = null) where TModel : IModel;
    }
}