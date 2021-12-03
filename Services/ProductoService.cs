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

namespace Services
{
    public class ProductoService : IProductoService
    {
        private Entities _context;
        private string _rutaImagenes;

        public ProductoService()
        {
            _context = new Entities();
            _rutaImagenes = ConfigurationManager.AppSettings["rutaImagenesProductos"];  // Se encuentra en el Web.config
        }

        public RespuestaService<PRODUCTO> BuscarPorId(int id)
        {
            PRODUCTO p = _context.PRODUCTO.FirstOrDefault(c => c.PRODUCTO_ID == id);
            return new RespuestaService<PRODUCTO>() { Objeto = p };
        }

        public RespuestaService<List<PRODUCTO>> Listar()
        {
            return new RespuestaService<List<PRODUCTO>>() { Objeto = _context.PRODUCTO.ToList() };
        }

        public RespuestaService<DataTableDTO<ProductoDTO>> LlenarDataTable(PRODUCTO producto, int inicio, int registrosPorPagina)
        {
            try
            {
                if (producto == null) producto = new PRODUCTO();  // En caso de que sea nulo se inicializa 

                IQueryable<PRODUCTO> v = (from a in _context.PRODUCTO
                                          select a);

                if (producto.CATEGORIA_ID != 0)
                    v = v.Where(a => a.CATEGORIA_ID == producto.CATEGORIA_ID);

                if (!string.IsNullOrEmpty(producto.NOMBRE_PRODUCTO))
                    v = v.Where(a => a.NOMBRE_PRODUCTO.Contains(producto.NOMBRE_PRODUCTO));

                int totalRegistros = v.Count();
                v = v.OrderBy(x => x.PRODUCTO_ID).Skip(inicio).Take(registrosPorPagina);

                List<PRODUCTO> lista = v.ToList();

                return new RespuestaService<DataTableDTO<ProductoDTO>>()
                {
                    Objeto = new DataTableDTO<ProductoDTO>()
                    {
                        RecordsFiltered = totalRegistros,
                        RecordsTotal = totalRegistros,
                        Data = lista.Select(Mapper.ToDTO).ToList()
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
                ObjectParameter idGenerado = new ObjectParameter("id_generado", typeof(decimal));
                _context.PRODUCTO_INSERTAR(p.NOMBRE_PRODUCTO, p.PRECIO_PRODUCTO.ToString(), p.STOCK_PRODUCTO, null, p.CATEGORIA_ID, idGenerado);

                p.PRODUCTO_ID = Convert.ToDecimal(idGenerado.Value);
                return new RespuestaService<PRODUCTO>() { Objeto = p };
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
                _context.PRODUCTO_ACTUALIZAR(p.PRODUCTO_ID, p.NOMBRE_PRODUCTO, p.PRECIO_PRODUCTO.ToString(), p.STOCK_PRODUCTO, p.CATEGORIA_ID);
                return new RespuestaService<PRODUCTO>() { Objeto = p };
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
                PRODUCTO p = _context.PRODUCTO.FirstOrDefault(c => c.PRODUCTO_ID == id);
                string Foto = p.IMAGEN_PRODUCTO;    // 20.png

                _context.PRODUCTO_ELIMINAR(id);

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

        public void ActualizarFotoProducto(decimal productoId, string extensionArchivo)
        {
            PRODUCTO producto = _context.PRODUCTO.FirstOrDefault(c => c.PRODUCTO_ID == productoId);
            producto.IMAGEN_PRODUCTO = $"{productoId}.png";

            _context.Entry(producto).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool EliminarFoto(int productoId)
        {
            try
            {
                PRODUCTO producto = _context.PRODUCTO.FirstOrDefault(c => c.PRODUCTO_ID == productoId);
                string foto = producto.IMAGEN_PRODUCTO;
                producto.IMAGEN_PRODUCTO = null;

                _context.Entry(producto).State = EntityState.Modified;
                _context.SaveChanges();

                string ruta = $"{_rutaImagenes}/{foto}";  // C:\temp\20.png

                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
