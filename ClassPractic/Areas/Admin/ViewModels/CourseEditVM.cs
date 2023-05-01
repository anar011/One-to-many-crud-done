using ClassPractic.Models;
using System.ComponentModel.DataAnnotations;

namespace ClassPractic.Areas.Admin.ViewModels
{
    public class CourseEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Sales { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public string Description { get; set; }
        public int CourseAuthorId { get; set; }
        public List<CourseImage> Images { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
