# 🎬 MediaCenter
### Gestor Multimedia Personal — Proyecto Final de Ingeniería en Informática

![Version](https://img.shields.io/badge/versión-1.0.0-blue)
![Platform](https://img.shields.io/badge/plataforma-Windows-blue)
![Framework](https://img.shields.io/badge/.NET-Windows%20Forms-purple)
![Database](https://img.shields.io/badge/base%20de%20datos-SQL%20Server-red)
![Language](https://img.shields.io/badge/lenguaje-C%23-green)

---

## 📋 Descripción

**MediaCenter** es una aplicación de escritorio desarrollada en **C# con Windows Forms** que funciona como un gestor multimedia personal. Permite organizar, visualizar y reproducir archivos de fotos, música y videos, almacenando toda la información en una base de datos **SQL Server**.

La aplicación cuenta con un diseño moderno **Dark Blue Premium**, reloj en tiempo real, estadísticas de biblioteca y carga automática de archivos desde la base de datos.

---

## ✨ Características principales

- 📷 **Gestión de Fotos** — Visor de imágenes con metadatos GPS y mapa interactivo
- 🎵 **Gestión de Música** — Reproductor con metadatos TagLib, listas de reproducción
- 🎬 **Gestión de Videos** — Reproductor integrado con Windows Media Player
- 🗄️ **Base de Datos** — CRUD completo con importación/exportación CSV
- 🏠 **Pantalla de Inicio** — Estadísticas en tiempo real, actividad reciente y distribución
- 📂 **Importación por carpeta** — Carga masiva de archivos con detección de duplicados
- 🕐 **Reloj en vivo** — Hora y fecha en tiempo real en la barra superior
- 🏷️ **Badges** — Conteo de archivos por categoría en el menú lateral
- 🟢 **Barra de estado** — Estado de conexión SQL y total de archivos

---

## 🛠️ Tecnologías utilizadas

| Tecnología | Versión | Uso |
|---|---|---|
| C# | .NET 8 | Lenguaje principal |
| Windows Forms | .NET 8 | Interfaz gráfica |
| SQL Server Express | 2019+ | Base de datos |
| Microsoft.Data.SqlClient | NuGet | Conexión a BD |
| TagLib# | NuGet | Metadatos de audio |
| WebView2 | NuGet | Mapa GPS interactivo |
| AxWindowsMediaPlayer | COM | Reproducción multimedia |

---

## 📁 Estructura del proyecto

```
MediaCenter/
├── 📂 Datos/
│   └── ConexionSQL.cs          # Cadena de conexión y método ObtenerConexion()
├── 📂 Modelos/
│   └── ItemCancion.cs          # Modelo para canciones y videos en ListBox
├── 📂 Recursos/
│   └── (imágenes y recursos)
├── 📂 Servicios/
│   ├── EstadisticasServicio.cs  # Consultas SQL: conteos, recientes, por tipo
│   └── ValidarArchivos.cs       # Validación de imágenes, audio y video
├── 📂 Vistas/
│   ├── VistaFotos.cs            # Vista de fotos con GPS
│   ├── VistaMusica.cs           # Vista de música con reproductor
│   ├── VistaVideos.cs           # Vista de videos con reproductor
│   ├── VistaBaseDatos.cs        # Vista de base de datos con DataGridView
│   ├── VistaConfiguracion.cs    # Vista de configuración y estadísticas
│   └── VistaInicio.cs           # Pantalla de inicio con tarjetas y paneles
├── FormPrincipal.cs             # Formulario principal con menú lateral
├── UITheme.cs                   # Clase estática con paleta de colores
└── Program.cs                   # Punto de entrada de la aplicación
```

---

## 🗃️ Estructura de la base de datos

### Tabla principal: `dbo.Archivos`

| Columna | Tipo | Descripción |
|---|---|---|
| `IdArchivo` | int (PK) | Identificador único |
| `Nombre` | nvarchar(255) | Nombre del archivo |
| `RutaCompleta` | nvarchar(500) | Ruta absoluta en disco |
| `Tipo` | nvarchar(20) | `Foto`, `Musica` o `Video` |
| `Extension` | nvarchar(10) | Extensión del archivo |
| `TamanoKB` | decimal(12,2) | Tamaño en kilobytes |
| `FechaAgregado` | datetime | Fecha de registro |
| `EstaCorrupto` | bit | Indica si el archivo está corrupto |

### Tablas relacionadas

| Tabla | Descripción |
|---|---|
| `dbo.MetadatosFoto` | Coordenadas GPS y datos EXIF |
| `dbo.MetadatosMusica` | Título, artista, álbum, duración |
| `dbo.ListasReproduccion` | Listas de reproducción creadas |
| `dbo.CancionesEnLista` | Relación canciones-listas |

---

## 🎨 Sistema de diseño — UITheme.cs

Todos los colores están centralizados en la clase estática `UITheme`:

```csharp
// Fondos
SidebarBg       = Color.FromArgb(10,  22,  40)   // Menú lateral
SidebarActive   = Color.FromArgb(21, 101, 192)   // Botón activo
ContentBg       = Color.FromArgb(13,  31,  60)   // Área de contenido

// Texto
TextPrimary     = Color.FromArgb(255, 255, 255)  // Blanco
TextSecondary   = Color.FromArgb(142, 180, 212)  // Azul claro
TextMuted       = Color.FromArgb( 61, 106, 154)  // Azul grisáceo

// Acento
AccentBlue      = Color.FromArgb( 90, 168, 232)  // Azul brillante
DividerLine     = Color.FromArgb( 26,  48,  80)  // Separadores
```

---

## 🏗️ Arquitectura del proyecto

```
FormPrincipal
    ├── UITheme.cs          ← paleta de colores centralizada
    ├── panelMenu           ← menú lateral con botones y perfil
    ├── panelContenido      ← área dinámica donde se cargan las vistas
    │   ├── _topBar         ← barra superior con ícono, título y reloj
    │   └── UserControl     ← vista activa (Fotos/Música/Videos/etc.)
    └── _statusBar          ← barra de estado inferior

Cada Vista (UserControl)
    ├── AplicarTema()           ← aplica colores Dark Blue
    ├── CargarDesdeDB()         ← carga archivos de la BD al abrir
    ├── GuardarArchivoEnBD()    ← inserta nuevo archivo en dbo.Archivos
    ├── ArchivoYaExisteEnBD()   ← verifica duplicados antes de insertar
    └── ArchivoAgregado (event) ← notifica al FormPrincipal para actualizar badges

EstadisticasServicio
    ├── ContarPorTipo()         ← SELECT COUNT para tarjetas del inicio
    ├── ObtenerRecientes()      ← TOP 3 archivos más recientes
    └── ObtenerArchivosPorTipo()← todos los archivos de un tipo
```

---

## ⚙️ Requisitos previos

- Windows 10 o superior
- Visual Studio 2022 o superior
- SQL Server Express 2019+
- .NET 8 SDK
- WebView2 Runtime instalado

---

## 🚀 Instalación y configuración

### 1. Clonar el repositorio
```bash
git clone https://github.com/TuUsuario/MediaCenter.git
cd MediaCenter
```

### 2. Configurar la base de datos
Abre **SQL Server Management Studio** y ejecuta el script de creación:
```sql
CREATE DATABASE MediaCenterDB;
USE MediaCenterDB;

CREATE TABLE dbo.Archivos (
    IdArchivo     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        NVARCHAR(255)    NOT NULL,
    RutaCompleta  NVARCHAR(500)    NOT NULL,
    Tipo          NVARCHAR(20)     NOT NULL,
    Extension     NVARCHAR(10)     NOT NULL,
    TamanoKB      DECIMAL(12,2),
    FechaAgregado DATETIME         NOT NULL,
    EstaCorrupto  BIT              NOT NULL DEFAULT 0
);
```

### 3. Configurar la cadena de conexión
Abre `Datos/ConexionSQL.cs` y actualiza el servidor:
```csharp
private static string cadenaConexion =
    @"Server=TU_SERVIDOR\SQLEXPRESS;Database=MediaCenterDB;
      Integrated Security=True;TrustServerCertificate=True;";
```

### 4. Instalar paquetes NuGet
En Visual Studio: **Tools → NuGet Package Manager → Package Manager Console**
```
Install-Package Microsoft.Data.SqlClient
Install-Package TagLibSharp
Install-Package Microsoft.Web.WebView2
```

### 5. Compilar y ejecutar
```
Ctrl + Shift + B  →  compilar
F5               →  ejecutar
```

---

## 📖 Guía de uso

### Agregar archivos
1. Selecciona una sección del menú (Fotos, Música o Videos)
2. Clic en **Agregar** para seleccionar archivos individuales
3. O clic en **Importar Carpeta** para cargar todos los archivos de una carpeta

### Ver estadísticas
- La **pantalla de Inicio** muestra el total de archivos por categoría
- El panel de **Actividad reciente** muestra los últimos 3 archivos agregados
- El panel de **Distribución** muestra barras de porcentaje por tipo

### Base de Datos
- **Importar CSV** — carga registros desde un archivo CSV
- **Exportar CSV** — guarda todos los registros en CSV
- **Nuevo / Modificar / Eliminar** — CRUD manual de registros

---

## 👨‍💻 Autor

**Josue J.**
Estudiante de Ingeniería en Informática
Proyecto Final — 2026

---

## 📄 Licencia

Este proyecto fue desarrollado con fines educativos como proyecto final de la carrera de Ingeniería en Informática.
