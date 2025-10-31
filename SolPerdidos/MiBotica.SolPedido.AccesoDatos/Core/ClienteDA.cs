using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MiBotica.SolPedido.Entidades.Core;

namespace MiBotica.SolPedido.AccesoDatos.Core
{
    public class ClienteDA
    {
        private string cadenaConexion = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString;

        public List<Cliente> ListarClientes()
        {
            List<Cliente> listaEntidad = new List<Cliente>();

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("paListarClientes", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        listaEntidad.Add(LlenarEntidad(reader));
                    }
                }
            }
            return listaEntidad;
        }

        public Cliente LlenarEntidad(IDataReader reader)
        {
            Cliente cliente = new Cliente();

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName='Codigo'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["Codigo"]))
                    cliente.Codigo = Convert.ToInt32(reader["Codigo"]);
            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName='NombreCompleto'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["NombreCompleto"]))
                    cliente.NombreCompleto = Convert.ToString(reader["NombreCompleto"]);
            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName='Zona'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["Zona"]))
                    cliente.Zona = Convert.ToString(reader["Zona"]);
            }

            return cliente;
        }

        public Cliente BuscarCliente(int codigo)
        {
            Cliente cliente = null;
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("paBuscarCliente", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Codigo", codigo);

                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        cliente = LlenarEntidad(reader);
                    }
                }
            }
            return cliente;
        }

        public void InsertarCliente(Cliente cliente)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("paInsertarClientes", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@NombreCompleto", cliente.NombreCompleto);
                    comando.Parameters.AddWithValue("@Zona", cliente.Zona);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ModificarCliente(Cliente cliente)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("paModificarCliente", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Codigo", cliente.Codigo);
                    comando.Parameters.AddWithValue("@NombreCompleto", cliente.NombreCompleto);
                    comando.Parameters.AddWithValue("@Zona", cliente.Zona);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void EliminarCliente(int codigo)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand("paEliminarCliente", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Codigo", codigo);

                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
