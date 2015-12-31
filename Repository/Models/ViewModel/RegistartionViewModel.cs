using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models.ViewModel
{
    public class RegistartionViewModel
    {
        [Required(ErrorMessage = "Please enter dealer name.")]
        public string DealerName { get; set; }

        [Required(ErrorMessage = "Please enter email id")]
        [Display(Name = "E-Mail ")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please enter correct email id")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password and this field should be identical")]
        [Compare("Password")]
        public string ConfirmPasssword { get; set; }
        public string ParticipantId { get; set; }

    }
}
