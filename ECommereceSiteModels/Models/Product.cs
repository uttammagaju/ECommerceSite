using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommereceSiteModels.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]

        [Display(Name ="Title")]
        public string ProductName {  get; set; }

        public string Description {  get; set; }

        public string Price {  get; set; }

        public string discountRate {  get; set; }

        [Required]
        public string ImageUrl {  get; set; }

        public int quantity { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId {  get; set; }

        public Category Category { get; set; }

    }
}
