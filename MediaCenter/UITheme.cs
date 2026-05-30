using System;
using System.Collections.Generic;
using System.Text;

namespace MediaCenter
{
    internal class UITheme
    {
        // --- Fondos ---
        public static Color SidebarBg = Color.FromArgb(10, 22, 40);   // azul muy oscuro
        public static Color SidebarHover = Color.FromArgb(19, 34, 56);   // hover del botón
        public static Color SidebarActive = Color.FromArgb(21, 101, 192);   // botón seleccionado
        public static Color ContentBg = Color.FromArgb(13, 31, 60);   // área principal
        public static Color TopBarBg = Color.FromArgb(10, 22, 40);   // barra superior

        // --- Texto ---
        public static Color TextPrimary = Color.FromArgb(255, 255, 255);  // blanco
        public static Color TextSecondary = Color.FromArgb(142, 180, 212);  // azul claro
        public static Color TextMuted = Color.FromArgb(61, 106, 154);  // azul grisáceo

        // --- Acento ---
        public static Color AccentBlue = Color.FromArgb(90, 168, 232);  // azul brillante
        public static Color DividerLine = Color.FromArgb(26, 48, 80);  // línea separadora

        // --- Fuente del menú ---
        public static Font MenuFont = new Font("Segoe UI", 10.5f, FontStyle.Regular);
        public static Font MenuFontActive = new Font("Segoe UI", 10.5f, FontStyle.Bold);
        public static Font TitleFont = new Font("Segoe UI", 13f, FontStyle.Bold);
        public static Font SubtitleFont = new Font("Segoe UI", 9f, FontStyle.Regular);
    }
}
