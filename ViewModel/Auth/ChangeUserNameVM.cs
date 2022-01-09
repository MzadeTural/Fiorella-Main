using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.ViewModel.Auth
{
    public class ChangeUserNameVM
    {
        [Required]        
        public string UserName { get; set; }
    }
}
