using DataAccess;
using Mappings.DTO;
using System.Collections.Generic;

namespace Services
{
    public interface IProductoService
    {
        RespuestaService<PRODUCTO> BuscarPorId(int id);
        RespuestaService<List<PRODUCTO>> Listar();
        RespuestaService<DataTableDTO<ProductoDTO>> LlenarDataTable(PRODUCTO producto, int inicio, int registrosPorPagina);
        RespuestaService<PRODUCTO> Guardar(PRODUCTO producto);
        RespuestaService<PRODUCTO> Actualizar(PRODUCTO producto);
        RespuestaService<bool> Eliminar(int id);
        void AgregarFotoProducto(int productoId, string extensionArchivo);
        bool EliminarFoto(int productoId);
    }
}