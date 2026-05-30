using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace MediaCenter.Servicios
{
    internal class EstadisticasServicio
    {
        private string _connectionString;

        public EstadisticasServicio(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ── Cuenta cuántos archivos hay de un tipo específico ──
        // tipo puede ser: "Foto", "Musica" o "Video"
        public int ContarPorTipo(string tipo)
        {
            int total = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM dbo.Archivos WHERE Tipo = @tipo";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // @tipo evita inyección SQL — buena práctica siempre
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    total = (int)cmd.ExecuteScalar();
                }
            }

            return total;
        }

        public List<(string Nombre, string Tipo, DateTime Fecha)> ObtenerRecientes(int cantidad)
        {
            var lista = new List<(string, string, DateTime)>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT TOP (@n) Nombre, Tipo, FechaAgregado 
                       FROM dbo.Archivos 
                       ORDER BY FechaAgregado DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@n", cantidad);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add((
                                reader["Nombre"].ToString(),
                                reader["Tipo"].ToString(),
                                Convert.ToDateTime(reader["FechaAgregado"])
                            ));
                        }
                    }
                }
            }

            return lista;
        }

        // Obtener todos los archivos de un tipo específico
        public List<(int Id, string Nombre, string RutaCompleta)>
            ObtenerArchivosPorTipo(string tipo)
        {
            var lista = new List<(int, string, string)>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT IdArchivo, Nombre, RutaCompleta 
                       FROM dbo.Archivos 
                       WHERE Tipo = @tipo
                       ORDER BY FechaAgregado DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add((
                                Id: Convert.ToInt32(reader["IdArchivo"]),
                                Nombre: reader["Nombre"].ToString(),
                                RutaCompleta: reader["RutaCompleta"].ToString()
                            ));
                        }
                    }
                }
            }
            return lista;
        }


    }
}
