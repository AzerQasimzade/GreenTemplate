using GreenSpecial.Areas.ViewModels;
using GreenSpecial.DAL;
using GreenSpecial.Models;
using GreenSpecial.Utilities.Enums;
using GreenSpecial.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenSpecial.Areas.GreenAdmin.Controllers
{
    [Area("GreenAdmin")]
    public class MealController : Controller
	{
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MealController(AppDbContext context,IWebHostEnvironment env)
		{
            _context = context;
            _env = env;
        }
		public async Task<IActionResult> Index()
		{
			List<Meal> meals = await _context.Meals.ToListAsync();
			return View(meals);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateMealVM mealVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (!mealVM.Photo.ValidateFileType(FileHelper.Image))
			{
				ModelState.AddModelError("Photo", "File type is incorrect");
				return View();
			}
			if (!mealVM.Photo.ValidateFileSize(SizeHelper.mb))
			{
                ModelState.AddModelError("Photo", "File size is incorrect");
                return View();
            }
			string filename=Guid.NewGuid().ToString()+mealVM.Photo.FileName;
			string path = Path.Combine(_env.WebRootPath,"assets","uploads",filename);
			FileStream file = new FileStream(path, FileMode.Create);
			await mealVM.Photo.CopyToAsync(file);
			Meal meal = new Meal
			{
				Image = filename,
				Name = mealVM.Name,
				Price = mealVM.Price,
			};
			await _context.Meals.AddAsync(meal);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index","Home");
		}
		public async Task<IActionResult> Update(int id)
		{
			if (id<=0)
			{
				return BadRequest();
			}
			Meal meal= await _context.Meals.FirstOrDefaultAsync(x => x.Id==id);
			if (meal==null)
			{
				return NotFound();
			}
			UpdateMealVM mealVM = new UpdateMealVM
			{
				Image = meal.Image,
				Name = meal.Name,
				Price = meal.Price,
			};
			return View(mealVM);
		}
		[HttpPost]
        public async Task<IActionResult> Update(int id, UpdateMealVM mealVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			Meal existed= await _context.Meals.FirstOrDefaultAsync(c=>c.Id==id);
			if (existed==null)
			{
				return NotFound();
			}
			if (mealVM.Photo is not null)
			{
                if (!mealVM.Photo.ValidateFileType(FileHelper.Image))
                {
                    ModelState.AddModelError("Photo", "File type is incorrect");
                    return View();
                }
                if (!mealVM.Photo.ValidateFileSize(SizeHelper.mb))
                {
                    ModelState.AddModelError("Photo", "File size is incorrect");
                    return View();
                }
                string filename = Guid.NewGuid().ToString() + mealVM.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "uploads", filename);
                FileStream file = new FileStream(path, FileMode.Create);
                await mealVM.Photo.CopyToAsync(file);
				existed.Image.DeleteFile(_env.WebRootPath, "assets", "uploads");
				existed.Image = filename;
            }
			existed.Name = mealVM.Name;
			existed.Price = mealVM.Price;
			await _context.SaveChangesAsync();
			return RedirectToAction("Index","Home");
        }
		public async Task<IActionResult> Delete(int id)
		{
            if (id <= 0)
            {
                return BadRequest();
            }
            Meal existed = await _context.Meals.FirstOrDefaultAsync(x => x.Id == id);
            if (existed == null)
            {
                return NotFound();
            }
            existed.Image.DeleteFile(_env.WebRootPath, "assets", "uploads");
			 _context.Meals.Remove(existed);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index","Home");
        }
    }
}
