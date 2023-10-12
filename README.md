# 김민상 - 내일배움캠프 게임개발 심화 개인과제

# [🎮다운로드](https://github.com/berylstar/WeAreGoingToMars/releases/tag/1.00)

### 목차

1. 게임 개요 및 개발 기간
2. 구현 기능
3. 기획 설계
4. 기능 명세서
5. 사용 에셋

---
<br>
<br>

# 1. 게임 개요 및 개발 기간

- **게임명** : `WeAreGoingToMars`
- **설명** : Photon 멀티 주식 게임
- **개요** : 주식으로 화성 가즈아 ! (※ <더 지니어스 : 블랙 가넷> 중 '폭풍의 증권시장'을 모티브로 제작했습니다.)
- **게임 방법**
    - 장이 열리는 09:00부터 시작해 장이 마감되는 15:30까지 다섯 개의 주식의 변동을 예측해 매수하고 매도해, 최고의 수익을 거두는 게임입니다.
    - ~~주식은 각각의 다양한 타입을 가지고 있으며 그 타입을 바탕으로 무작위로 주식이 변동합니다.~~ (추후 구현 예정)
    - **즉 모든 주식은 미리 변동사항이 정해져 있으며, 그 정보는 MARS MAGAZINE이 단독 입수 했다는 소식입니다 !**
    - MARS MAGAZINE은 변화하는 주식의 정보를 기사로 제공할 것 입니다. 이를 기반으로 다른 플레이어와 정보를 공유하고 주식 시장의 판도를 예측하세요.
- **플레이타임** : 7분 이내
- **개발 환경** : Unity 2022.3.2f1
- **타겟 플랫폼** : PC
- **개발 기간** `2023.10.06 ~ 2023.10.12`

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

## **GameScene**
- 방장 플레이어가 모든 플레이어들이 게임 씬으로 넘어오고 상태를 확인한 후, 게임을 시작합니다.
- 게임이 시작되면 플레이어들은 2000원의 자산을 받으며, 다섯 개의 주식을 자유롭게 매수하거나 매도할 수 있습니다.
- 상단 좌측의 버튼으로 스코어 보드를 열 수 있으며, 플레이어들의 자산, 주식 보유량을 확인 할 수 있습니다.
- 본인의 자산이 주식을 살 수 있는 정도라면 비용을 지불하고 해당 주식의 매수 버튼을 클릭해 매수할 수 있습니다.
- 소지한 주식을 매도하고 싶을 때는 해당 주식의 매도 버튼을 클릭해 매도할 수 있으며 매도한 주식의 가격만큼 자산을 추가합니다.
- 만약 소지한 주식이 0원이 된다면, 즉시 상장폐지되며 소지한 해당 주식은 모두 사라지고 상장폐지된 주식은 구매가 불가능합니다.
- 주식에 대한 정보는 우측의 뉴스로 확인할 수 있습니다. 특정주가 오르거나 내리거나에 대한 정보를 알 수 있습니다. 틀린 정보를 제공하지는 않지만, 어떤 주식일지에 대해 추측해야 합니다.
- 뉴스는 RPC로 동기화 하지 않고 플레이어마다 다른 뉴스를 볼 수 있도록 유도해, 플레이어 간의 정보 교환만 진실된다면 자산을 늘릴 수 있도록 게임을 구성했습니다.
- 장 마감 시간인 15:30이 되면 더 이상 시간이 흐르지 않고 주식을 매도할 수 없으며 게임이 종료됩니다.

## **추후 구현 예정**
- BGM과 효과음
- 각각의 주식 타입에 따른 변화 값 세팅
- 주식의 총 갯수 제한. (플레이어가 전략적으로 구매할 수 있도록)
- 플레이어 파산 안내

---
<br>
<br>

# 3. 기획 설계

- **~~주식 타입~~(추후 구현 예정)**
    | 타입 이름 | 기능 |
    | --- | --- |
    | RANDOM | 무작위로 변동되는 주식. 기본 베이스 |
    | WAVE | 더 큰 폭에서 무작위로 변동되는 주식 |
    | HEAVY | 더 적은 폭에서 무작위로 변동되는 주식 |
    | INCREASE | 결과적으로 처음보다 증가하는 주식 |
    | BIGUP | 특정 라운드에서 큰 폭으로 증가하는 주식 |
    | BIGDOWN | 특정 라운드에서 큰 폭으로 감소하는 주식 |

- **뉴스 타입**
    | 상황 | 뉴스 기사 |
    | -- | -- |
    

---
<br>
<br>

# 4. 기능 명세서

### - **[LobbyMainPanel](Assets/Photon/PhotonUnityNetworking/Demos/DemoAsteroids/Scripts/Lobby/LobbyMainPanel.cs)**

