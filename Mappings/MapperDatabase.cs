using DataAccess;
using Mappings.DTO;

namespace Mappings
{
    public static partial class Mapper
    {
        public static CATEGORIA ToDatabaseObject(this CategoriaDTO dto)
        {
            if (dto == null) return null;

            CATEGORIA model = new CATEGORIA()
            {
                CATEGORIA_ID = dto.CategoriaId,
                NOMBRE = dto.Nombre
            };

            return model;
        }

        public static PRODUCTO ToDatabaseObject(this ProductoDTO dto)
        {
            if (dto == null) return null;

            PRODUCTO model = new PRODUCTO()
            {
                PRODUCTO_ID = dto.ProductoId,
                NOMBRE_PRODUCTO = dto.NombreProducto,
                PRECIO_PRODUCTO = dto.PrecioProducto,
                STOCK_PRODUCTO = dto.StockProducto,
                IMAGEN_PRODUCTO = dto.ImagenProducto,
                CATEGORIA_ID = dto.Categoria != null ? dto.Categoria.CategoriaId : dto.CategoriaId,
                //CATEGORIA = dto.Categoria.ToDatabaseObject(),   // Esto daría problemas, ya que intenta crear una nueva categoria
            };

            return model;
        }
    }
}
