# 🎮 Unity 3D RPG Character Controller System
##개요
이 프로젝트는 유니티 기반의 싱글 플레이어 RPG 시스템을 구현한 예제입니다. 주요 구성 요소는 다음과 같습니다:

- 상태 기반 플레이어 FSM (Idle, Move, Jump)
- 스탯 및 버프/디버프 시스템
- 아이템 소비 및 장비 시스템
- 인터랙션 시스템
- 카메라 컨트롤러
- 인벤토리 UI
  
---

##주요기능
###✅ PlayerController 시스템
- StateMachine 기반 FSM 구조로 상태 분기 (Idle, Move, Jump)
- 입력 기반 이동 처리 및 벽 타기, 점프 기능 지원
- IObjectExecutable, IPlatform 기반 인터랙션 대응
- 벽 감지 및 플랫폼 동기화 처리

###✅ 상태 시스템 (Stat, Buff, Debuff)
- StatManager를 통해 CalculatedStat, ResourceStat 타입의 스탯을 관리
- StatusEffectManager는 시간 기반 혹은 즉시 적용 버프/디버프를 처리
- BuffFactory를 통해 SO 기반 버프 데이터를 StatusEffect 인스턴스로 변환
  
###✅ 인벤토리 시스템
- InventoryManager는 장비형/소비형 아이템의 추가, 사용, 장착 등을 처리
- UIInventory는 인벤토리 UI 갱신 및 선택 기능을 제공

###✅ 인터랙션 시스템
- IInteractable 인터페이스로 상호작용 가능한 오브젝트 정의
- InteractionManager는 카메라 기반 Ray를 사용해 HUD를 표시하고 인터랙션 가능 여부 판단

###✅ 카메라 컨트롤
- CameraController는 마우스 입력에 따라 회전 및 줌을 처리
- CinemachineVirtualCamera를 통해 부드러운 시점 전환 구현

##사용된 디자인 패턴
- State Pattern: 플레이어의 행동 상태 전환 (Idle, Move, Jump)
- Factory Pattern: BuffFactory를 이용해 SO 기반의 버프 생성
- Strategy Pattern (인터페이스 기반): IInteractable, IObjectExecutable, IPlatform, IKnockbackable 등 행동 전략 정의
---

