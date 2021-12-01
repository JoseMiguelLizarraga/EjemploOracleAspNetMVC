using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Usuario
    {
        public decimal Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime UltimaConexion { get; set; }
    }
}