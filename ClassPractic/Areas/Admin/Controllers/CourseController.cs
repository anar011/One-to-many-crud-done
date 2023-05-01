using ClassPractic.Areas.Admin.ViewModels;
using ClassPractic.Data;
using ClassPractic.Helper;
using ClassPractic.Models;
using ClassPractic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ClassPractic.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IAuthorService _authorService;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseController(ICourseService courseService,
                                IAuthorService authorService,
                                       AppDbContext context,
                                       IWebHostEnvironment env
                                       )


        {
            _courseService = courseService;
            _authorService = authorService;
            _context = context;
            _env = env;
           
        }
        public async  Task<IActionResult> Index()
        {

            IEnumerable<Course> courses = await _context.Courses.Include(m=>m.CourseImages).Include(m => m.CourseAuthor).Where(m => !m.SoftDelete).ToListAsync();
            
            return View(courses);
        }

        public async Task<IActionResult> Detail(int ? id)
        {
            if (id == null) return BadRequest();

            Course? course= await _context.Courses.Include(m => m.CourseImages).Include(m => m.CourseAuthor).Where(m=>m.Id == id).FirstOrDefaultAsync();

            if (course is null) return NotFound();
            
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {


            ViewBag.authors = await GetAuthorsAsync();


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM model)
        {
            try
            {
                ViewBag.categories = await GetAuthorsAsync();  // submit-etdikden sonra reflesh- etdikde category silinmemesi ucun 


                if (!ModelState.IsValid)
                {
                    return View(model); //model-i ,View-a gonderme sebebi create-eden zaman hansisa xana sef olduqda refles zamani butun xanalar silinmemesi ucun(yalnizca sef olan xanani yeniden yazmaq ucucn)
                }


                foreach (var photo in model.Photos)     //bir-bir sekilleri yoxlamaq ucun
                {
                    if (!photo.CheckFileType("image/"))
                    {

                        ModelState.AddModelError("Photo", "File type must be image");
                        return View();

                    }

                    if (!photo.CheckFileSize(200))
                    {

                        ModelState.AddModelError("Photo", "Image size must be max 200kb");
                        return View();

                    }
                }
                //instance - almamis obyektleri,List-leri bir-birine beraberlesdirmeyin
                List<CourseImage> courseImages = new();  // (CourseImage) - Model folderinde olan


                foreach (var photo in model.Photos)   // sekilleri fiziki olaraq yaratmaq ucun (img folderinin icerisinde)//
                {


                    //Guid-datalari ferqli-ferqli yaratmaq ucun// 
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;               //Duzeltdiyin datani stringe cevir// 
                                                                                                      //Datanin adina photo - un adini birlesdir
                                                                                                      //(img) - sekilleri fizi olaraq yaradiriq img folderin icerisinde 
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "images", fileName);

                    //FileStream - bir fayli fiziki olaraq kompda harasa save etmek isteyirsense onda bir yayin(axin,muhit) yaradirsan ki,onun vasitesi ile save edesen.


                    await FileHelper.SaveFileAsync(path, photo);   // sekli path edir,

                    CourseImage courseImage = new()   // sekli gelir bura yazir (www.root-n icerisdindeki img-e sekli yazir)
                    {
                        Image = fileName
                    };

                    courseImages.Add(courseImage);   // sekli yazdiqdan sonra ,productImage-den gelir object yaradir //

                }


                courseImages.FirstOrDefault().IsMain = true;   //sekli yuxarida olan List-e  [ List<ProductImage> productImages = new() ] elave edir

                decimal convertPrice = decimal.Parse(model.Price.Replace(".", ",")); //(Price) - da (.) noqte ve (,) vergul islemesi ucun.

                Course newCourse = new()
                {
                    Name = model.Name,
                    Price = convertPrice,
                    Sales = model.Sales,
                    Description = model.Description,
                    CourseAuthorId = model.CourseAuthorId,
                    CourseImages = courseImages
                };
                //(AddRangeAsync)- Listi listin iceriseine qoymaq ucun hazir metod. 
                await _context.CourseImages.AddRangeAsync(courseImages);
                await _context.Courses.AddAsync(newCourse);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }


        }




        private async Task<SelectList> GetAuthorsAsync()
        {
            IEnumerable<CourseAuthor> authors = await _authorService.GetAll();    // submit-etdikden sonra reflesh- etdikde category silinmemesi ucun 
            return new SelectList(authors, "Id", "Name");
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
    
        public async Task<IActionResult> Delete(int? id)
        {

            try
            {

                Course course = await _courseService.GetFullDataById((int)id);

                foreach (var item in course.CourseImages)
                {

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "img", item.Image);

                    FileHelper.DeleteFile(path);
                }


                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }

        }




        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                bool result = false;

                CourseImage courseImage = await _context.CourseImages.Where(m => m.Id == id).FirstOrDefaultAsync();

                if (courseImage == null) return NotFound();

                var data = await _context.Courses.Include(m => m.CourseImages).FirstOrDefaultAsync(m => m.Id == courseImage.CourseId);

                if (data.CourseImages.Count > 1)
                {
                    string path = FileHelper.GetFilePath(_env.WebRootPath, "img", courseImage.Image);

                    FileHelper.DeleteFile(path);

                    _context.CourseImages.Remove(courseImage);

                    await _context.SaveChangesAsync();

                    result = true;
                }

                data.CourseImages.FirstOrDefault().IsMain = true;

                await _context.SaveChangesAsync();

                return Ok(result);

            }
            catch (Exception)
            {

                throw;
            }




          
        }









        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                ViewBag.authors = await GetAuthorsAsync();

                Course dbCourse = await _courseService.GetFullDataById((int)id);

                if (dbCourse == null) return NotFound();


                CourseEditVM model = new()
                {
                    Id = dbCourse.Id,
                    Name = dbCourse.Name,
                    Sales = dbCourse.Sales,
                    Price = dbCourse.Price.ToString("0.#####"),
                    CourseAuthorId = dbCourse.CourseAuthorId,
                    Images = dbCourse.CourseImages.ToList(),
                    Description = dbCourse.Description
                };


                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CourseEditVM updatedCourse)
        {
            try
            {
                if (id == null) return BadRequest();

                ViewBag.authors = await GetAuthorsAsync();

                Course dbCourse = await _context.Courses.AsNoTracking().Include(m => m.CourseImages).Include(m => m.CourseAuthor).FirstOrDefaultAsync(m => m.Id == id);

                if (dbCourse == null) return NotFound();

                if (!ModelState.IsValid)
                {
                    updatedCourse.Images = dbCourse.CourseImages.ToList();
                    return View(updatedCourse);
                }

                List<CourseImage> courseImages = new();

                if (updatedCourse.Photos is not null)
                {
                    foreach (var photo in updatedCourse.Photos)
                    {
                        if (!photo.CheckFileType("image/"))
                        {
                            ModelState.AddModelError("Photo", "File type must be image");
                            updatedCourse.Images = dbCourse.CourseImages.ToList();
                            return View(updatedCourse);
                        }

                        if (!photo.CheckFileSize(200))
                        {
                            ModelState.AddModelError("Photo", "Image size must be max 200kb");
                            updatedCourse.Images = dbCourse.CourseImages.ToList();
                            return View(updatedCourse);
                        }
                    }



                    foreach (var photo in updatedCourse.Photos)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                        string path = FileHelper.GetFilePath(_env.WebRootPath, "images", fileName);

                        await FileHelper.SaveFileAsync(path, photo);

                        CourseImage productImage = new()
                        {
                            Image = fileName
                        };

                        courseImages.Add(productImage);
                    }

                    await _context.CourseImages.AddRangeAsync(courseImages);
                }

                decimal convertedPrice = decimal.Parse(updatedCourse.Price.Replace(".", ","));

                Course newCourse = new()
                {
                    Id = dbCourse.Id,
                    Name = updatedCourse.Name,
                    Price = convertedPrice,
                    Sales = updatedCourse.Sales,
                    Description = updatedCourse.Description,
                    CourseAuthorId = updatedCourse.CourseAuthorId,
                    CourseImages = courseImages.Count == 0 ? dbCourse.CourseImages : courseImages
                };


                _context.Courses.Update(newCourse);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
