# Electronic Observer (七四式電子観測儀)

<p align="center"><a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README.md">English</a> | <a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README_CN.md">简体中文</a> | <a href="https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/README_ES.md">Español</a> | <b>한국어</b></p>


[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ElectronicObserverEN_ElectronicObserver&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=ElectronicObserverEN_ElectronicObserver)

![screenshot](https://cloud.githubusercontent.com/assets/6127734/21392624/18089a70-c7d4-11e6-8f85-55b877aef5b3.jpg)

Electronic Observer는 제독들이 자신의 함대를 효율적으로 관리하도록 돕는 브라우저입니다.  
이 프로그램은 함대 계획 및 함대 컬렉션(칸코레) 플레이를 보다 쉽게 해주는 다양한 기능을 제공합니다.

## 주요 기능
각 기능은 독립된 창으로 구성되어 있으며, 사용자가 자유롭게 도킹, 이동, 배치할 수 있습니다.

아래는 간단한 설명이며, **자세한 내용은 [Wiki](https://github.com/ElectronicObserverEN/ElectronicObserver/wiki)를 참고하세요.**

* 내장 브라우저 (스크린샷, 음소거, 확대/축소 기능 포함)
* 함대 표시 (원정 중, 보급 필요 등 상태 표시 / 제해권, 탐지력 표시)
    * 개별 함선 표시 (레벨, HP, 컨디션, 보급 상태, 장비 슬롯 등)
    * 함대 목록 (모든 함대 상태를 한눈에 확인)
    * 그룹 관리 (함선을 그룹으로 나누어 별도로 추적 가능)
* 도크 (수리 중인 함선과 남은 시간 표시)
* 조선소 (건조 중인 함선과 남은 시간 표시)
* 사령부 (제독 정보 및 자원 현황)
* 나침반 (다음 항로, 적 함대, 자원 획득/손실 표시)
* 전투 (전투 예측 및 결과 표시)
* 정보 탭 (미확인 CG, 제작 결과, 맵 게이지 등)
* 임무 (완전한 임무 추적 기능)
* 도감 (함선 및 장비 도감)
* 장비 목록
* 알림 기능 (원정 완료, 수리 완료, 대파 경고 등)
* 기록 기능 (드랍, 건조, 개발 결과 저장)
* 창 캡처 (외부 창을 프로그램 내로 캡처)

이 모든 기능은 칸코레의 정상적이고 합법적인 실행에 간섭하지 않습니다.

## 다운로드
최신 버전은 [**Releases**](https://github.com/ElectronicObserverEN/ElectronicObserver/releases/latest)에서 받을 수 있습니다.

참고: 프로그램 실행을 위해 [Visual C++ 2019](https://support.microsoft.com/en-us/topic/the-latest-supported-visual-c-downloads-2647da03-1eea-4433-9aff-95f26a218cc0) [(다이렉트 링크)](https://aka.ms/vs/16/release/vc_redist.x64.exe)가 필요합니다.  
Windows 7/8 사용자는 추가로 [KB2533623](https://support.microsoft.com/help/2533623/microsoft-security-advisory-insecure-library-loading-could-allow-remot)를 설치해야 할 수도 있습니다.

## 문서
칸코레 API 관련 문서는 원본 프로젝트의 [Other/Information/](https://github.com/andanteyk/ElectronicObserver/tree/develop/ElectronicObserver/Other/Information) 경로에 있으며, 일본어로만 제공됩니다.  
정확성은 보장되지 않으므로 참고용으로만 사용하세요.

## 빌드
1. [Visual Studio Preview](https://visualstudio.microsoft.com/vs/preview/#download-preview)를 다운로드합니다.  
2. 설치 시 “.NET desktop development” 항목을 반드시 선택합니다.  
   ![image](https://github.com/ElectronicObserverEN/ElectronicObserver/assets/40002167/748d862c-4c61-4ef6-b147-961b532852c9)  
3. [리포지토리를 복제](https://learn.microsoft.com/en-us/visualstudio/version-control/git-clone-repository)합니다.  
4. 아래 버튼을 클릭하거나 F5 키를 눌러 실행합니다.  
   ![image](https://github.com/ElectronicObserverEN/ElectronicObserver/assets/40002167/dbee165d-8ea9-4f27-9c28-d406e2a9978a)

## 라이브러리
* [DynaJson](https://github.com/fujieda/DynaJson) (JSON 읽기/쓰기) - [MIT License](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/DynaJson.txt)  
* [DockPanel Suite](http://dockpanelsuite.com/) (윈도우 레이아웃 관리) - [MIT License](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/DockPanelSuite.txt)  
* [Nekoxy](https://github.com/veigr/Nekoxy) (네트워크 캡처) - [MIT License](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/Nekoxy.txt)  
    * [TrotiNet](http://trotinet.sourceforge.net/) - [GNU LGPL v3.0](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/LGPL.txt)  
        * [log4net](https://logging.apache.org/log4net/) - [Apache License 2.0](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/Licenses/Apache.txt)

## 라이선스
이 프로젝트는 MIT License 하에 배포됩니다. 자세한 내용은 [LICENSE](https://github.com/ElectronicObserverEN/ElectronicObserver/blob/main/LICENSE)를 참고하세요.

## 연락처
문제, 질문, 제안이 있을 경우 GitHub의 [**Issues**](https://github.com/ElectronicObserverEN/ElectronicObserver/issues)에 새 이슈를 열어주세요.

Electronic Observer를 사용해주셔서 감사합니다.  
* 번역자: [silfumus](https://github.com/silfumus)  
* 일본어 배포 웹사이트: [ブルネイ工廠電気実験部](http://electronicobserver.blog.fc2.com/)  
* 개발자: [Andante](https://twitter.com/andanteyk)  
* <a rel="me" href="https://fosstodon.org/@ElectronicObserver">Mastodon</a>
