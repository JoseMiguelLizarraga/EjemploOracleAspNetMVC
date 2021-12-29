using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using System.Data.Objects;
using Util;
using Mappings.DTO;
using Mappings;
using System.Configuration;
using Repository;

namespace Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repositorio;
        private string _rutaImagenes;

        public ProductoService(IProductoRepository repositorio)
        {
            _repositorio = repositorio;
            _rutaImagenes = ConfigurationManager.AppSettings["rutaImagenesProductos"];  // Se encuentra en el Web.config
        }

        public RespuestaService<PRODUCTO> BuscarPorId(int id)
        {
            var res = _repositorio.BuscarPorId(id);
            return new RespuestaService<PRODUCTO>() { Objeto = res };
        }

        public RespuestaService<List<PRODUCTO>> Listar()
        {
            var res = _repositorio.Listar();
            return new RespuestaService<List<PRODUCTO>>() { Objeto = res };
        }

        public RespuestaService<DataTableDTO<ProductoDTO>> LlenarDataTable(PRODUCTO producto, int inicio, int registrosPorPagina)
        {
            try
            {
                if (producto == null) producto = new PRODUCTO();  // En caso de que sea nulo se inicializa 

                (List<PRODUCTO>, int) res = _repositorio.ListarConPaginacion(producto, inicio, registrosPorPagina);

                return new RespuestaService<DataTableDTO<ProductoDTO>>()
                {
                    Objeto = new DataTableDTO<ProductoDTO>()
                    {
                        Data = res.Item1.Select(Mapper.ToDTO).ToList(),
                        RecordsFiltered = res.Item2,
                        RecordsTotal = res.Item2
                    }
                };
            }
            catch (Exception ex)
            {
                return new RespuestaService<DataTableDTO<ProductoDTO>>() { ExcepcionCapturada = ExcepcionesHelper.ObtenerExcepcion(ex) };
            }
        }

        public RespuestaService<PRODUCTO> Guardar(PRODUCTO p)
        {
            try
            {
                var res = _repositorio.Guardar(p);
                return new RespuestaService<PRODUCTO>() { Objeto = res };
            }
            catch (Exception ex)
            {
                return new RespuestaService<PRODUCTO>() { ExcepcionCapturada = ExcepcionesHelper.ObtenerExcepcion(ex) };
            }
        }

        public RespuestaService<PRODUCTO> Actualizar(PRODUCTO p)
        {
            try
            {
                var res = _repositorio.Actualizar(p);
                return new RespuestaService<PRODUCTO>() { Objeto = res };
            }
            catch (Exception ex)
            {
                return new RespuestaService<PRODUCTO>() { ExcepcionCapturada = ExcepcionesHelper.ObtenerExcepcion(ex) };
            }
        }

        public RespuestaService<bool> Eliminar(int id)
        {
            try
            {
                PRODUCTO p = _repositorio.BuscarPorId(id);
                string Foto = p.IMAGEN_PRODUCTO;    // 20.png

                _repositorio.Eliminar(id);

                string rutaFoto = $"{_rutaImagenes}/{Foto}";  // C:\temp\20.png

                if (System.IO.File.Exists(rutaFoto))
                    System.IO.File.Delete(rutaFoto);

                return new RespuestaService<bool>() { EsValido = true };
            }
            catch (Exception ex)
            {
                return new RespuestaService<bool>() { EsValido = false, ExcepcionCapturada = ExcepcionesHelper.ObtenerExcepcion(ex) };
            }
        }

        public void AgregarFotoProducto(int productoId, string extensionArchivo)
        {
            PRODUCTO producto = _repositorio.BuscarPorId(productoId);
            producto.IMAGEN_PRODUCTO = $"{productoId}.{extensionArchivo}";

            _repositorio.ActualizarSinSP(producto);
        }

        public bool EliminarFoto(int productoId)
        {
            try
            {
                PRODUCTO producto = _repositorio.BuscarPorId(productoId);

                string foto = producto.IMAGEN_PRODUCTO;
                producto.IMAGEN_PRODUCTO = null;

                _repositorio.ActualizarSinSP(producto);

                string ruta = $"{_rutaImagenes}/{foto}";  // C:\temp\20.png

                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
