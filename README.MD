# WinAppCaptureS2

Está aplicación permite la captura de forma automática e instantánea cada tiempo asignado en código en el caso por defecto es 2 segundos.

Este programa funciona con .NET 8 para compilarlo con mayor facilidad primero debes descargar su sdk y después recomiendo abrirlo en Visual Studio Code.

## Consideraciones

* El archivo RAR contiene la última versión compilada para sistema x64 de Windows y es portable

### Compilación para x64

correr en consola:

dotnet publish -c Release -r win-x64 --self-contained true

### Compilación para x32
Correr en consola:

dotnet publish -c Release -r win-x86 --self-contained true

La publicaciones estarán en la carpeta:

bin\Release\net8.0\win-x86\publish\

toda publicación ya contiene lo necesario para ejecutarse sin instalar nada.


## Cambiar intervalo de capturas:

Buscar la línea en el archivo Form1.cs:

captureTimer.Interval = 2000; // Captura cada 2 segundos

##Cómo se usa:

Ejecuta el archivo: CapturaPantallaApp.exe

![cargar App](/img/1.png)

Al ejecutar el programa automáticamente creará una carpeta en la raíz del programa con la fecha-hora-minutos-segundos en los que se inicia el programa y ahí se guardarán las capturas de esa sesión.

![cargar App](/img/3.png)

Para iniciar a capturar debes presionar el botón iniciar captura e iniciara la toma cada 2 segundos (por defecto) luego para detener las capturas haz clic en el mismo botoón que se llamará Detener y luego clic en el botón rojo para cerrar el programa

![cargar App](/img/2.png)

# Descargar el programa para x64

Haz clic en el link de abajo o copialo y pegalo en tu navegador:

https://github.com/edwinsacrecinos/WinAppCaptureS2/raw/refs/heads/main/CapturaAPP25.rar 