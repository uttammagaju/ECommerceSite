using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteModels.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public String? Name { get; set; }
        public String? StreetAddress { get; set; }
        public String? City { get; set; }
        public String? State { get; set; }
        public String? PostalCode { get; set; }
        public String? PhoneNumber { get; set; }
    }
}

