using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.ViewModel.Auth
{
    public class ChangePasswordVM
    {
        [Required, MaxLength(255), DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
       
        [Required, MaxLength(255), DataType(DataType.Password)]
        public string NewPassword { get; set; }
       
        [DataType(DataType.Password), Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
