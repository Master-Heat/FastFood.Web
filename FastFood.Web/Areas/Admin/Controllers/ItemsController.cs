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
        [HttpGet]

        public IActionResult Index()
        {
            var items = _context.Items.Include(x => x.Category).Include(y => y.SubCategory)
                .Select(model => new ItemViewModels()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Price   = model.Price,
                    CategoryId = model.CategoryId,
                    SubCategoryId   = model.SubCategoryId,
                    Category = model.Category,
                    SubCategory = model.SubCategory,

                }).ToList();

            return View(items);
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
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category) // Include Category for display in dropdown
                .Include(i => i.SubCategory) // Include SubCategory for display in dropdown
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            var viewModel = new ItemViewModels
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Price = item.Price,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                // If you want to pre-select by Title (less reliable if titles aren't unique):
                // CategoryTitle = item.Category?.Title,
                // SubCategoryTitle = item.SubCategory?.Title
            };

            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", item.CategoryId);
            ViewBag.SubCategory = new SelectList(_context.SubCategories.Where(sc => sc.CategoryId == item.CategoryId), "Id", "Title", item.SubCategoryId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemViewModels vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var itemToUpdate = await _context.Items.FindAsync(id);

                if (itemToUpdate == null)
                {
                    return NotFound();
                }

                itemToUpdate.Title = vm.Title;
                itemToUpdate.Description = vm.Description;
                itemToUpdate.Price = vm.Price;
                itemToUpdate.CategoryId = vm.CategoryId;
                itemToUpdate.SubCategoryId = vm.SubCategoryId;

                // Handle image updates if needed (similar to Create)

                try
                {
                    _context.Update(itemToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If ModelState is invalid, repopulate ViewBag and return the view
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", vm.CategoryId);
            ViewBag.SubCategory = new SelectList(_context.SubCategories.Where(sc => sc.CategoryId == vm.CategoryId), "Id", "Title", vm.SubCategoryId);
            return View(vm);
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
        //----------------------------------------

    }
}