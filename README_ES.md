# Electronic Observer (七四式電子観測儀)

<p align="center"><a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README.md">English</a> | <a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README_CN.md">简体中文</a> | <b>Español</b> | <a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README_KR.md">한국어</a></p>

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ElectronicObserverEN_ElectronicObserver\&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=ElectronicObserverEN_ElectronicObserver)

![captura de pantalla](https://cloud.githubusercontent.com/assets/6127734/21392624/18089a70-c7d4-11e6-8f85-55b877aef5b3.jpg)

Electronic Observer es un navegador diseñado para ayudar a los almirantes a gestionar sus flotas. Está lleno de funcionalidades para proporcionar información adicional y ayudarles a planificar y jugar KanColle de manera más eficiente.

## Características principales

Cada función se encuentra en ventanas separadas, que el usuario puede acoplar, mover y organizar libremente.

A continuación se muestra una breve explicación. **Para más detalles, por favor visita la [Wiki](https://github.com/ElectronicObserverEN/ElectronicObserver/wiki).**

* Navegador interno (incluye captura de pantalla, silenciar, y zoom)
* Vista de flota (estado \[en expedición, necesita suministros, etc.], poder aéreo, LoS)

  * Vista individual de los barcos (nivel, HP, condición, suministros, espacios de equipamiento)
  * Lista de flotas (ver el estado de todas las flotas en un vistazo)
  * Agrupación (agrupa tus barcos y hazles seguimiento de forma diferente)
* Dique seco (barcos reparando, tiempo restante)
* Arsenal (construcción, tiempo restante)
* Cuartel general (estado del almirante, visualización de recursos)
* Brújula (ruta próxima, vista de la flota enemiga, ganancias/pérdidas de recursos)
* Batalla (predicción de batalla y resultados)
* Información (CGs no vistos, resultados de construción, medidores de mapas)
* Misiones (con seguimiento completo)
* Enciclopedia (de barcos y equipamiento)
* Listado de equipamiento
* Notificaciones (expediciones, reparación, daño crítico, y más)
* Registros (mantén registros de drops, construcciones y desarrollos)
* Captura de ventanas (captura ventanas externas dentro del programa)

Ninguna de estas funciones interfiere con la operación normal y legal de KanColle.

## Descargas

La última versión de Electronic Observer está disponible en la página de [**Lanzamientos**](https://github.com/ElectronicObserverEN/ElectronicObserver/releases/latest).

Nota: necesitas tener instalado [Visual C++ 2019](https://support.microsoft.com/en-us/topic/the-latest-supported-visual-c-downloads-2647da03-1eea-4433-9aff-95f26a218cc0) [(enlace directo)](https://aka.ms/vs/16/release/vc_redist.x64.exe). Versiones anteriores de Windows (7, 8) podrían necesitar también [KB2533623](https://support.microsoft.com/help/2533623/microsoft-security-advisory-insecure-library-loading-could-allow-remot).

## Documentación

La API de Kancolle está documentada (solo en japonés) en el [Other/Information/](https://github.com/andanteyk/ElectronicObserver/tree/develop/ElectronicObserver/Other/Information) del proyecto original.

Úsala como desees, pero no se garantiza la precisión.

## Compilación

1. Descarga la última versión de [Visual Studio Preview](https://visualstudio.microsoft.com/vs/preview/#download-preview)
2. En el instalador, asegúrate de habilitar el desarrollo de escritorio con ".NET" ![image](https://github.com/ElectronicObserverEN/ElectronicObserver/assets/40002167/748d862c-4c61-4ef6-b147-961b532852c9)
3. [Clona este repositorio](https://learn.microsoft.com/en-us/visualstudio/version-control/git-clone-repository)
4. Haz clic en este botón o presiona F5 ![image](https://github.com/ElectronicObserverEN/ElectronicObserver/assets/40002167/dbee165d-8ea9-4f27-9c28-d406e2a9978a)

## Bibliotecas

* [DynaJson](https://github.com/fujieda/DynaJson) (lectura/escritura JSON) - [Licencia MIT](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/DynaJson.txt)
* [DockPanel Suite](http://dockpanelsuite.com/) (gestión de ventanas) - [Licencia MIT](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/DockPanelSuite.txt)
* [Nekoxy](https://github.com/veigr/Nekoxy) (captura de red) - [Licencia MIT](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/Nekoxy.txt)

  * [TrotiNet](http://trotinet.sourceforge.net/) - [Licencia GNU Lesser General Public License v3.0](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/LGPL.txt)

    * [log4net](https://logging.apache.org/log4net/) - [Licencia Apache versión 2.0](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/Apache.txt)

## Licencia

Este proyecto está licenciado bajo la Licencia MIT. Para más información, consulta el archivo [LICENSE](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/LICENSE).

## Contacto

Por favor, abre un [**issue**](https://github.com/ElectronicObserverEN/ElectronicObserver/issues) en Github si tienes algún problema, pregunta o sugerencia.

Gracias por usar Electronic Observer.

* Traductor: [silfumus](https://github.com/silfumus)
* Página de distribución japonesa: [ブルネイ工廠電気実験部](http://electronicobserver.blog.fc2.com/)
* Desarrollador: [Andante](https://twitter.com/andanteyk)
* <a rel="me" href="https://fosstodon.org/@ElectronicObserver">Mastodon</a>
