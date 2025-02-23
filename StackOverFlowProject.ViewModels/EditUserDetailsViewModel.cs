using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowProject.ViewModels
{
    public class EditUserDetailsViewModel
    {
        public int UserId { get; set; }
        [Required]
        [RegularExpression(@"^[\w\.-]+@[\w\.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z ]*$")]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
    }
}
