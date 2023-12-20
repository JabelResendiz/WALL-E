
# Geo_WallE 🤖

Geo_WallE es un app de Blazor Server, un framework de desarrollo web de código abierto desarrollado por Microsoft para el desarrollo de aplicaciones web modernas con C# y ASP.NET Core. 

![Logo](/BlazorServerApp/BlazorServer/wwwroot/logo.png)


## Run Locally 🦾

Clone the project in your PC

```bash
  git clone https://github.com/JabelResendiz/WALL-E
```

Go to the project directory

```bash
  cd BlazorServerApp/BlazorServer/
```

Start the server in Linux

```bash
  make dev
```

Start the server in Windows

```bash
  dotnet watch
```

## G# Lenguage 👨‍💻

G# es un lenguaje de programación sobre el que se fundamenta gran parte de la lógica del proyecto. Un programa en G# es un conjunto de instrucciones. Las instrucciones permiten recibir argumentos de
entrada, importar otros códigos, definir funciones o constantes, configurar características del visor y dibujar objetos
geométricos. En la carpeta Lenguage aparece implementada la lógica del lenguaje.

## Commands

- **[OK]** DRAW
- **[OK]** COLOR
- **[OK]** RESTORE
- **[OK]** IMPORT


## Running Tests

El proyecto es capaz de reconocer librerias importadas que el usuario deberá ubicar en la dirección :
```bash
   ~/BlazorServerApp/Tests
```
Ahi vera que hay cargada otros archivos que estaran a su disposición y los cuales sera libre de modificar. 

## Screenshots 📸

![App Screenshot](/HexagonWallE.png)


## Usage/Examples 🪂

```
mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m = measure (p1, p2);
        c1 = circle (p1, m);
        c2 = circle (p2, m);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2);
```

## Web Design 🚀

Usando la Tecnología de BlazorServer y aprovechando gran parte de sus implementaciones , el proyecto se visualiza en una página web. Blazor Server utiliza la arquitectura de servidor para renderizar las páginas web en el servidor y enviar actualizaciones al cliente a través de una conexión en tiempo real. Esto permite que las aplicaciones Blazor Server sean rápidas y responsivas, ya que solo se envían las actualizaciones necesarias al cliente en lugar de toda la página.

Ademas hacemos uso de las librerías P5 de JavaScript para la representacion del lienzo de las figuras y el dibujo de las mismas.

No pretendemos que el diseño super novedoso pero en realidad es un punto de inflexion en la vida del proyecto. El usuario (usted) se sentira mas comodo trabajando desde un sitio al cual puede acceder sin necesidad de un compilador específico, solo usando el navegador de su PC. 

Aunque las pruebas no estan hechas en todos los navegadores del mercado, al menos en esta primera versión esta probado en el Chrome ,que sí, no es el navegador por defecto de Windows, Mac o de su distribucion de Linux pero agradeceríamos que fuese GoogleChrome su opción. Sin embargo el software es libre, esta en su derecho de modificar el diseño. 🚀

## Documentation 🛰

El proyecto implementa muchas funcionabilidades de nuestro Segundo Proyecto HULK, una invitacion a que se entere de que iba y quede mas satisfecho con la explicacion de muchas implementaciones si no le quedan claro aqui. Aunque lo dudamos 👾

[HULK Documentation](https://github.com/JabelResendiz/HULK---Interpreter)
[WallE Informe](/GeoWall-E.pdf)

## Authors 🎖


- [@jabelresendiz](https://github.com/JabelResendiz)
- [@noelperez](https://github.com/noelpc03)

