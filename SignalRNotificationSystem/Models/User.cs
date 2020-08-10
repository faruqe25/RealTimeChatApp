using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRNotificationSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [Display(Name ="User Name")]
        [Remote("ValidateUserName", "Home",
            ErrorMessage = "Your name is already in use,try new one")] 
        public string UserName { get; set; } 
    }
}
