using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YARG.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Username")] 
        public string Username { get; set; }

        [Display(Name = "IsEmailConfirmed")] 
        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "EmailRequired")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "EmailIsNotValid")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "StatusMessage")]
        public string StatusMessage { get; set; }
    }
}
