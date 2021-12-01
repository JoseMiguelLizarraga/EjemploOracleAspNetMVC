using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Mappings.DTO;

namespace Mappings
{
    public static partial class Mapper
    {
        public static CategoriaDTO ToDTO(this CATEGORIA model)
        {
            if (model == null) return null;

            CategoriaDTO dto = new CategoriaDTO()
            {
                CategoriaId = model.CATEGORIA_ID,
                Nombre = model.NOMBRE
            };

            return dto;
        }

        public static ProductoDTO ToDTO(this PRODUCTO model)
        {
            if (model == null) return null;

            ProductoDTO dto = new ProductoDTO()
            {
                ProductoId = model.PRODUCTO_ID,
                NombreProducto = model.NOMBRE_PRODUCTO,
                CategoriaId = (decimal)model.CATEGORIA_ID,
                PrecioProducto = (decimal)model.PRECIO_PRODUCTO,
                StockProducto = (decimal)model.STOCK_PRODUCTO,
                ImagenProducto = model.IMAGEN_PRODUCTO,
                Categoria = model.CATEGORIA.ToDTO(),
            };

            return dto;
        }
    }
}
