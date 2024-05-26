using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [ValidateNever]
        public string ImageUrl {  get; set; }=string.Empty;

        public int quantity { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId {  get; set; }
        [ValidateNever]
        public Category Category { get; set; }

    }
}
