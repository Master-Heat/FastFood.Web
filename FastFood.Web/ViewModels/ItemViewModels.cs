using FastFood.Models;

namespace FastFood.Web.ViewModels
{
    public class ItemViewModels
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public IFormFile ImageUrl {  get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public Category Category { get; set; }
    }
}
