using System;
using System.Collections.Generic;
using MiBotica.SolPedido.Entidades.Core;
using MiBotica.SolPedido.AccesoDatos.Core;
using MiBotica.SolPedido.Entidades.Base;

namespace MiBotica.SolPedido.LogicaNegocio.Core
{
    public class ClienteLN : BaseLN
    {
        private readonly ClienteDA _clienteDA = new ClienteDA();

        public List<Cliente> ListarClientes()
        {
            try
            {
                return _clienteDA.ListarClientes();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Cliente BuscarCliente(int codigo)
        {
            try
            {
                return _clienteDA.BuscarCliente(codigo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertarCliente(Cliente cliente)
        {
            try
            {
                _clienteDA.InsertarCliente(cliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ModificarCliente(Cliente cliente)
        {
            try
            {
                _clienteDA.ModificarCliente(cliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EliminarCliente(int codigo)
        {
            try
            {
                _clienteDA.EliminarCliente(codigo);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
