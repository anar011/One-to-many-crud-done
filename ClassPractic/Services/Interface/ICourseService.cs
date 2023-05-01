using ClassPractic.Models;

namespace ClassPractic.Services.Interface
{
    public interface ICourseService
    {
        Task<Course> GetById(int id);
        Task<IEnumerable<Course>> GetAll();
        Task<Course> GetFullDataById(int id);
        Task<List<Course>> GetPaginatedDatas(int page, int take);
    
    }
}
