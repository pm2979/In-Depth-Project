# 심화 프로젝트 : 방치형 RPG

## 📑 목차
1. [개요](#개요)
2. [주요 기능](#주요-기능)
3. [주요 컴포넌트](#주요-컴포넌트)
4. [컨트롤](#컨트롤)
5. [프로젝트 구조](#프로젝트-구조)

---

## 개요
Unity 심화 프로젝트 : 방치형 RPG

## 주요 기능
- **멀티 씬 UI 관리**: Additive 모드로 UI 씬을 분리 로드하여 효율적인 씬 관리
- **상태 머신 아키텍처**: State 패턴 기반으로 플레이어/적 행동 로직 구현
- **인벤토리 시스템**: 소비형·장착형 아이템, 강화·구매 기능 완벽 지원
- **무기 시스템**: 데미지·넉백·강화 수치 적용, 충돌 핸들링
- **화폐 관리**: `PlayerPrefs` 기반 영구 저장, 이벤트 버스 갱신 알림
- **이벤트 버스**: `Publish`/`Subscribe` API로 느슨한 결합 통신
- **적 스폰 시스템**: 웨이브 단위 스폰 + 자동 웨이브 완료 체크
- **데이터 확장성**: 스크립터블 오브젝트 활용으로 손쉬운 데이터 관리

## 주요 컴포넌트

### GameManager
- 게임 전체 흐름 제어(시작·일시정지·종료)
- UI 씬 로드/언로드 관리

### 상태 머신
- **PlayerBaseState** / **EnemyBaseState**
  - 입력 처리, 이동, 애니메이션, 상태 전환
  - `IState` 인터페이스 확장 가능

### 인벤토리 시스템
- **InventoryUI**: 아이템 목록 조회, 선택, 사용, 구매, 강화
- 스크립터블 오브젝트로 데이터 관리

### 무기 시스템
- **Weapon**: 기본 공격력 + 강화 수치, 넉백 처리
- 트리거 충돌 시 효과 적용

### 화폐 관리
- **CurrencyManager**: 화폐 추가/소비, `PlayerPrefs` 영구 저장
- 이벤트 버스 알림으로 UI 갱신

### 이벤트 버스
- **EventBus**: `Publish`/`Subscribe` 메서드 지원
- `EventType` 열거형 키로 메시지 전달

### 적 스폰 시스템
- **EnemyManager**: 웨이브 단위로 적 스폰
- 사망 처리 및 웨이브 완료 이벤트 발송

---

## 컨트롤
| 키           | 기능               |
| ------------ | ------------------ |
| WASD/↑↓←→    | 이동               |
| 마우스 왼쪽   | 공격 (UI 제외)     |

---

## 프로젝트 구조
```text
Assets/
├── Scenes/            # 씬 파일
│   ├── MainScene.unity  # 게임 플레이 씬
│   └── UI.unity         # UI 전용 씬
├── Scripts/           # C# 스크립트
│   ├── Managers/       # GameManager, CurrencyManager, EnemyManager 등
│   ├── StateMachines/  # PlayerBaseState, EnemyBaseState 등
│   ├── UI/             # InventoryUI, UISlot 등
│   ├── Entities/       # Weapon, ItemData 등
│   └── Utils/          # EventBus, Singleton 등
└── Prefabs/           # 프리팹 모음



