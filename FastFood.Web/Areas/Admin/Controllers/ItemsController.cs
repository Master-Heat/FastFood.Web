using FastFood.Models;
using FastFood.Reposiory;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FastFood.Web.Areas.Admin.Controllers
{
        [Area("Admin")]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this._webHostEnvironment = webHostEnvironment;
        }
        //[HttpGet]

        //public IActionResult Index()
        //{
        //    var items = _context.Items.Include(x => x.Category).Include(y => y.SubCategory)
        //        .Select(model => new ItemViewModels()
        //        {
        //            Id = model.Id,
        //            Title = model.Title,
        //            Description = model.Description,
        //            Price   = model.Price,
        //            CategoryId = model.CategoryId,
        //            SubCategoryId   = model.SubCategoryId,
                    
        //        }).ToList();
        //    return View(items);
        //}


            [HttpGet]
            public IActionResult Index()
            {
                var theitem = _context.Items.Include(x => x.Category).ToList();

                return View(theitem);
            }


        [HttpGet]
        public IActionResult Create()
        {
            ItemViewModels vm = new ItemViewModels();
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title");
            //ViewBag.SubCategory = new SelectList(_context.SubCategories, "Id", "Title");
            return View();   

        }

        [HttpGet]
        public IActionResult GetSubCategory(int CategoryId)
        {
            var subCategory = _context.SubCategories.
                Where(x => x.CategoryId == CategoryId)
                .Select(s => new { id = s.Id, title = s.Title }) // Project to anonymous object for JSON serialization
        .ToList();
            return Json(subCategory);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModels vm)
        {
            Item model = new Item();

            if (ModelState.IsValid) { 
            if (vm.ImageUrl !=null && vm.ImageUrl.Length > 0)
                {
                    var uploadDir = @"Images/Items";
                    var filename = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, uploadDir, filename);
                    await vm.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    model.Image = "/" + uploadDir + "/" + filename;
                }
            model.Price = vm.Price;
                model.Description = vm.Description;
                model.Title = vm.Title;
                 model.CategoryId = vm.CategoryId;
                model.SubCategoryId = vm.SubCategoryId;
                _context.Items.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            ItemViewModels vm = new ItemViewModels();
           var item = _context.Items.Where(x => x.Id == id).FirstOrDefault();
            if(item != null)
            {
                vm.Id = item.Id;
                vm.Title = item.Title;
                vm.Description = item.Description;
                vm.Price = item.Price;
                //vm.ImageUrl = item.Image;


                ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", item.CategoryId);
            ViewBag.SubCategory = new SelectList(_context.SubCategories, "Id", "Title", item.SubCategory);
            }
            
            return View(vm);

        }

        //----------------------------------------

       

        [HttpPost]
        public IActionResult Edit(ItemViewModels vm)
        {
            Item item = _context.Items.Where(x => x.Id == vm.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {

                item.Id = vm.Id;
                item.Title = vm.Title;
                item.Description = vm.Description;
                item.Price = vm.Price;
                item.CategoryId = vm.CategoryId;
                item.SubCategoryId = vm.SubCategoryId;
                _context.Items.Update(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        //----------------------------------------

    }
}