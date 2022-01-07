﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorella_second.ViewModel.Auth
{
    public class UserLoginVM
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.Password)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public bool IsActivated { get; set; }

    }
}
