using System;
using System.Collections.Generic;
using System.Web.Http;
using MiBotica.SolPedido.Entidades.Core;
using MiBotica.SolPedido.LogicaNegocio.Core;

namespace ApiUsuario.Controllers
{
    [RoutePrefix("api/cliente")]
    public class ClienteController : ApiController
    {
        private readonly ClienteLN _clienteLN = new ClienteLN();

        // GET: api/cliente
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                List<Cliente> lista = _clienteLN.ListarClientes();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/cliente/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Cliente cliente = _clienteLN.BuscarCliente(id);
                if (cliente == null)
                    return NotFound();

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/cliente
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Cliente value)
        {
            if (value == null)
                return BadRequest("El objeto cliente no puede ser nulo.");

            try
            {
                _clienteLN.InsertarCliente(value);
                return Ok(new { mensaje = "Cliente registrado correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/cliente/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] Cliente value)
        {
            if (value == null)
                return BadRequest("El objeto cliente no puede ser nulo.");

            try
            {
                value.Codigo = id;
                _clienteLN.ModificarCliente(value);
                return Ok(new { mensaje = "Cliente actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/cliente/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _clienteLN.EliminarCliente(id);
                return Ok(new { mensaje = "Cliente eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
