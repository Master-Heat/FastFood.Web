using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Title { get; set; }
        public ICollection <Item> Items { get; set; }
        public ICollection <SubCategory> SubCategories { get; set; }
    }
}
