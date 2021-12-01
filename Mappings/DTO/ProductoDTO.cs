using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappings.DTO
{
    public class ProductoDTO
    {
        public decimal ProductoId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [RegularExpression(@"^.{3,}$", ErrorMessage = "El nombre del producto debe tener al menos 3 caracteres")]
        [StringLength(30, ErrorMessage = "El nombre del producto debe tener un máximo de 30 caracteres")]
        public string NombreProducto { get; set; }


        [Required(ErrorMessage = "El precio del producto es requerido")]
        [Range(1000, 50000000, ErrorMessage = "El precio del producto no está dentro del rango entre {1} y {2}.")]
        public decimal PrecioProducto { get; set; }


        [Required(ErrorMessage = "El stock del producto es requerido")]
        [Range(1, 10000000, ErrorMessage = "El stock del producto no está dentro del rango entre {1} y {2}.")]
        public decimal StockProducto { get; set; }

        public string ImagenProducto { get; set; }
        public decimal CategoriaId { get; set; }

        [Required(ErrorMessage = "La categoría del producto es requerida")]
        public CategoriaDTO Categoria { get; set; }  // Esto se llama "Colaboracion de Clases"
    }
}
