﻿using System.ComponentModel.DataAnnotations;

namespace EvAldarado.ViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
