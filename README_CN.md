# Electronic Observer (七四式電子観測儀)
<p align="center"><a href="https://github.com/andanteyk/ElectronicObserver/blob/master/README.md">English</a> | <b>简体中文</b></p>

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ElectronicObserverEN_ElectronicObserver&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=ElectronicObserverEN_ElectronicObserver)

![screenshot](https://cloud.githubusercontent.com/assets/6127734/21392624/18089a70-c7d4-11e6-8f85-55b877aef5b3.jpg)

Electronic Observer 是一个帮助提督管理自己舰队的浏览器类工具。本工具功能丰富，旨在为《舰队Collection》的提督们提供详尽舰队信息，助力运筹帷幄。

## 主要功能
各功能模块均以独立窗口呈现，支持自由停靠、移动与个性化排布。

简介如下，**请查看 [Wiki](https://github.com/RyuuKitsune/ElectronicObserver/wiki) 寻找更多信息**。

* 内置浏览器 (包括截图、禁音和缩放功能)
* 舰队显示 (状态 [如远征中、需要补给等], 制空, LoS)
    * 单个船的状态显示 (等级, 血量, 状况, 补给, 装备格)
    * 舰队总览 (直接显示整个舰队所有船的状况信息)
    * 编组（将舰船分组并独立追踪各编队）
* 修理 (正在修理的船、剩余时间)
* 工厂 (当前建造、剩余时间)
* 镇守府 (提督状态、资源显示)
* 指南针 (路线预测、敌舰阵容、资源获取/损失)
* 战况 (战况预测和战斗结果)
* 信息 (未观看的CG、开发结果、海图血条)
* 任务 (完成情况追踪)
* 图鉴 (舰船图鉴、装备图鉴)
* 装备清单
* 提醒 (远征、修理、大破等)
* 记录 (掉落记录、建造记录、开发记录)
* 窗口捕获 (将外部窗口捕获至程序内)

所有功能均不会影响《舰队Collection》的正常合法操作和运行。

## 下载

最新版 Electronic Observer 可以在 [**Releases**](https://github.com/gre4bee/ElectronicObserver/releases/latest) 页下载。

注意: 你需要安装 [Visual C++ 2019](https://support.microsoft.com/en-us/topic/the-latest-supported-visual-c-downloads-2647da03-1eea-4433-9aff-95f26a218cc0) [(direct link)](https://aka.ms/vs/16/release/vc_redist.x64.exe)，更早的 Windows 版本 (7, 8) 可能还需要安装 [KB2533623](https://support.microsoft.com/help/2533623/microsoft-security-advisory-insecure-library-loading-could-allow-remot)

## 文档

舰队Collection API 文档 (仅有日语) 在原项目的 [Other/Information/](https://github.com/andanteyk/ElectronicObserver/tree/develop/ElectronicObserver/Other/Information).

可随意使用，但无法保证准确性。

## 编译

1. 下载最新版 [Visual Studio Preview](https://visualstudio.microsoft.com/vs/preview/#download-preview)
2. 在安装器中，确保你勾选了 ".NET 桌面开发" ![image](https://github.com/ElectronicObserverEN/ElectronicObserver/assets/40002167/748d862c-4c61-4ef6-b147-961b532852c9)
3. [克隆本仓库](https://learn.microsoft.com/en-us/visualstudio/version-control/git-clone-repository)
4. 点击这个按钮或者按F5 ![image](https://github.com/ElectronicObserverEN/ElectronicObserver/assets/40002167/dbee165d-8ea9-4f27-9c28-d406e2a9978a)

## 相关库

* [DynaJson](https://github.com/fujieda/DynaJson) (JSON 读写) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/DynaJson.txt)
* [DockPanel Suite](http://dockpanelsuite.com/) (窗口布局) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/DockPanelSuite.txt)
* [Nekoxy](https://github.com/veigr/Nekoxy) (网络抓包) - [MIT License](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Nekoxy.txt)
    * [TrotiNet](http://trotinet.sourceforge.net/) - [GNU Lesser General Public License v3.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/LGPL.txt)
        * [log4net](https://logging.apache.org/log4net/) - [Apache License version 2.0](https://github.com/andanteyk/ElectronicObserver/blob/master/Licenses/Apache.txt)

## 许可证

本仓库使用 MIT License，详情查看 [LICENSE](https://github.com/andanteyk/ElectronicObserver/blob/master/LICENSE)

## 联系

如遇问题、疑问或有建议，请在 GitHub 提交 [**issue**](https://github.com/silfumus/ElectronicObserver/issues)。

感谢使用 Electronic Observer。
* 翻译: [silfumus](https://github.com/silfumus)
* 日版官方发行网站: [ブルネイ工廠電気実験部](http://electronicobserver.blog.fc2.com/)
* 开发者: [Andante](https://twitter.com/andanteyk)
* <a rel="me" href="https://fosstodon.org/@ElectronicObserver">Mastodon</a>

## 感谢

<img src="https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.png" alt="JetBrains Logo (Main) logo" width="200" />
