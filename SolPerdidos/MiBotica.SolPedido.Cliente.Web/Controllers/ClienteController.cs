using MiBotica.SolPedido.Entidades.Core;
using MiBotica.SolPedido.LogicaNegocio.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace MiBotica.SolPedido.Cliente.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly string RutaApi = "https://localhost:44300/api/";
        private readonly string jsonMediaType = "application/json";

        public ClienteController()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        // GET: Clientes
        public ActionResult Index(int? searchCodigo = null)
        {
            List<Entidades.Core.Cliente> lista = new List<Entidades.Core.Cliente>();

            try
            {
                using (WebClient cliente = new WebClient())
                {
                    cliente.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    cliente.Encoding = UTF8Encoding.UTF8;

                    string rutacompleta = RutaApi + "cliente";
                    var data = cliente.DownloadString(new Uri(rutacompleta));

                    lista = JsonConvert.DeserializeObject<List<Entidades.Core.Cliente>>(data);
                }

                // Si se busca por código
                if (searchCodigo.HasValue)
                {
                    lista = lista
                        .Where(c => c.Codigo == searchCodigo.Value)
                        .ToList();

                    if (!lista.Any())
                    {
                        ViewBag.Error = $"No se encontró usuario con el código {searchCodigo}.";
                    }
                }
            }
            catch (WebException ex)
            {
                using (var reader = new StreamReader(ex.Response?.GetResponseStream() ?? Stream.Null))
                {
                    string respuestaError = reader.ReadToEnd();
                    ViewBag.Error = $"Error al conectar con la API: {ex.Message} - {respuestaError}";
                }
            }

            return View(lista);
        }


        // GET: Cliente/Details/5
        public ActionResult Details(int id)
        {
            Entidades.Core.Cliente obj = null;
            using (WebClient cliente = new WebClient())
            {
                cliente.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                cliente.Encoding = UTF8Encoding.UTF8;

                string rutacompleta = RutaApi + "cliente/" + id;
                var data = cliente.DownloadString(new Uri(rutacompleta));
                obj = JsonConvert.DeserializeObject<Entidades.Core.Cliente>(data);
            }
            return View(obj);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        public ActionResult Create(Entidades.Core.Cliente nuevo)
        {
            try
            {
                using (WebClient cliente = new WebClient())
                {
                    cliente.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    cliente.Encoding = UTF8Encoding.UTF8;

                    string json = JsonConvert.SerializeObject(nuevo);
                    cliente.UploadString(new Uri(RutaApi + "cliente"), "POST", json);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear cliente: " + ex.Message;
                return View(nuevo);
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            Entidades.Core.Cliente obj = null;
            using (WebClient cliente = new WebClient())
            {
                cliente.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                cliente.Encoding = UTF8Encoding.UTF8;

                string rutacompleta = RutaApi + "cliente/" + id;
                var data = cliente.DownloadString(new Uri(rutacompleta));
                obj = JsonConvert.DeserializeObject<Entidades.Core.Cliente>(data);
            }
            return View(obj);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(Entidades.Core.Cliente editado)
        {
            try
            {
                using (WebClient cliente = new WebClient())
                {
                    cliente.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    cliente.Encoding = UTF8Encoding.UTF8;

                    string json = JsonConvert.SerializeObject(editado);
                    cliente.UploadString(new Uri(RutaApi + "cliente/" + editado.Codigo), "PUT", json);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al editar cliente: " + ex.Message;
                return View(editado);
            }
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            Entidades.Core.Cliente obj = null;
            using (WebClient cliente = new WebClient())
            {
                cliente.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                cliente.Encoding = UTF8Encoding.UTF8;

                string rutacompleta = RutaApi + "cliente/" + id;
                var data = cliente.DownloadString(new Uri(rutacompleta));
                obj = JsonConvert.DeserializeObject<Entidades.Core.Cliente>(data);
            }
            return View(obj);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (WebClient cliente = new WebClient())
                {
                    cliente.Headers[HttpRequestHeader.ContentType] = jsonMediaType;
                    cliente.Encoding = UTF8Encoding.UTF8;
                    cliente.UploadString(new Uri(RutaApi + "cliente/" + id), "DELETE", "");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar cliente: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
