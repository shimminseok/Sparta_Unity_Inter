# 🎮 Unity 3D RPG Character Controller System
## 개요
이 프로젝트는 유니티 기반의 싱글 플레이어 RPG 시스템을 구현한 예제입니다. 주요 구성 요소는 다음과 같습니다:

- 상태 기반 플레이어 FSM (Idle, Move, Jump)
- 스탯 및 버프/디버프 시스템
- 아이템 소비 및 장비 시스템
- 인터랙션 시스템
- 카메라 컨트롤러
- 인벤토리 UI
  
---

## 주요기능
### ✅ PlayerController 시스템
- StateMachine 기반 FSM 구조로 상태 분기 (Idle, Move, Jump)
- 입력 기반 이동 처리 및 벽 타기, 점프 기능 지원
- IObjectExecutable, IPlatform 기반 인터랙션 대응
- 벽 감지 및 플랫폼 동기화 처리

### ✅ 상태 시스템 (Stat, Buff, Debuff)
- StatManager를 통해 CalculatedStat, ResourceStat 타입의 스탯을 관리
- StatusEffectManager는 시간 기반 혹은 즉시 적용 버프/디버프를 처리
- BuffFactory를 통해 SO 기반 버프 데이터를 StatusEffect 인스턴스로 변환
  
### ✅ 인벤토리 시스템
- InventoryManager는 장비형/소비형 아이템의 추가, 사용, 장착 등을 처리
- UIInventory는 인벤토리 UI 갱신 및 선택 기능을 제공

### ✅ 인터랙션 시스템
- IInteractable 인터페이스로 상호작용 가능한 오브젝트 정의
- InteractionManager는 카메라 기반 Ray를 사용해 HUD를 표시하고 인터랙션 가능 여부 판단

### ✅ 카메라 컨트롤
- CameraController는 마우스 입력에 따라 회전 및 줌을 처리
- CinemachineVirtualCamera를 통해 부드러운 시점 전환 구현
  
---
## 사용된 디자인 패턴
- State Pattern: 플레이어의 행동 상태 전환 (Idle, Move, Jump)
- Factory Pattern: BuffFactory를 이용해 SO 기반의 버프 생성
- Strategy Pattern (인터페이스 기반): IInteractable, IObjectExecutable, IPlatform, IKnockbackable 등 행동 전략 정의
---
## 인터페이스 정의 요약
|인터페이스|설|
|------|---|
|IInteractable|플레이어와 상호작용 가능한 오브젝트 정의|
|IActivatable|외부에 의해 활성화 가능한 오브젝트|
|IPlatform|지속적으로 플레이어와 상호작용하는 플랫폼|
|IObjectExecutable|일회성 상호작용 오브젝트 (예: 점프 발판)|
|IKnockbackable|넉백 처리 대상 객체|

---
## 확장성
- 새로운 버프 타입을 추가하려면 StatusEffect를 상속한 클래스만 추가하고, BuffFactory에 등록하면 됩니다.
- 새로운 상태는 PlayerState enum과 함께 IState<PlayerController>를 구현하면 손쉽게 FSM에 연결할 수 있습니다.

## 사용법
- WASD를 이용하여 이동하고 마우스 오른쪽을 누른채 마우스를 움직이면 시점이 변경됩니다.
- 상호작용 가능한 Object 앞에 위치시 상호작용 가능 안내 텍스트가 활성화 되며 상호작용키 입력시 상호작용이 가능합니다.
- 오브젝트에 마우스를 올려두면 해당 오브젝트의 정보가 나옵니다.
- Tab키를 눌러 인벤토리를 열고 닫을 수 있으며 F1 입력시 존재하는 모든 아이템을 획득할 수 있습니다.
- 인벤토리 내에서 마우스 오른쪽 클릭시 아이템이 사용 & 장착되며 왼쪽 드래그로 아이템의 위치를 옮길 수 있습니다.
