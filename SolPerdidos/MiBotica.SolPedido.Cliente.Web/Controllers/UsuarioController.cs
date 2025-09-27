using MiBotica.SolPedido.Entidades.Core;
using MiBotica.SolPedido.LogicaNegocio.Core;
using MiBotica.SolPedido.Utiles.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MiBotica.SolPedido.Cliente.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioLN _usuarioLN = new UsuarioLN();

        // GET: Usuario
        public ActionResult Index(int? searchId)
        {
            var usuarios = _usuarioLN.ListaUsuarios();

            if (searchId.HasValue && searchId.Value > 0)
            {
                usuarios = usuarios.Where(u => u.IdUsuario == searchId.Value).ToList();
            }

            return View(usuarios);
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            var usuario = _usuarioLN.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View(new Usuario());
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(usuario.ClaveTexto))
                    {
                        usuario.Clave = EncriptacionHelper.EncriptarByte(usuario.ClaveTexto);
                    }

                    _usuarioLN.AgregarUsuario(usuario);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al crear el usuario: " + ex.Message);
                }
            }
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            var usuario = _usuarioLN.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            usuario.ClaveTexto = string.Empty;

            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(usuario.ClaveTexto))
                    {
                        usuario.Clave = EncriptacionHelper.EncriptarByte(usuario.ClaveTexto);
                    }
                    else
                    {
                        usuario.Clave = null; 
                    }

                    _usuarioLN.ActualizarUsuario(usuario);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar el usuario: " + ex.Message);
                }
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            var usuario = _usuarioLN.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _usuarioLN.EliminarUsuario(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al eliminar el usuario: " + ex.Message);
                return RedirectToAction("Delete", new { id });
            }
        }
    }
}
