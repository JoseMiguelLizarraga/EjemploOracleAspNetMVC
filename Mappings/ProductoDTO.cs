using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappings
{
    public class ProductoDTO
    {
        public decimal ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public decimal StockProducto { get; set; }
        public string ImagenProducto { get; set; }
        public decimal CategoriaId { get; set; }
        public CategoriaDTO Categoria { get; set; }  // Esto se llama "Colaboracion de Clases"
    }
}
