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
using Mappings.DTO;
using System.Configuration;
using Rotativa;
using OfficeOpenXml;    // Usa la libreria EPPLUS para generar excel. Hasta la version 4 es open source 
using System.IO;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private IProductoService _servicio;
        private string _rutaImagenes;

        public ProductoController(IProductoService servicio)
        {
            _servicio = servicio;
            _rutaImagenes = ConfigurationManager.AppSettings["rutaImagenesProductos"];  // Se encuentra en el Web.config
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
                return Json(new { status = result.ExcepcionCapturada.Status, error = "No se encontraron resultados" });
        }

        [HttpGet]
        public ActionResult LlenarDataTable(ProductoDTO dto, int start, int length)
        {
            RespuestaService<DataTableDTO<ProductoDTO>> result = _servicio.LlenarDataTable(dto.ToDatabaseObject(), start, length);

            if (result.Objeto != null)
            {
                result.Objeto.Data = result.Objeto.Data.Select(x => AgregarFotoProductoImagenBase64(x)).ToList();  // Agrega a la lista la foto en base 64
                return new JsonCamelCaseResult(result.Objeto, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { status = result.ExcepcionCapturada.Status, error = result.ExcepcionCapturada.MensajeError });
        }

        [HttpGet]
        public ActionResult BuscarPorId(int id)
        {
            RespuestaService<PRODUCTO> result = _servicio.BuscarPorId(id);

            if (result.Objeto != null)
                return new JsonCamelCaseResult(result.Objeto.ToDTO(), JsonRequestBehavior.AllowGet);
            else
                return Json(new { status = result.ExcepcionCapturada.Status, error = result.ExcepcionCapturada.MensajeError });
        }

        [HttpPost]
        public ActionResult Guardar(ProductoDTO dto)
        {
            int status = 200;   // Status OK  
            ProductoDTO objeto = null;

            if (ModelState.IsValid)
            {
                PRODUCTO producto = dto.ToDatabaseObject();
                RespuestaService<PRODUCTO> result;

                result = (producto.PRODUCTO_ID == 0) ?
                    _servicio.Guardar(producto) : _servicio.Actualizar(producto);

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
        public ActionResult AgregarFotoProducto(ProductoDTO producto)
        {
            // HttpPostedFileBase fotoProducto = Request.Files["fotoProducto"];   // Asi tambien se puede obtener la foto
            HttpPostedFileBase fotoProducto = producto.FotoProducto;

            if (producto.ProductoId == 0)
                return Json(new { status = 400, error = "El producto no trae id" });

            if (fotoProducto == null)
                return Json(new { status = 400, error = "La imagen es requerida" });

            if (fotoProducto.ContentLength > 0 && !fotoProducto.ContentType.Contains("image"))  // Si se recibio un archivo pero no es una foto
                return Json(new { status = 400, error = "El archivo debe ser una imagen" });

            try
            {
                string extensionArchivo = fotoProducto.FileName.Split('.')[1];

                string cadenaArchivo = $"{_rutaImagenes}/{producto.ProductoId}.{extensionArchivo}";
                fotoProducto.SaveAs(cadenaArchivo);

                _servicio.AgregarFotoProducto(Convert.ToInt32(producto.ProductoId), extensionArchivo);
                return Json(new { status = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { status = 500, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult BorrarFoto(int productId)
        {
            bool result = _servicio.EliminarFoto(productId);

            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
                return Json(new { status = 500, error = "Se encontró un error" });
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

        public ActionResult ImprimirPDF()
        {
            RespuestaService<List<PRODUCTO>> result = _servicio.Listar();

            if (result.Objeto != null)
            {
                List<ProductoDTO> listaProductoDTO = result.Objeto.Select(Mapper.ToDTO).ToList()
                    .Select(x => AgregarFotoProductoImagenBase64(x)).ToList();                    // Agrega a la lista la foto en base 64

                return new PartialViewAsPdf("_imprimirPDF", listaProductoDTO) { FileName = "ReporteProductos.pdf" };
            }
            else
            {
                return Json(new { status = 500, error = "" });
            }
        }

        private ProductoDTO AgregarFotoProductoImagenBase64(ProductoDTO producto)
        {
            if (!string.IsNullOrEmpty(producto.RutaImagenProducto))
            {
                string ruta = $"{_rutaImagenes}\\{producto.RutaImagenProducto}";     // C:\temp\20.png

                if (System.IO.File.Exists(ruta))
                {
                    byte[] imagenByte = System.IO.File.ReadAllBytes(ruta);
                    string imageBase64Data = Convert.ToBase64String(imagenByte);

                    string extensionArchivo = producto.RutaImagenProducto.Split('.')[1];  // png
                    producto.RutaImagenProducto = $"data:image/{extensionArchivo};base64,{imageBase64Data}";
                }
            }

            return producto;
        }


        [HttpGet]
        public ActionResult GenerarExcel( )
        {
            RespuestaService<List<PRODUCTO>> result = _servicio.Listar();
            List<ProductoDTO> listaProductoDTO = result.Objeto.Select(Mapper.ToDTO).ToList();

            try
            {
                ExcelPackage package = new ExcelPackage();
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Hoja 1");
                int indice = 1;

                // Header del archivo 

                sheet.Cells[$"A{indice}"].Value = "Id";
                sheet.Cells[$"B{indice}"].Value = "Imagen";
                sheet.Cells[$"C{indice}"].Value = "Precio";
                sheet.Cells[$"D{indice}"].Value = "Stock";
                sheet.Cells[$"E{indice}"].Value = "Categoría";
                sheet.Cells[$"F{indice}"].Value = "Nombre";
                indice++;

                foreach (ProductoDTO c in listaProductoDTO)
                {
                    sheet.Cells[$"A{indice}"].Value = c.ProductoId;
                    sheet.Cells[$"B{indice}"].Value = c.RutaImagenProducto;
                    sheet.Cells[$"C{indice}"].Value = c.PrecioProducto;
                    sheet.Cells[$"D{indice}"].Value = c.StockProducto;
                    sheet.Cells[$"E{indice}"].Value = c.Categoria.CategoriaId;
                    sheet.Cells[$"F{indice}"].Value = c.NombreProducto;
                    indice++;
                }

                sheet.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = " application / application / vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                string name = "ReporteProductos.xlsx";
                Response.AppendHeader("content-disposition", " attachment; filename =" + name);
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();
                return Index();
            }
            catch (Exception ex)
            {
                Response.StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError);
                return Json(new { mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