| 메소드 | 내용 |
| -- | -- |
| -- | 기본적으로 포톤 에셋에 포함된 LobbyScenePanel.cs에서 필요한 부분만 수정하여 사용 |


### - **[GameManager](Assets/Scripts/GameManager.cs)**

| 메소드 | 내용 |
| -- | -- |
| bool AllHasTag(string key) | 모든 플레이어의 커스텀 프로퍼티 값을 확인 |
| IEnumerator CoLoading() | AllHasTag 메소드로 플레이어들의 상태를 확인한 후, 시계, 주식을 세팅
| void RPCOnGame() | RPC 메소드로 로딩 판넬을 비활성화, FindMyBoard로 플레이어 할당
| void ToggleScoreBoard() | 플레이어 스코어 보드를 활성화/비활성화. ButtonScoreBoard에 할당
| PlayerBoard FindMyBoard() | 자신의 플레이어 보드를 찾아서 반환 |
| void ShowMyStatus() | 플레이어 정보(자본, 주식 보유량)를 표시 |
| void NextRound() | 모든 주식을 다음 라운드 값으로 변동 |
| void GameOver() | 게임 종료 메소드 |
| void OnButStockButton(int index) | 각각의 주식의 매수 버튼에 할당된 메소드. |
| void OnSellStockButton(int index) | 각각의 주식의 매도 버튼에 할당된 메소드. |
| void ApplyDelistedStock(Stock stock) | 상장폐지된 주식 적용 |
| void ShowNews() | 라운드 별 무작위 주식의 정보 또는 이번 라운드의 정보 공개 |

### - **[PlayerBoard](Assets/Scripts/PlayerBoard.cs)**

| 메소드 | 내용 |
| -- | -- |
| void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) | IPunObservable을 사용해 자산과 주식 보유량을 송신/수신 |
| void ShowPlayerStatus() | 스코어 보드에 자신의 정보를 표시 |
| void TryBuyStock(Stock stock) | 해당 주식을 구매할 수 있다면 매수 |
| void TrySellStock(Stock stock) | 해당 주식을 판매할 수 있다면 매도 |

### - **[Clock](Assets/Scripts/Clock.cs)**

| 메소드 | 내용 |
| -- | -- |
| IEnumerator CoStartTime() | 게임 오버 전까지 시간이 흐르도록 하는 코루틴. switch 문을 이용해 시간마다 벌어져야 하는 메소드 실행 |
| void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) | IPunObservable을 사용해 시간 데이터를 송신/수신 |
| void RPCNextRound() | 게임 매니저의 NextRound를 RPC로 전체 실행하도록 하는 메소드 |
| void RPCGameOver() | 게임 매니저의 GameOver를 RPC로 전체 실행하도록 하는 메소드 |
| void RPCTimeImminent() | CoClockBlink를 RPC로 전체 실행하도록 하는 메소드 |
| IEnumerator CoClockBlink() | 시간이 임박했을 때 시계가 깜빡이는 효과를 위한 메소드

### - **[Stock](Assets/Scripts/Stock.cs)**

| 메소드 | 내용 |
| -- | -- |
| void InitialStock() | 주식 초기 상태 설정 |
| void SetStockInAdvance() | 게임 매니저의 CoLoading을 통해 실행되는 메소드, 모든 주식의 변동 값을 주식 타입을 바탕으로 무작위 설정.
| void RPCSetStockGraph(int index, int value) | SetStockInAdvance 메소드에서 주식 값을 하나씩 설정할 때마다 모든 플레이어들에게 동일 한 값으로 설정하도록 하는 메소드 |
| void ShowStockStatus() | 주식 판에서 주식의 정보 표시 |
| string ShowCostChange() | 주식의 변동 정보 표시 |
| void ChangeStockCost() | 게임 매니저의 NextRound를 통해 실행되는 메소드, 다음 라운드의 주식 값으로 변동시키고 상장폐지를 체크.
| void Deslisting() | 주식의 상장폐지 적용 |
| string ReturnNews() | 해당 주식의 정보를 게임매니저로 리턴 |
| void MarketClosed() | 게임이 종료되면 매도 버튼을 비활성화 |
| int HowDoesStockChange() | 다음 라운드의 주식 변화량 반환 |

---
<br>
<br>

# 5. 사용 에셋
- 주식 이미지 : <더 지니어스 : 블랙 가넷> E06 장면 중 캡처
- 버튼 이미지 : 직접 제작
- 신문 이미지 : 직접 제작
- 시계 폰트 : LABDigital
- 그 외 폰트 : DungGeunMo
