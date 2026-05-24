using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using MediaCenter.Servicios;
using System.Linq;



namespace MediaCenter.Servicios
{
    internal class ValidarArchivos
    {
        // ============================================================
        // MÉTODO 1: Validar IMAGEN
        // ============================================================
        public static bool EsImagenValida(string ruta, out string mensajeError)
        {
            mensajeError = "";

            // Validación básica: que exista
            if (!File.Exists(ruta))
            {
                mensajeError = "El archivo no existe.";
                return false;
            }

            // Validación de tamaño: no debe estar vacío
            FileInfo info = new FileInfo(ruta);
            if (info.Length == 0)
            {
                mensajeError = "El archivo está vacío (0 bytes).";
                return false;
            }

            // Leemos los primeros bytes para revisar la "huella digital" (magic numbers)
            byte[] cabecera = LeerCabecera(ruta, 8);
            if (cabecera == null)
            {
                mensajeError = "No se pudo leer el archivo.";
                return false;
            }

            // Verificamos magic numbers de los formatos de imagen comunes
            bool esJPG = cabecera[0] == 0xFF && cabecera[1] == 0xD8 && cabecera[2] == 0xFF;
            bool esPNG = cabecera[0] == 0x89 && cabecera[1] == 0x50 && cabecera[2] == 0x4E && cabecera[3] == 0x47;
            bool esGIF = cabecera[0] == 0x47 && cabecera[1] == 0x49 && cabecera[2] == 0x46;
            bool esBMP = cabecera[0] == 0x42 && cabecera[1] == 0x4D;
            bool esWEBP = cabecera[0] == 0x52 && cabecera[1] == 0x49 && cabecera[2] == 0x46 && cabecera[3] == 0x46;

            if (!(esJPG || esPNG || esGIF || esBMP || esWEBP))
            {
                mensajeError = "El archivo NO es una imagen real (tiene extensión cambiada o está dañado).";
                return false;
            }

            // Prueba final: intentar abrirlo como imagen. Si truena, está dañado
            try
            {
                using (Image img = Image.FromFile(ruta))
                {
                    // Si llegamos aquí, la imagen abrió correctamente
                    return true;
                }
            }
            catch
            {
                mensajeError = "La imagen está dañada y no se puede abrir.";
                return false;
            }
        }

        // ============================================================
        // MÉTODO 2: Validar AUDIO
        // ============================================================
        public static bool EsAudioValido(string ruta, out string mensajeError)
        {
            mensajeError = "";

            if (!File.Exists(ruta))
            {
                mensajeError = "El archivo no existe.";
                return false;
            }

            FileInfo info = new FileInfo(ruta);
            if (info.Length == 0)
            {
                mensajeError = "El archivo está vacío (0 bytes).";
                return false;
            }

            byte[] cabecera = LeerCabecera(ruta, 12);
            if (cabecera == null)
            {
                mensajeError = "No se pudo leer el archivo.";
                return false;
            }

            // Magic numbers de audio comunes
            // MP3 con tag ID3: "ID3" al inicio
            bool esMP3_ID3 = cabecera[0] == 0x49 && cabecera[1] == 0x44 && cabecera[2] == 0x33;
            // MP3 sin tag: empieza con FF FB o FF F3 o FF F2
            bool esMP3_Raw = cabecera[0] == 0xFF && (cabecera[1] == 0xFB || cabecera[1] == 0xF3 || cabecera[1] == 0xF2);
            // WAV: "RIFF" en bytes 0-3 y "WAVE" en bytes 8-11
            bool esWAV = cabecera[0] == 0x52 && cabecera[1] == 0x49 && cabecera[2] == 0x46 && cabecera[3] == 0x46
                      && cabecera[8] == 0x57 && cabecera[9] == 0x41 && cabecera[10] == 0x56 && cabecera[11] == 0x45;
            // FLAC: "fLaC"
            bool esFLAC = cabecera[0] == 0x66 && cabecera[1] == 0x4C && cabecera[2] == 0x61 && cabecera[3] == 0x43;
            // OGG: "OggS"
            bool esOGG = cabecera[0] == 0x4F && cabecera[1] == 0x67 && cabecera[2] == 0x67 && cabecera[3] == 0x53;
            // M4A/AAC: contienen "ftyp" en bytes 4-7
            bool esM4A = cabecera[4] == 0x66 && cabecera[5] == 0x74 && cabecera[6] == 0x79 && cabecera[7] == 0x70;

            if (!(esMP3_ID3 || esMP3_Raw || esWAV || esFLAC || esOGG || esM4A))
            {
                mensajeError = "El archivo NO es audio real (tiene extensión cambiada o está dañado).";
                return false;
            }

            return true;
        }

        // ============================================================
        // MÉTODO 3: Validar VIDEO
        // ============================================================
        public static bool EsVideoValido(string ruta, out string mensajeError)
        {
            mensajeError = "";

            if (!File.Exists(ruta))
            {
                mensajeError = "El archivo no existe.";
                return false;
            }

            FileInfo info = new FileInfo(ruta);
            if (info.Length == 0)
            {
                mensajeError = "El archivo está vacío (0 bytes).";
                return false;
            }

            byte[] cabecera = LeerCabecera(ruta, 12);
            if (cabecera == null)
            {
                mensajeError = "No se pudo leer el archivo.";
                return false;
            }

            // MP4/MOV/M4V: contienen "ftyp" en bytes 4-7
            bool esMP4 = cabecera[4] == 0x66 && cabecera[5] == 0x74 && cabecera[6] == 0x79 && cabecera[7] == 0x70;
            // AVI: "RIFF" + "AVI "
            bool esAVI = cabecera[0] == 0x52 && cabecera[1] == 0x49 && cabecera[2] == 0x46 && cabecera[3] == 0x46
                      && cabecera[8] == 0x41 && cabecera[9] == 0x56 && cabecera[10] == 0x49;
            // WMV/ASF: 30 26 B2 75
            bool esWMV = cabecera[0] == 0x30 && cabecera[1] == 0x26 && cabecera[2] == 0xB2 && cabecera[3] == 0x75;
            // MKV/WebM: 1A 45 DF A3
            bool esMKV = cabecera[0] == 0x1A && cabecera[1] == 0x45 && cabecera[2] == 0xDF && cabecera[3] == 0xA3;
            // FLV: "FLV"
            bool esFLV = cabecera[0] == 0x46 && cabecera[1] == 0x4C && cabecera[2] == 0x56;

            if (!(esMP4 || esAVI || esWMV || esMKV || esFLV))
            {
                mensajeError = "El archivo NO es video real (tiene extensión cambiada o está dañado).";
                return false;
            }

            return true;
        }

        // ============================================================
        // MÉTODO AUXILIAR: Leer los primeros N bytes de un archivo
        // ============================================================
        private static byte[] LeerCabecera(string ruta, int cantidadBytes)
        {
            try
            {
                using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[cantidadBytes];
                    int leidos = fs.Read(buffer, 0, cantidadBytes);

                    // Si el archivo es más corto que lo que pedimos, está corrupto
                    if (leidos < cantidadBytes) return null;

                    return buffer;
                }
            }
            catch
            {
                // Si no podemos leerlo (permisos, bloqueado, etc.) lo tratamos como inválido
                return null;
            }
        }



    }
}
