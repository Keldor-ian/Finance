﻿using System.ComponentModel.DataAnnotations;

namespace FinShark.DTOs.User
{
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
