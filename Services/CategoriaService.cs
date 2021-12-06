using DataAccess;
using Mappings;
using Mappings.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Services
{
    public class CategoriaService : ICategoriaService
    {
        private Entities _context;

        public CategoriaService()
        {
            _context = new Entities();
        }

        public RespuestaService<List<CATEGORIA>> Listar()
        {
            return new RespuestaService<List<CATEGORIA>>() { Objeto = _context.CATEGORIA.ToList() };
        }

        public RespuestaService<DataTableDTO<CategoriaDTO>> LlenarDataTable(CATEGORIA categoria, int inicio, int registrosPorPagina)
        {
            if (categoria == null) categoria = new CATEGORIA();  // En caso de que sea nulo se inicializa 

            IQueryable<CATEGORIA> v = (from a in _context.CATEGORIA
                                       select a);

            if (!string.IsNullOrEmpty(categoria.NOMBRE))
                v = v.Where(a => a.NOMBRE.Contains(categoria.NOMBRE));

            int totalRegistros = v.Count();
            v = v.OrderBy(x => x.CATEGORIA_ID).Skip(inicio).Take(registrosPorPagina);

            List<CATEGORIA> lista = v.ToList();

            return new RespuestaService<DataTableDTO<CategoriaDTO>>()
            {
                Objeto = new DataTableDTO<CategoriaDTO>()
                {
                    RecordsFiltered = totalRegistros,
                    RecordsTotal = totalRegistros,
                    Data = lista.Select(Mapper.ToDTO).ToList()
                }
            };
        }

        public RespuestaService<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
        {
            int cantidadRegistros = 0;

            IQueryable<CATEGORIA> consulta = (from a in _context.CATEGORIA select a);

            if (!string.IsNullOrEmpty(busqueda))
                consulta = consulta.Where(a => a.NOMBRE.Contains(busqueda));

            cantidadRegistros = consulta.Count();
            consulta.Skip((numeroPagina - 1) * registrosPorPagina).Take(registrosPorPagina);
            List<Select2Detalle> dataSalida = consulta.Select(x => new Select2Detalle() { Id = (int) x.CATEGORIA_ID, Text = x.NOMBRE }).ToList();

            return new RespuestaService<Select2DTO>()
            {
                Objeto = new Select2DTO() { 
                    Total = cantidadRegistros, 
                    Results = dataSalida 
                }
            };
        } 

        public RespuestaService<CATEGORIA> BuscarPorId(int id)
        {
            CATEGORIA p = _context.CATEGORIA.FirstOrDefault(c => c.CATEGORIA_ID == id);
            return new RespuestaService<CATEGORIA>() { Objeto = p };
        }

        public RespuestaService<CATEGORIA> Guardar(CATEGORIA categoria)
        {
            try
            {
                _context.CATEGORIA.Add(categoria);
                _context.SaveChanges();

                return new RespuestaService<CATEGORIA>() { Objeto = categoria };
            }
            catch (Exception ex)
            {
                return new RespuestaService<CATEGORIA>() { ExcepcionCapturada = ExcepcionesHelper.ObtenerExcepcion(ex) };
            }
        }

        public RespuestaService<CATEGORIA> Actualizar(CATEGORIA categoria)
        {
            try
            {
                CATEGORIA objeto = _context.CATEGORIA.FirstOrDefault(c => c.CATEGORIA_ID == categoria.CATEGORIA_ID);

                objeto.NOMBRE = categoria.NOMBRE;

                _context.Entry(objeto).State = EntityState.Modified;
                _context.SaveChanges();
                return new RespuestaService<CATEGORIA>() { Objeto = objeto };
            }
            catch (Exception ex)
            {
                return new RespuestaService<CATEGORIA>() { ExcepcionCapturada = ExcepcionesHelper.ObtenerExcepcion(ex) };
            }
        }

        public RespuestaService<bool> Eliminar(int id)
        {
            try
            {
                CATEGORIA p = _context.CATEGORIA.FirstOrDefault(c => c.CATEGORIA_ID == id);

                _context.PRODUCTO.Where(c => c.CATEGORIA_ID == id).ToList().ForEach(producto => {
                    _context.Entry(producto).State = EntityState.Deleted;
                });

                _context.Entry(p).State = EntityState.Deleted;
                _context.SaveChanges();
                return new RespuestaService<bool>() { EsValido = true };
            }
            catch (Exception ex)
            {
                return new RespuestaService<bool>() { EsValido = false, ExcepcionCapturada = ExcepcionesHelper.ObtenerExcepcion(ex) };
            }
        }

        public RespuestaService<bool> GuardarDatosExcel(List<CATEGORIA> listaCategoria)
        {
            try
            {
                foreach (CATEGORIA categoria in listaCategoria)
                {
                    _context.CATEGORIA_INSERTAR(categoria.NOMBRE);
                }

                return new RespuestaService<bool>() { EsValido = true };
            }
            catch (Exception ex)
            {
                return new RespuestaService<bool>() { EsValido = false, ExcepcionCapturada = ExcepcionesHelper.ObtenerExcepcion(ex) };
            }
        }
    }
}
