

using ClassPractic.Models;

namespace ClassPractic.Services.Interface
{
    public interface IAuthorService
    {
        Task<IEnumerable<CourseAuthor>> GetAll();
      
    }
}

