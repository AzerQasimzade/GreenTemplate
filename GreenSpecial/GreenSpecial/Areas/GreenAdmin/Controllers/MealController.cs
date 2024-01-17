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
			return RedirectToAction("Index","Home");
		}
	}
}
