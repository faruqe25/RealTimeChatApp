using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRNotificationSystem.Models
{
    public class Product
    {  
        [Key]
        public int ProductId { get; set; }
        [Display(Name ="Product Name")]
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Display(Name ="Is Available")]
        public bool AvaiableStatus { get; set; } 

    }
}
