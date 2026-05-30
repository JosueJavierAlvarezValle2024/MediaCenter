using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace MediaCenter.Datos
{
    internal class ConexionSQL
    {

        private static string cadenaConexion =
        @"Server=HPCOMPUTER18\SQLEXPRESS01;Database=MediaCenterDB;Integrated Security=True;TrustServerCertificate=True;";


        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        
        public static void InsertarArchivo(string nombre, string ruta, string tipo,
                                            string extension, decimal tamanoKB, bool estaCorrupto)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                string sql = @"INSERT INTO Archivos 
                       (Nombre, RutaCompleta, Tipo, Extension, TamanoKB, EstaCorrupto) 
                       VALUES (@nombre, @ruta, @tipo, @ext, @tam, @corrupto)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@ruta", ruta);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@ext", extension);
                    cmd.Parameters.AddWithValue("@tam", tamanoKB);
                    cmd.Parameters.AddWithValue("@corrupto", estaCorrupto);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        
        public static DataTable ObtenerTodosLosArchivos()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                string sql = "SELECT * FROM Archivos ORDER BY IdArchivo ASC";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    adapter.Fill(tabla);
                }
            }

            return tabla;
        }


        
        public static void ActualizarArchivo(int idArchivo, string nombre, string ruta,
                                              string tipo, string extension, decimal tamanoKB, bool estaCorrupto)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                string sql = @"UPDATE Archivos 
                       SET Nombre = @nombre, RutaCompleta = @ruta, Tipo = @tipo, 
                           Extension = @ext, TamanoKB = @tam, EstaCorrupto = @corrupto
                       WHERE IdArchivo = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idArchivo);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@ruta", ruta);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@ext", extension);
                    cmd.Parameters.AddWithValue("@tam", tamanoKB);
                    cmd.Parameters.AddWithValue("@corrupto", estaCorrupto);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        
        public static void EliminarArchivo(int idArchivo)
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                string sql = "DELETE FROM Archivos WHERE IdArchivo = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idArchivo);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public static string ObtenerCadena()
        {
            return cadenaConexion;
        }

        public static int ContarPorTipo(string tipo)
        {
            int total = 0;
            using (SqlConnection conn = ObtenerConexion())
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Archivos WHERE Tipo = @tipo";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    total = (int)cmd.ExecuteScalar();
                }
            }
            return total;
        }

    }   

}
