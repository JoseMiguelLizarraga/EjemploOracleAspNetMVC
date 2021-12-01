using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappings.DTO
{
    public class CategoriaDTO
    {
        public decimal CategoriaId { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es requerido")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "El nombre de la categoría debe tener al menos 3 caracteres")]
        [StringLength(30, ErrorMessage = "El nombre de la categoría debe tener un máximo de 30 caracteres")]
        public string Nombre { get; set; }
    }
}
