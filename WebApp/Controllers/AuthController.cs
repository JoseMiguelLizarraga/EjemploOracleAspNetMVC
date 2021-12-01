using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using System.Web.Security;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario usuario, string ReturnUrl)  
        {
            // ReturnUrl es para que, al intentar entrar a una pagina, luego de iniciar sesion, entre a esa pagina

            if (IsValid(usuario))
            {
                // Le paso el email a la cookie. El segundo param es si queremos que persista despues de cerrar el navegador
                FormsAuthentication.SetAuthCookie(usuario.Email, false);

                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);

                return RedirectToAction("Index", "Home");
            }

            TempData["mensaje"] = "Credenciales incorrectas";
            return View(usuario);
        }

        private bool IsValid(Usuario usuario)
        {
            if (usuario.Email == "admin@admin.cl" && usuario.Password == "admin")
                return true;
            else
                return false;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}