# 김민상 - 내일배움캠프 게임개발 심화 개인과제

### 목차

1. 게임 개요 및 개발 기간
2. 구현 기능
3. 기능 명세서
4. 기획 설계
5. 클래스 설명
6. 사용 에셋

---
<br>
<br>

# 1. 게임 개요 및 개발 기간

- **게임명** : `WeAreGoingToMars`
- **설명** : Photon 멀티 주식 게임
- **개요** : 주식으로 화성 가즈아 ! (※ <더 지니어스 : 블랙 가넷> 중 '폭풍의 증권시장'을 모티브로 제작했습니다.)
- **조작법:**
    - 마우스 클릭
- **개발 환경** : Unity 2022.3.2f1
- **타겟 플랫폼** : PC
- **개발 기간** `2023.10.06 ~`

---
<br>
<br>

# 2. 구현 기능

## **LobbyScene**
- 게임 시작시 실행되는 씬
- 닉네임을 설정해 로비에 입장할 수 있으며, 닉네임을 입력하지 않으면 무작위의 닉네임이 설정된다.
- 로비에서는 직접 방을 만들거나, 무작위 방에 입장하거나, 방 리스트를 확인할 수 있다.
- 직접 방을 만들 때는 방의 이름과 플레이어 인원 수를 최대 5명까지 정할 수 있고, 방 이름은 정하지 않으면 '닉네임's Room'으로 설정된다.
- 무작위 방에 입장하면 현재 입장 가능한 방 중 무작위의 방에 들어갈 수 있고, 가능한 경우가 없다면 기본 설정의 방을 만들어 입장한다.
- 방 리스트를 확인할 때는 방의 이름, 인원 수를 체크하여 입장할 수 있다.
- 방에서는 레디 버튼을 통해 준비 상태로 진입하며, 플레이어가 혼자뿐이거나 모두 준비상태가 아니라면 게임 시작 버튼이 비활성화됩니다.
- 활성화된 게임 시작 버튼을 클릭하여 게임 씬으로 넘어갑니다.

---
<br>
<br>

# 3. 기능 명세서

진행도 | 기능명 | 기능 소개 | 
--|--|--|
★★★ | 플레이어 | 게임 내를 돌아다니며 자원을 채취하고 포탑을 건설하는 주체

---
<br>
<br>

# 4. 기획 설계

- **주식 타입**
    | 타입 이름 | 기능 |
    | --- | --- |
    | RANDOM | 무작위로 변동되는 주식. 기본 베이스 |
    | WAVE | 더 큰 폭에서 무작위로 변동되는 주식 |
    | HEAVY | 더 적은 폭에서 무작위로 변동되는 주식 |
    | INCREASE | 결과적으로 처음보다 증가하는 주식 |
    | BIGUP | 특정 라운드에서 큰 폭으로 증가하는 주식 |
    | BIGDOWN | 특정 라운드에서 큰 폭으로 감소하는 주식 |

---
<br>
<br>

# 5. 클래스 설명
    
| 클래스 | 기능 |
| -- | -- |
|[LobbyPanelMain](Assets\Photon\PhotonUnityNetworking\Demos\DemoAsteroids\Scripts\Lobby\LobbyMainPanel.cs)|Photon을 사용해 플레이어 접속과 방 생성에 대한 스크립트|
|[GameManager](Assets/Scripts/GameManager.cs)||
|[PlayerBoard](Assets/Scripts/PlayerBoard.cs)||
|[Clock](Assets/Scripts/Clock.cs)||
|[Stock](Assets/Scripts/Stock.cs)||

---
<br>
<br>

# 6. 사용 에셋
- 주식 이미지 : <더 지니어스 : 블랙 가넷> E06 장면 중 캡처
- 버튼 이미지 : 직접 제작