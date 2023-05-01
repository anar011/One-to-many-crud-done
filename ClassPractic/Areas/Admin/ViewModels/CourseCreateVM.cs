using ClassPractic.Models;
using System.ComponentModel.DataAnnotations;

namespace ClassPractic.Areas.Admin.ViewModels
{
    public class CourseCreateVM
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }

        public string Price { get; set; }

        public int Sales { get; set; }

        public int CourseAuthorId { get; set; }

        public List<IFormFile> Photos { get; set; }




    }
}
