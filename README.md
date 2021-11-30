
# Aplicación web desarrollada en ASP.NET MVC 5, Entity Framework, Razor y Oracle 11g Express Edition

Incluye un mantenedor de categorías y otro de productos. Ambos mantenedores poseen un mecanismo de paginación backend, pero solo el de productos 
utiliza procedimientos almacenados a la hora de insertar, actualizar y eliminar.

# Características

La solución posee un proyecto principal de tipo modelo vista controlador llamado WebApp. 
También posee Bibliotecas de clases desarrolladas en .NET Framework para separar la lógica
en capas. La capa DataAccess contiene las clases de dominio y la conexión a la base de datos. La capa Mappings posee los DTO y los mecanismos
para convertir una instancia del modelo de datos a DTO y viceversa. La capa Services posee los servicios que son utilizados mediante inyección
de dependencias por el proyecto WebApp. La capa Util posee herramientas para validaciones.

# Pasos para utilizar el proyecto

## 1- Crear el usuario

Utilizar el archivo crear_usuario_officemarket.sql que está en la carpeta Archivos SQL

## 1- Crear las tablas, secuencias, triggers, procedimientos almacenados e insertar algunos registros

Utilizar el archivo script_officemarket.sql que está en la carpeta Archivos SQL

## Reemplazar la cadena de conexion a oracle

La cadena de conexión debe cambiarse dentro de la solución en: <br /> 
DataAccess\App.Config <br />
WebApp\Web.config <br />