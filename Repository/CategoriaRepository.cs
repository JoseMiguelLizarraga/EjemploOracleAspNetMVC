using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly Entities _context;

        public CategoriaRepository()
        {
            _context = new Entities();
        }

        public CATEGORIA BuscarPorId(int id)
        {
            return _context.CATEGORIA.FirstOrDefault(c => c.CATEGORIA_ID == id);
        }

        public List<CATEGORIA> Listar()
        {
            return _context.CATEGORIA.ToList();
        }

        public (List<CATEGORIA>, int) ListarConPaginacion(CATEGORIA categoria, int inicio, int registrosPorPagina)
        {
            IQueryable<CATEGORIA> v = (from a in _context.CATEGORIA
                                       select a);

            if (!string.IsNullOrEmpty(categoria.NOMBRE))
                v = v.Where(a => a.NOMBRE.Contains(categoria.NOMBRE));

            int totalRegistros = v.Count();
            v = v.OrderBy(x => x.CATEGORIA_ID).Skip(inicio).Take(registrosPorPagina);

            return (v.ToList(), totalRegistros);
        }

        public (IQueryable<CATEGORIA>, int) LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
        {
            int cantidadRegistros = 0;

            IQueryable<CATEGORIA> consulta = (from a in _context.CATEGORIA select a);

            if (!string.IsNullOrEmpty(busqueda))
                consulta = consulta.Where(a => a.NOMBRE.Contains(busqueda));

            cantidadRegistros = consulta.Count();
            consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina);

            return (consulta, cantidadRegistros);
        }

        public CATEGORIA Guardar(CATEGORIA categoria)
        {
            _context.CATEGORIA.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public CATEGORIA Actualizar(CATEGORIA categoria)
        {
            CATEGORIA objeto = _context.CATEGORIA.FirstOrDefault(c => c.CATEGORIA_ID == categoria.CATEGORIA_ID);

            objeto.NOMBRE = categoria.NOMBRE;

            _context.Entry(objeto).State = EntityState.Modified;
            _context.SaveChanges();
            return objeto;
        }

        public bool Eliminar(int id)
        {
            CATEGORIA p = _context.CATEGORIA.FirstOrDefault(c => c.CATEGORIA_ID == id);

            _context.PRODUCTO.Where(c => c.CATEGORIA_ID == id).ToList().ForEach(producto => {
                _context.Entry(producto).State = EntityState.Deleted;
            });

            _context.Entry(p).State = EntityState.Deleted;
            _context.SaveChanges();
            return true;
        }

        public void InsertarSP(CATEGORIA categoria)
        {
            _context.CATEGORIA_INSERTAR(categoria.NOMBRE);
        }
    }
}
