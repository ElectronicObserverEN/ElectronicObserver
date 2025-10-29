# Electronic Observer (七四式電子観測儀)
<p align="center"><b>English</b> | <a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README_CN.md">简体中文</a> | <a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README_ES.md">Español</a> | <a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README_KR.md">한국어</a></p>

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ElectronicObserverEN_ElectronicObserver&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=ElectronicObserverEN_ElectronicObserver)

![screenshot](https://cloud.githubusercontent.com/assets/6127734/21392624/18089a70-c7d4-11e6-8f85-55b877aef5b3.jpg)

Electronic Observer is a browser to help admirals manage their fleets. It is packed with features to provide additional informations for admirals to plan and play KanColle better.

## Main Features
Each function is located in separate windows, and can be freely docked, moved, and arranged by the user.

Below is a brief explanation. **For more details, please see the [Wiki](https://github.com/ElectronicObserverEN/ElectronicObserver/wiki).**

* Internal browser (including screenshot, mute, and zoom)
* Fleet display (status [on expedition, needs supplies, etc.], air power, LoS)
    * Individual ship display (Level, HP, condition, supplies, equipment slots)
    * Fleet list (see all fleet statuses at a glance)
    * Grouping (group your ships and track them differently)
* Dock (current ships, remaining time)
* Arsenal (current construction, remaining time)
* Headquarters (Admiral status, resource display)
* Compass (upcoming route, enemy fleet display, resource gain/losses)
* Battle (battle prediction and results)
* Information (unseen CGs, crafting results, map gauges)
* Quests (with complete tracking)
* Encyclopedia (of ships and equipment)
* Equipment List
* Notifications (expeditions, docing, critical damage, and more)
* Records (keep records of drops, constructions, and developments)
* Window Capture (capture external windows into the program)

None of these functions interfere with KanColle normal, legal operation.

## Downloads

The latest version of Electronic Observer is available at the [**Releases**](https://github.com/ElectronicObserverEN/ElectronicObserver/releases/latest) page.

note: you need to have [Visual C++ 2019](https://support.microsoft.com/en-us/topic/the-latest-supported-visual-c-downloads-2647da03-1eea-4433-9aff-95f26a218cc0) [(direct link)](https://aka.ms/vs/16/release/vc_redist.x64.exe) installed, earlier versions of windows (7, 8) might also need [KB2533623](https://support.microsoft.com/help/2533623/microsoft-security-advisory-insecure-library-loading-could-allow-remot)

## Documentations

The Kancolle API is documented (in Japanese only) at the original project's [Other/Information/](https://github.com/andanteyk/ElectronicObserver/tree/develop/ElectronicObserver/Other/Information).

Use as you wish, but no guarantee of accuracy is made.

## Build

1. Download the latest [Visual Studio Preview](https://visualstudio.microsoft.com/vs/preview/#download-preview)
2. In the installer, make sure to enable ".NET desktop development" ![image](https://github.com/ElectronicObserverEN/ElectronicObserver/assets/40002167/748d862c-4c61-4ef6-b147-961b532852c9)
3. [Clone this repository](https://learn.microsoft.com/en-us/visualstudio/version-control/git-clone-repository)
4. Click this button or press F5 ![image](https://github.com/ElectronicObserverEN/ElectronicObserver/assets/40002167/dbee165d-8ea9-4f27-9c28-d406e2a9978a)

## Libraries

* [DynaJson](https://github.com/fujieda/DynaJson) (JSON read/write) - [MIT License](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/DynaJson.txt)
* [DockPanel Suite](http://dockpanelsuite.com/) (Window layout) - [MIT License](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/DockPanelSuite.txt)
* [Nekoxy](https://github.com/veigr/Nekoxy) (Network capture) - [MIT License](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/Nekoxy.txt)
    * [TrotiNet](http://trotinet.sourceforge.net/) - [GNU Lesser General Public License v3.0](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/LGPL.txt)
        * [log4net](https://logging.apache.org/log4net/) - [Apache License version 2.0](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/Apache.txt)

## License
This project is licensed under the MIT License, for more information, see [LICENSE](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/LICENSE).

## Contacts

Please open a new [**issue**](https://github.com/ElectronicObserverEN/ElectronicObserver/issues) at Github if you have any problem, question, or suggestion.

Thank you for using Electronic Observer.
* Translator: [silfumus](https://github.com/silfumus)
* Japanese Distribution Website: [ブルネイ工廠電気実験部](http://electronicobserver.blog.fc2.com/)
* Developer: [Andante](https://twitter.com/andanteyk)
* <a rel="me" href="https://fosstodon.org/@ElectronicObserver">Mastodon</a>
