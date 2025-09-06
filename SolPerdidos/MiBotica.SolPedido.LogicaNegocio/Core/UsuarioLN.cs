using System;
using System.Collections.Generic;
using MiBotica.SolPedido.Entidades.Core;
using MiBotica.SolPedido.AccesoDatos.Core;
using MiBotica.SolPedido.Entidades.Base;

namespace MiBotica.SolPedido.LogicaNegocio.Core
{
    public class UsuarioLN : BaseLN
    {
        private readonly UsuarioDA _usuarioDA = new UsuarioDA();

        public List<Usuario> ListaUsuarios()
        {
            try
            {
                return _usuarioDA.ListaUsuarios();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            try
            {
                return _usuarioDA.ObtenerUsuarioPorId(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AgregarUsuario(Usuario usuario)
        {
            try
            {
                _usuarioDA.AgregarUsuario(usuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            try
            {
                _usuarioDA.ActualizarUsuario(usuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EliminarUsuario(int id)
        {
            try
            {
                _usuarioDA.EliminarUsuario(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
