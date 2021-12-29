using DataAccess;
using Mappings;
using Mappings.DTO;
using Repository;
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
        private readonly ICategoriaRepository _repositorio;

        public CategoriaService(ICategoriaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public RespuestaService<List<CATEGORIA>> Listar()
        {
            var lista = _repositorio.Listar();
            return new RespuestaService<List<CATEGORIA>>() { Objeto = lista };
        }

        public RespuestaService<DataTableDTO<CategoriaDTO>> LlenarDataTable(CATEGORIA categoria, int inicio, int registrosPorPagina)
        {
            if (categoria == null) categoria = new CATEGORIA();  // En caso de que sea nulo se inicializa 

            (List<CATEGORIA>, int) res = _repositorio.ListarConPaginacion(categoria, inicio, registrosPorPagina);

            return new RespuestaService<DataTableDTO<CategoriaDTO>>()
            {
                Objeto = new DataTableDTO<CategoriaDTO>()
                {
                    Data = res.Item1.Select(Mapper.ToDTO).ToList(),
                    RecordsFiltered = res.Item2,
                    RecordsTotal = res.Item2
                }
            };
        }

        public RespuestaService<Select2DTO> LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
        {
            (IQueryable<CATEGORIA>, int) res = _repositorio.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina);
            List<Select2Detalle> dataSalida = res.Item1.Select(x => new Select2Detalle() { Id = (int)x.CATEGORIA_ID, Text = x.NOMBRE }).ToList();

            return new RespuestaService<Select2DTO>()
            {
                Objeto = new Select2DTO()
                {
                    Total = res.Item2,
                    Results = dataSalida
                }
            };
        } 

        public RespuestaService<CATEGORIA> BuscarPorId(int id)
        {
            var res = _repositorio.BuscarPorId(id);
            return new RespuestaService<CATEGORIA>() { Objeto = res };
        }

        public RespuestaService<CATEGORIA> Guardar(CATEGORIA categoria)
        {
            try
            {
                var res = _repositorio.Guardar(categoria);
                return new RespuestaService<CATEGORIA>() { Objeto = res };
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
                var res = _repositorio.Actualizar(categoria);
                return new RespuestaService<CATEGORIA>() { Objeto = res };
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
                var res = _repositorio.Eliminar(id);
                return new RespuestaService<bool>() { EsValido = res };
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
                    _repositorio.InsertarSP(categoria);
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
