using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using Mappings;
using Mappings.DTO;
using Services;
using WebApp.Util;
using ExcelDataReader;  // Usa la libreria ExcelDataReader.DataSet para leer archivos excel 
using System.Data;

namespace WebApp.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        ICategoriaService _servicio;
        public CategoriaController(ICategoriaService servicio)
        {
            _servicio = servicio;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LlenarSelect2(string busqueda, int registrosPorPagina, int numeroPagina)
        {
            RespuestaService<Select2DTO> result = _servicio.LlenarSelect2(busqueda, registrosPorPagina, numeroPagina);

            if (result.Objeto != null)
            {
                Select2DTO retorno = result.Objeto;
                return Json(new { Total = retorno.Total, Results = retorno.Results }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { status = result.ExcepcionCapturada.Status, error = result.ExcepcionCapturada.MensajeError });
        }

        [HttpGet]
        public ActionResult LlenarDataTable(CategoriaDTO dto, int start, int length)
        {
            RespuestaService<DataTableDTO<CategoriaDTO>> result = _servicio.LlenarDataTable(dto.ToDatabaseObject(), start, length);

            if (result.Objeto != null)
                return new JsonCamelCaseResult(result.Objeto, JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = result.ExcepcionCapturada.Status, error = result.ExcepcionCapturada.MensajeError });
        }

        [HttpGet]
        public ActionResult BuscarPorId(int id)
        {
            RespuestaService<CATEGORIA> result = _servicio.BuscarPorId(id);

            if (result.Objeto != null)
                return new JsonCamelCaseResult(result.Objeto.ToDTO(), JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = result.ExcepcionCapturada.Status, error = result.ExcepcionCapturada.MensajeError });
        }

        [HttpPost]
        public ActionResult Guardar(CategoriaDTO dto)
        {
            int status = 200;   // Status OK  
            CategoriaDTO objeto = null;

            if (ModelState.IsValid)
            {
                CATEGORIA categoria = dto.ToDatabaseObject();
                RespuestaService<CATEGORIA> result;

                result = (categoria.CATEGORIA_ID == 0) ?
                    _servicio.Guardar(categoria) : _servicio.Actualizar(categoria);

                if (result.Objeto != null)
                    objeto = result.Objeto.ToDTO();
                else
                {
                    status = result.ExcepcionCapturada.Status;
                    ModelState.AddModelError(string.Empty, result.ExcepcionCapturada.MensajeError);
                }
            }
            else status = 400;  // Bad Request

            return Json(new { objeto, status, errores = ModelState.Values.SelectMany(x => x.Errors).Select(x => new { error = x.ErrorMessage }) });
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            RespuestaService<bool> result = _servicio.Eliminar(id);

            if (result.EsValido)
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            else
                return Json(new { status = result.ExcepcionCapturada.Status, error = result.ExcepcionCapturada.MensajeError });
        }

        [HttpPost]
        public ActionResult ImportarExcel()
        {
            HttpRequest httpRequest = System.Web.HttpContext.Current.Request;
            HttpPostedFile archivo = httpRequest.Files["archivo"];

            if (archivo == null)
                return Json(new { status = 400, error = "El archivo es requerido" });

            try
            {
                if (archivo.ContentType == "application/vnd.ms-excel" || archivo.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    Stream stream = archivo.InputStream;
                    List<CategoriaDTO> listaCategoriaDTO = new List<CategoriaDTO>();

                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        if (reader.AsDataSet().Tables.Count > 1) 
                            throw new ArgumentException("El archivo recibido posee más de una hoja"); 

                        DataTable pagina = reader.AsDataSet().Tables[0];
                        int contadorFila = 1;  

                        if (pagina.Rows.Count == 0)
                            throw new ArgumentException("La hoja 1 del archivo excel no posee registros");

                        for (int i = 0; i < pagina.Rows.Count; i++)
                        {
                            DataRow fila = pagina.Rows[i];

                            string Nombre = fila[0].ToString();

                            if (string.IsNullOrEmpty(Nombre))
                                throw new ArgumentException($"El campo Nombre de la fila {contadorFila} esta vacío");

                            CategoriaDTO categoria = new CategoriaDTO();

                            categoria.Nombre = Nombre;
                            listaCategoriaDTO.Add(categoria);
                            contadorFila++;
                        }
                    }

                    List<CATEGORIA> listaCategoria = listaCategoriaDTO.Select(Mapper.ToDatabaseObject).ToList();
                    RespuestaService<bool> result = _servicio.GuardarDatosExcel(listaCategoria);

                    if (result.EsValido)
                        return Json(new { status = 200 });
                    else
                        return Json(new { status = result.ExcepcionCapturada.Status, error = result.ExcepcionCapturada.MensajeError });
                }
                else
                {
                    return Json(new { status = 400, error = "El archivo debe ser en formato xls" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { status = 500, error = ex.Message });
            }
        }
    }
}
