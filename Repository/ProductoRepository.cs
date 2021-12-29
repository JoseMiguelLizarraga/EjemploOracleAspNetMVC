using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly Entities _context;

        public ProductoRepository()
        {
            _context = new Entities();
        }

        public List<PRODUCTO> Listar()
        {
            return _context.PRODUCTO.ToList();
        }

        public (List<PRODUCTO>, int) ListarConPaginacion(PRODUCTO producto, int inicio, int registrosPorPagina)
        {
            IQueryable<PRODUCTO> v = (from a in _context.PRODUCTO
                                      select a);

            if (producto.CATEGORIA_ID != 0)
                v = v.Where(a => a.CATEGORIA_ID == producto.CATEGORIA_ID);

            if (!string.IsNullOrEmpty(producto.NOMBRE_PRODUCTO))
                v = v.Where(a => a.NOMBRE_PRODUCTO.Contains(producto.NOMBRE_PRODUCTO));

            int totalRegistros = v.Count();
            v = v.OrderBy(x => x.PRODUCTO_ID).Skip(inicio).Take(registrosPorPagina);

            return (v.ToList(), totalRegistros);
        }

        public PRODUCTO BuscarPorId(int id)
        {
            return _context.PRODUCTO.FirstOrDefault(c => c.PRODUCTO_ID == id);
        }

        public PRODUCTO Guardar(PRODUCTO p)
        {
            ObjectParameter idGenerado = new ObjectParameter("id_generado", typeof(decimal));
            _context.PRODUCTO_INSERTAR(p.NOMBRE_PRODUCTO, p.PRECIO_PRODUCTO.ToString(), p.STOCK_PRODUCTO, null, p.CATEGORIA_ID, idGenerado);

            p.PRODUCTO_ID = Convert.ToDecimal(idGenerado.Value);
            return p;
        }

        public PRODUCTO Actualizar(PRODUCTO p)
        {
            _context.PRODUCTO_ACTUALIZAR(p.PRODUCTO_ID, p.NOMBRE_PRODUCTO, p.PRECIO_PRODUCTO.ToString(), p.STOCK_PRODUCTO, p.CATEGORIA_ID);
            return p;
        }

        public void ActualizarSinSP(PRODUCTO producto)
        {
            _context.Entry(producto).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            _context.PRODUCTO_ELIMINAR(id);
        }
    }
}
