namespace Core.Services.Search.Common;

public interface IBaseSearchService<T> where T : BaseSearchModel
{
    void Create(T searchModel);
    void Update(T searchModel);
    void Delete(T searchModel);
    // zwraca listę Id znalezionych elementów
    Task<IList<int>> Search(string query);
}