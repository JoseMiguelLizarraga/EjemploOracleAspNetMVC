using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;
using Mappings;
using DataAccess;
using System.Net;
using WebApp.Util;

namespace WebApp.Controllers
{
    public class ProductoController : Controller
    {
        private IProductoService _servicio;

        public ProductoController(IProductoService servicio)
        {
            _servicio = servicio;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Listar()
        {
            RespuestaService<List<PRODUCTO>> result = _servicio.Listar();

            if (result.Objeto != null)
                return new JsonCamelCaseResult(result.Objeto.Select(Mapper.ToDTO).ToList(), JsonRequestBehavior.AllowGet);
            else
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "No se encontraron resultados");
        }

        [HttpGet]
        public ActionResult LlenarDataTable(ProductoDTO dto, int start, int length)
        {
            RespuestaService<DataTableDTO> result = _servicio.LlenarDataTable(dto.ToDatabaseObject(), start, length);

            if (result.Objeto != null)
                return new JsonCamelCaseResult(result.Objeto, JsonRequestBehavior.AllowGet);

            else if (result.ExcepcionCapturada.Status == 400)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.ExcepcionCapturada.MensajeError);

            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, result.ExcepcionCapturada.MensajeError);
        }

        [HttpGet]
        public ActionResult BuscarPorId(int id)
        {
            RespuestaService<PRODUCTO> result = _servicio.BuscarPorId(id);

            if (result.Objeto != null)
                return new JsonCamelCaseResult(result.Objeto.ToDTO(), JsonRequestBehavior.AllowGet);

            else if (result.ExcepcionCapturada.Status == 400)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.ExcepcionCapturada.MensajeError);

            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, result.ExcepcionCapturada.MensajeError);
        }

        [HttpPost]
        public ActionResult Guardar(ProductoDTO dto)
        {
            PRODUCTO producto = dto.ToDatabaseObject();
            RespuestaService<PRODUCTO> result;

            result = (producto.PRODUCTO_ID == 0) ?
                _servicio.Guardar(producto) : _servicio.Actualizar(producto);

            if (result.Objeto != null)
                return new JsonCamelCaseResult(result.Objeto.ToDTO(), JsonRequestBehavior.AllowGet);

            else if (result.ExcepcionCapturada.Status == 400)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.ExcepcionCapturada.MensajeError);

            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, result.ExcepcionCapturada.MensajeError);
        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            RespuestaService<bool> result = _servicio.Eliminar(id);

            if (result.EsValido)
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, result.ExcepcionCapturada.MensajeError);
        }
    }
}
