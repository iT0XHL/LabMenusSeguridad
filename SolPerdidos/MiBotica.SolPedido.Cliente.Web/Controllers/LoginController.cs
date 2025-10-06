/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiBotica.SolPedido.Entidades.Core;
using MiBotica.SolPedido.Utiles.Helpers;
using System.Web.Security;
using MiBotica.SolPedido.LogicaNegocio.Core;
using MiBotica.SolPedido.Entidades;
using MiBotica.SolPedido.Entidades.Base;

namespace MiBotica.SolPedido.Cliente.Web.Controllers
{
    public class LoginController : BaseLN
    {
        // GET: Login
        public ActionResult Index()
        {
            Usuario u = new Usuario();
            return View(u);
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.CodUsuario) || string.IsNullOrEmpty(usuario.ClaveTexto))
            {
                //Log.Info("Llego usuario: " + usuario.CodUsuario);
                ModelState.AddModelError("*", "Debe llenar el usuario o clave");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.Clave = EncriptacionHelper.EncriptarByte(usuario.ClaveTexto);

                    Usuario res = new UsuarioLN().BuscarUsuario(usuario);

                    if (res != null && res.IdUsuario > 0) // las credenciales son correctas
                    {
                        // Llenar una cookie
                        FormsAuthentication.SetAuthCookie(res.CodUsuario, true);

                        // Llenar una sesión
                        List<Opcion> lista = new OpcionLN().ListaOpciones();

                        // Para separar los controles de las acciones en la tabla de Opciones.
                        ParsearAcciones(lista);
                        VariablesWeb.gOpciones = lista;
                        Log.Info("Llegaron las opciones: " + VariablesWeb.gOpciones.Count().ToString());
                        VariablesWeb.gUsuario = res;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("*", "Usuario / Clave no válidos");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("*", ex.Message);
                }
            }

            return View(usuario);
        }

        [NonAction]
        private void ParsearAcciones(List<Opcion> lista)
        {
            int cantidad = 0;
            foreach (Opcion item in lista)
            {
                if (!string.IsNullOrEmpty(item.UrlOpcion))
                {
                    cantidad = item.UrlOpcion.Split('/').Count();
                    switch (cantidad)
                    {
                        case 3:
                            item.Area = item.UrlOpcion.Split('/')[0];
                            item.Controladora = item.UrlOpcion.Split('/')[1];
                            item.Accion = item.UrlOpcion.Split('/')[2];
                            break;
                        case 2:
                            item.Controladora = item.UrlOpcion.Split('/')[0];
                            item.Accion = item.UrlOpcion.Split('/')[1];
                            break;
                        case 1:
                            item.Controladora = item.UrlOpcion.Split('/')[0];
                            item.Accion = "Index";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MiBotica.SolPedido.Entidades;
using MiBotica.SolPedido.Entidades.Base;
using MiBotica.SolPedido.Entidades.Core;
using MiBotica.SolPedido.LogicaNegocio.Core;
using MiBotica.SolPedido.Utiles.Helpers;

namespace MiBotica.SolPedido.Cliente.Web.Controllers
{
    public class LoginController : BaseLN
    {
        // GET: Login
        public ActionResult Index()
        {
            return View(new Usuario());
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.CodUsuario) || string.IsNullOrEmpty(usuario.ClaveTexto))
            {
                ModelState.AddModelError("*", "Debe llenar el usuario y la clave.");
                return View(usuario);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.Clave = EncriptacionHelper.EncriptarByte(usuario.ClaveTexto);

                    Usuario res = new UsuarioLN().BuscarUsuario(usuario);

                    if (res != null && res.IdUsuario > 0)
                    {
                        // Autenticación
                        FormsAuthentication.SetAuthCookie(res.CodUsuario, true);

                        // Obtener todas las opciones del sistema
                        List<Opcion> listaPlanas = new OpcionLN().ListaOpciones();

                        // Armar jerarquía de menú
                        List<Opcion> listaJerarquica = ConstruirJerarquia(listaPlanas);

                        // Analizar rutas para los helpers
                        ParsearAcciones(listaJerarquica);

                        // Guardar en sesión
                        VariablesWeb.gOpciones = listaJerarquica;
                        VariablesWeb.gUsuario = res;

                        Log.Info("Opciones cargadas en sesión: " + listaJerarquica.Count);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("*", "Usuario o clave no válidos.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("*", "Error: " + ex.Message);
                }
            }

            return View(usuario);
        }

        // 🔹 Construye la jerarquía de opciones (padres e hijos)
        [NonAction]
        private List<Opcion> ConstruirJerarquia(List<Opcion> listaPlanas)
        {
            var diccionario = listaPlanas.ToDictionary(o => o.IdOpcion, o => o);

            List<Opcion> raiz = new List<Opcion>();

            foreach (var opcion in listaPlanas)
            {
                if (opcion.IdOpcionRef.HasValue && diccionario.ContainsKey(opcion.IdOpcionRef.Value))
                {
                    diccionario[opcion.IdOpcionRef.Value].Hijos.Add(opcion);
                }
                else
                {
                    raiz.Add(opcion);
                }
            }

            return raiz.OrderBy(o => o.NroOrden).ToList();
        }

        // 🔹 Separa área/controlador/acción de la URL
        [NonAction]
        private void ParsearAcciones(List<Opcion> lista)
        {
            foreach (var item in lista)
            {
                if (!string.IsNullOrEmpty(item.UrlOpcion))
                {
                    var partes = item.UrlOpcion.Split('/');
                    switch (partes.Length)
                    {
                        case 3:
                            item.Area = partes[0];
                            item.Controladora = partes[1];
                            item.Accion = partes[2];
                            break;
                        case 2:
                            item.Controladora = partes[0];
                            item.Accion = partes[1];
                            break;
                        case 1:
                            item.Controladora = partes[0];
                            item.Accion = "Index";
                            break;
                    }
                }

                // Recursividad para hijos
                if (item.Hijos != null && item.Hijos.Count > 0)
                {
                    ParsearAcciones(item.Hijos);
                }
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
