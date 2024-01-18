namespace GreenSpecial.Areas.ViewModels
{
    public class UpdateMealVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
