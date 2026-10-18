# Class Diagram — Hollow Chains Demo (v0.1, แผนก่อนเขียนโค้ด)

> เจ้าของ: ธีร์ · ตรวจ: กานต์, ณิชา · อ้างอิง: `TECH_SPEC.md` §4, skill `unity-csharp-architecture` (OOP/SOLID — D-020)
> สถานะ: **แผน + โค้ดบางส่วน** (หัวข้อที่มี ✅ = ตรงกับโค้ดจริง); ชื่อคลาสอาจปรับตอนลงมือ และต้องอัปเดตไฟล์นี้ใน commit เดียวกับโค้ดทุกครั้ง
> ดูภาพ: เปิดไฟล์นี้บน GitHub (branch `phase/P1-preproduction`) — GitHub วาด Mermaid ให้อัตโนมัติ

## วิธีอ่าน
| สัญลักษณ์ | ความหมาย | หลัก OOP |
|---|---|---|
| `<<interface>>` | สัญญา (สิ่งที่ทำได้) ไม่มีโค้ดจริง | Abstraction |
| `<<abstract>>` | คลาสแม่ที่มีโค้ดร่วม ใช้ตรง ๆ ไม่ได้ | Inheritance |
| `A <\|-- B` | B สืบทอดจาก A | Inheritance |
| `A <\|.. B` | B ทำตาม interface A | Polymorphism |
| `A *-- B` | A เป็นเจ้าของ B (B ตายตาม A) | Composition |
| `A o-- B` | A ใช้ B ที่มีอยู่แล้ว | Aggregation |
| `A ..> B` | A เรียกใช้ B | Dependency |
| `-` / `+` | private / public | Encapsulation |

หลักที่ทีมยึด: **Composition ก่อน, Inheritance ตื้น ๆ เฉพาะ "is-a" จริง**, สื่อสารข้ามระบบผ่าน Event Channel (ไม่อ้างอิงกันตรง ๆ), พึ่ง interface ไม่ใช่คลาสจริง

---

## 1. Core — รากฐาน (`HollowChains.Core`) ✅ มีโค้ดแล้ว (P2.0)
```mermaid
classDiagram
    class EventChannelBaseSO {
        <<abstract>>
        -string _description
        +string Description
    }
    class VoidEventChannelSO {
        -event Action Raised
        +Raise() void
        +Subscribe(Action listener) void
        +Unsubscribe(Action listener) void
    }
    class EventChannelSO~T~ {
        <<abstract>>
        -event Action~T~ Raised
        +Raise(T value) void
        +Subscribe(Action~T~ listener) void
        +Unsubscribe(Action~T~ listener) void
    }
    class IntEventChannelSO
    class StringEventChannelSO
    EventChannelBaseSO <|-- VoidEventChannelSO
    EventChannelBaseSO <|-- EventChannelSO~T~
    EventChannelSO~T~ <|-- IntEventChannelSO
    EventChannelSO~T~ <|-- StringEventChannelSO

    class IState {
        <<interface>>
        +Enter() void
        +Tick(float deltaTime) void
        +Exit() void
    }
    class StateMachine {
        -IState _current
        +IState Current
        +event StateChanged
        +ChangeState(IState next) void
        +Tick(float deltaTime) void
    }
    StateMachine o-- IState

    class IService {
        <<interface>>
    }
    class ServiceLocator {
        <<static>>
        -Dictionary Services
        +Register~T~(T service) void
        +Unregister~T~() void
        +TryGet~T~(out T service) bool
        +Get~T~() T
        +Clear() void
    }
    ServiceLocator o-- IService

    class IIdentifiable {
        <<interface>>
        +string Id
    }
```

## 2. Data — ข้อมูลเกม (`HollowChains.Data`, ScriptableObject)
```mermaid
classDiagram
    class IIdentifiable {
        <<interface>>
        +string Id
    }
    class GameDataSO {
        <<abstract>>
        -string _id
        +string Id
    }
    IIdentifiable <|.. GameDataSO

    class QuestSO {
        +LocalizedString Title
        +IReadOnlyList~QuestPhaseSO~ Phases
    }
    class QuestPhaseSO {
        +LocalizedString Objective
        +PhaseConditionSO CompleteWhen
    }
    class PhaseConditionSO {
        <<abstract>>
        +Bind(Action onMet) void
        +Unbind() void
    }
    class EventRaisedCondition {
        -VoidEventChannelSO _event
    }
    class ItemSO {
        +LocalizedString DisplayName
        +bool IsKeyItem
    }
    class DialogueLineSO {
        +string SpeakerId
        +LocalizedString Text
        +float Duration
    }
    class CharacterStatsSO {
        +int MaxHealth
        +int StartHumanity
    }

    GameDataSO <|-- QuestSO
    GameDataSO <|-- QuestPhaseSO
    GameDataSO <|-- ItemSO
    GameDataSO <|-- DialogueLineSO
    GameDataSO <|-- CharacterStatsSO
    QuestSO *-- QuestPhaseSO
    QuestPhaseSO o-- PhaseConditionSO
    PhaseConditionSO <|-- EventRaisedCondition
```

## 3. Gameplay — ผู้เล่น / Interact / ค่าสถานะ (`HollowChains.Gameplay`) — ✅ Player ส่วนเคลื่อนที่มีโค้ดแล้ว (P2.1); Interact/Health = แผน
```mermaid
classDiagram
    class PlayerController {
        -StateMachine _stateMachine
        -PlayerMovementConfigSO _config
        -IPlayerInput _input
        +bool IsCarrying
        +bool IsLocked
        +EnterCutscene() void
        +ExitCutscene() void
        +SetCarrying(bool carrying) void
        +SetSprintAllowed(bool allowed) void
        ~CameraRelative(Vector2 input) Vector3
    }
    class IPlayerInput {
        <<interface>>
        +Vector2 Move
        +Vector2 Look
        +bool SprintHeld
        +event InteractPressed
    }
    class PlayerInputReader {
        -InputActionReference _move
        -InputActionReference _sprint
    }
    IPlayerInput <|.. PlayerInputReader
    class PlayerMovementConfigSO {
        <<Data>>
        +float WalkSpeed
        +float RunSpeed
        +float SprintSpeed
        +float CarryMultiplier
    }
    PlayerController o-- PlayerMovementConfigSO
    PlayerController o-- IPlayerInput
    class PlayerMotor {
        +Move(Vector3 dir, float speed, config, float dt) void
        +Halt(config, float dt) void
        +bool IsGrounded
    }
    class PlayerState {
        <<abstract>>
        #PlayerController Owner
        +Enter() void
        +Tick(float deltaTime)* void
        +Exit() void
    }
    class LocomotionState
    class CarryState
    class SprintState
    class LockedState
    class IState {
        <<interface>>
    }
    IState <|.. PlayerState
    PlayerState <|-- LocomotionState
    PlayerState <|-- CarryState
    PlayerState <|-- SprintState
    PlayerState <|-- LockedState
    PlayerController *-- PlayerMotor
    PlayerController *-- PlayerInputReader : same GameObject
    PlayerController ..> PlayerState

    class IInteractable {
        <<interface>>
        +LocalizedString Prompt
        +CanInteract(Interactor who) bool
        +Interact(Interactor who) void
    }
    class InteractableBase {
        <<abstract>>
        #VoidEventChannelSO _onInteracted
        +Interact(Interactor who) void
        #OnInteract(Interactor who) void*
    }
    class PickupInteractable {
        -ItemSO _item
    }
    class InspectInteractable {
        -DialogueLineSO _thought
    }
    class ScriptedInteractable {
        -PlayableDirector _timeline
    }
    class Interactor {
        -IInteractable _focused
        +TryInteract() void
    }
    IInteractable <|.. InteractableBase
    InteractableBase <|-- PickupInteractable
    InteractableBase <|-- InspectInteractable
    InteractableBase <|-- ScriptedInteractable
    Interactor o-- IInteractable
    PlayerController *-- Interactor

    class IDamageable {
        <<interface>>
        +TakeDamage(int amount) void
    }
    class Health {
        -int _current
        +int Current
        +SetTo(int value) void
        +Heal(int amount) void
    }
    class HumanityState {
        -int _value
        +int Value
        +Change(int delta) void
    }
    IDamageable <|.. Health
    PlayerController *-- Health
    PlayerController *-- HumanityState
```
`ScriptedInteractable` = prompt "วิ่งเข้าไปกอด" (SC-04) และ "ยื่นมือ" (SC-06) · `HumanityState` ไม่มี UI (D-002)

## 4. Quest / Save / Scene flow (`HollowChains.Gameplay`)
```mermaid
classDiagram
    class IService {
        <<interface>>
    }
    class ISaveable {
        <<interface>>
        +string SaveKey
        +CaptureState() object
        +RestoreState(object state) void
    }
    class QuestService {
        -List~QuestState~ _active
        +Start(QuestSO quest) void
        +GetState(string questId) QuestState
    }
    class QuestState {
        +string QuestId
        +int PhaseIndex
        +bool IsComplete
    }
    class SaveService {
        -List~ISaveable~ _saveables
        +Save(string checkpointId) void
        +Load() bool
    }
    class SaveData {
        +int SaveVersion
        +string CheckpointId
        +Dictionary States
    }
    class SceneFlowService {
        +LoadScene(string sceneId) Task
        +Preload(string sceneId) void
    }
    class CheckpointTrigger {
        -string _checkpointId
    }

    IService <|.. QuestService
    IService <|.. SaveService
    IService <|.. SceneFlowService
    ISaveable <|.. QuestService
    QuestService *-- QuestState
    SaveService o-- ISaveable
    SaveService ..> SaveData
    CheckpointTrigger ..> SaveService
```

## 5. Narrative — บทพูด / Cutscene / Comic hook (`HollowChains.Narrative`)
```mermaid
classDiagram
    class IComicPage {
        <<interface>>
        +Play(Action onFinished) void
        +Skip() void
    }
    class ComicPlayer {
        -List~IComicPage~ _pages
        +PlayAll() void
    }
    class SubtitleComicPage {
        -DialogueLineSO[] _lines
    }
    class PanelComicPage {
        <<P7>>
    }
    IComicPage <|.. SubtitleComicPage
    IComicPage <|.. PanelComicPage
    ComicPlayer o-- IComicPage

    class DialogueRunner {
        +Play(DialogueLineSO line) void
        +event Action~DialogueLineSO~ LineStarted
    }
    class BarkEmitter {
        -DialogueLineSO[] _barks
        -float _radius
    }
    class InteractiveBeat {
        <<abstract>>
        +Begin(Action onDone) void
        #IsSatisfied() bool*
    }
    class HoldBeat {
        -float _holdSeconds
    }
    class MashBeat {
        -int _pressesNeeded
    }
    InteractiveBeat <|-- HoldBeat
    InteractiveBeat <|-- MashBeat
    BarkEmitter ..> DialogueRunner
```
`SubtitleComicPage` = S00 ตอนนี้ · `PanelComicPage` = Comic จริงใน P7 (เสียบแทนโดยไม่แก้ `ComicPlayer` — Open/Closed)
`HoldBeat` = "ปล่อยราก" (SC-05) · ผลลัพธ์คงที่ ล้มเหลวไม่ได้

## 6. UI (`HollowChains.UI`)
```mermaid
classDiagram
    class UIView {
        <<abstract>>
        +Show() void
        +Hide() void
        #OnShow() void*
    }
    class HealthBarView {
        -IntEventChannelSO _onHealthChanged
    }
    class InteractPromptView
    class QuestTrackerView
    class SubtitleView
    class MenuView {
        <<abstract>>
    }
    class TitleMenu
    class PauseMenu
    class EndScreen
    UIView <|-- HealthBarView
    UIView <|-- InteractPromptView
    UIView <|-- QuestTrackerView
    UIView <|-- SubtitleView
    UIView <|-- MenuView
    MenuView <|-- TitleMenu
    MenuView <|-- PauseMenu
    MenuView <|-- EndScreen
```
UI ฟัง Event Channel เท่านั้น — ไม่อ้างอิง `PlayerController` / `QuestService` ตรง ๆ (UI กับ Gameplay เปลี่ยนแยกกันได้)

## 6b. ✅ มีโค้ดแล้วใน P2.0b–P2.4 (Flow / Triggers / Camera / Interaction / UI)
> แทนที่ส่วน Interaction ใน §3 และ SceneFlow ใน §4 (ส่วน Health/Quest/Save ใน §3–4 ยังเป็นแผนของ P3)
```mermaid
classDiagram
    class ISceneFlowService {
        <<interface>>
        +bool IsLoading
        +string CurrentScene
        +event SceneLoaded
        +Load(string sceneName) void
    }
    class IService {
        <<interface>>
    }
    IService <|-- ISceneFlowService
    class SceneFlowService {
        -CanvasGroup _fade
        +Load(string sceneName) void
        +Adopt(Scene scene) void
    }
    ISceneFlowService <|.. SceneFlowService
    class GameBootstrapper {
        -SceneFlowService _sceneFlow
        -string _firstScene
    }
    class BootSceneLoader {
        <<static>>
    }
    GameBootstrapper o-- SceneFlowService
    BootSceneLoader ..> GameBootstrapper : loads _Boot

    class PlayerTriggerZone {
        <<abstract>>
        -bool _once
        #OnPlayerEnter(PlayerController p)* void
        #OnPlayerExit(PlayerController p) void
    }
    class SceneTransitionTrigger {
        -string _targetScene
    }
    class SprintZone
    class EventTriggerZone {
        -VoidEventChannelSO _onEnter
    }
    PlayerTriggerZone <|-- SceneTransitionTrigger
    PlayerTriggerZone <|-- SprintZone
    PlayerTriggerZone <|-- EventTriggerZone
    SceneTransitionTrigger ..> ISceneFlowService

    class OrbitLookInput {
        -PlayerController _player
        -CinemachineOrbitalFollow _orbit
    }
    class SprintCameraEffect {
        -float _sprintFovBonus
    }

    class IInteractable {
        <<interface>>
        +string Prompt
        +Vector3 Position
        +CanInteract(Interactor who) bool
        +Interact(Interactor who) void
    }
    class InteractableBase {
        <<abstract>>
        -string _prompt
        -VoidEventChannelSO _onInteracted
        +Interact(Interactor who) void
        #OnInteract(Interactor who)* void
    }
    class PickupInteractable {
        -string _itemId
    }
    class InspectInteractable {
        -string _thought
        -StringEventChannelSO _onThought
    }
    class Interactor {
        -StringEventChannelSO _onPromptChanged
        +IInteractable Focused
        +TryInteract() void
    }
    IInteractable <|.. InteractableBase
    InteractableBase <|-- PickupInteractable
    InteractableBase <|-- InspectInteractable
    Interactor o-- IInteractable

    class UIView {
        <<abstract>>
        +Show() void
        +Hide() void
    }
    class StringChannelTextView {
        -StringEventChannelSO _channel
        -float _autoHideSeconds
    }
    UIView <|-- StringChannelTextView
    Interactor ..> StringChannelTextView : EVT_InteractPrompt (event channel)
    InspectInteractable ..> StringChannelTextView : EVT_Thought (event channel)
```
`ScriptedInteractable` (กอด/ยื่นมือ) เลื่อนไป P5 พร้อม Timeline · ข้อความทั้งหมดยังเป็น placeholder อังกฤษ `[PH]` จนกว่าจะมีฟอนต์ไทย + Localization (P3.3)

## 6c. ✅ Quest system (P3.1)
> แทนที่แผน Quest ใน §2/§4 · Objective ยังเป็น string `[PH]` → เปลี่ยนเป็น LocalizedString ใน P3.3
```mermaid
classDiagram
    class IIdentifiable {
        <<interface>>
        +string Id
    }
    class GameDataSO {
        <<abstract>>
        -string _id
        +string Id
    }
    IIdentifiable <|.. GameDataSO
    class QuestSO {
        -string _title
        +IReadOnlyList Phases
    }
    class QuestPhaseSO {
        -string _objective
        +PhaseConditionSO CompleteWhen
    }
    GameDataSO <|-- QuestSO
    GameDataSO <|-- QuestPhaseSO
    QuestSO *-- QuestPhaseSO
    class PhaseConditionSO {
        <<abstract>>
        +CreateWatcher()* IConditionWatcher
    }
    class EventRaisedConditionSO {
        -VoidEventChannelSO _event
    }
    class IConditionWatcher {
        <<interface>>
        +Start(Action onMet) void
        +Dispose() void
    }
    PhaseConditionSO <|-- EventRaisedConditionSO
    PhaseConditionSO ..> IConditionWatcher : creates
    QuestPhaseSO o-- PhaseConditionSO

    class QuestState {
        <<Serializable>>
        +string QuestId
        +int PhaseIndex
        +bool IsComplete
    }
    class QuestRunner {
        +QuestState State
        +QuestPhaseSO CurrentPhase
        +event PhaseStarted
        +event Completed
        +Begin() void
        +ForceAdvance() void
    }
    class IQuestService {
        <<interface>>
        +StartQuest(QuestSO q) void
        +GetState(string id) QuestState
        +GetCurrentPhase(string id) QuestPhaseSO
    }
    class QuestService {
        -List _autoStart
        -StringEventChannelSO _onObjectiveChanged
    }
    IQuestService <|.. QuestService
    QuestService *-- QuestRunner
    QuestRunner *-- QuestState
    QuestRunner o-- IConditionWatcher
```
Watcher แยก state การฟัง event ออกจาก SO (SO เป็น asset ที่แชร์กัน ห้ามเก็บ runtime state) · เงื่อนไขชนิดใหม่ = subclass ใหม่ของ `PhaseConditionSO`

## 6d. ✅ Dialogue / Subtitle (P3.2)
```mermaid
classDiagram
    class DialogueLineSO {
        -string _speakerName
        -string _text
        -bool _isThought
        +float Duration
    }
    class GameDataSO {
        <<abstract>>
    }
    GameDataSO <|-- DialogueLineSO
    class DialogueLineEventChannelSO
    class EventChannelSO~T~ {
        <<abstract>>
    }
    EventChannelSO~T~ <|-- DialogueLineEventChannelSO
    class SubtitleQueue {
        +DialogueLineSO Current
        +bool IsBusy
        +event LineChanged
        +PlayNow(line) void
        +Enqueue(line) void
        +TryPlayAmbient(line) bool
        +Tick(float dt) void
    }
    class IDialogueService {
        <<interface>>
        +PlayNow(line) void
        +Enqueue(line) void
        +TryPlayAmbient(line) bool
    }
    class DialogueRunner {
        -DialogueLineEventChannelSO _playLineRequests
        -StringEventChannelSO _onSubtitle
    }
    IDialogueService <|.. DialogueRunner
    DialogueRunner *-- SubtitleQueue
    class BarkEmitter {
        -DialogueLineSO[] _lines
        -float _cooldownSeconds
    }
    BarkEmitter ..> IDialogueService : TryPlayAmbient
    class InspectInteractable {
        -DialogueLineSO _line
    }
    class LineTriggerZone {
        -DialogueLineSO _line
    }
    InspectInteractable ..> DialogueLineEventChannelSO : EVT_PlayLine
    LineTriggerZone ..> DialogueLineEventChannelSO : EVT_PlayLine
    DialogueLineEventChannelSO ..> DialogueRunner : listened by
```
Gameplay ขอเล่นบทผ่าน `EVT_PlayLine` (ไม่อ้างอิง Narrative) · Barks ไม่ขัดบทหลัก (`TryPlayAmbient`) · UI แสดงจาก `EVT_Subtitle`

## 6e. ✅ Localization + ภาษาไทย (P3.3)
```mermaid
classDiagram
    class LocalizedText {
        <<static>>
        +Resolve(LocalizedString s, string fallback) string
    }
    class DialogueLineSO {
        -LocalizedString _localizedText
        -LocalizedString _localizedSpeaker
        -string _text
    }
    class QuestPhaseSO {
        -LocalizedString _localizedObjective
        -string _objective
    }
    class InteractableBase {
        <<abstract>>
        -LocalizedString _localizedPrompt
        -string _prompt
    }
    DialogueLineSO ..> LocalizedText
    QuestPhaseSO ..> LocalizedText
    InteractableBase ..> LocalizedText
    class ThaiMarkRules {
        <<static>>
        +VerticalAction(string text, int i) ThaiMarkAction
        +ShiftLeft(string text, int i) bool
        +BaseIndex(string text, int i) int
    }
    class ThaiMarkAction {
        <<enumeration>>
        None
        StackOnPrevious
        StackOnNext
        DropBelowBase
    }
    class ThaiMarkPositioner {
        -float _stackGap
        -float _tallShift
        -Adjust(TMP_TextInfo info) void
    }
    ThaiMarkPositioner ..> ThaiMarkRules
    ThaiMarkRules ..> ThaiMarkAction
```
ข้อความมาจาก string table `UI` / `Dialogue` / `Quests` (locale `th` เริ่มต้น) · `ThaiMarkPositioner` ติดกับ TMP ทุกตัวที่แสดงไทย (D-034)

## 6f. ✅ Game flow, เมนู, HP (P3.3b)
```mermaid
classDiagram
    class IState {
        <<interface>>
    }
    class FlowState {
        <<abstract>>
        +string Name*
        #bool GameplayInput*
        #bool Paused*
        +Enter() void
    }
    IState <|.. FlowState
    FlowState <|-- TitleState
    FlowState <|-- PlayingState
    FlowState <|-- PausedState
    FlowState <|-- EndState
    class GameFlowService {
        -StateMachine _machine
        -InputActionReference _pauseAction
        +EnterTitle() void
        +EnterPlaying() void
        +StartNewGame() void
        +Pause() void
        +Resume() void
        +QuitToTitle() void
        +ShowEnd() void
    }
    GameFlowService *-- FlowState
    class GameBootstrapper
    GameBootstrapper ..> GameFlowService : Title or Playing

    class HealthModel {
        +int Max
        +int Current
        +float Normalized
        +bool IsCritical
        +TakeDamage(int) void
        +Heal(int) void
        +SetTo(int) void
    }
    class IDamageable {
        <<interface>>
        +TakeDamage(int) void
    }
    class Health {
        -FloatEventChannelSO _onHealthChanged
    }
    IDamageable <|.. Health
    Health *-- HealthModel
    class CriticalHealthVolume

    class UIView {
        <<abstract>>
    }
    class GameStateView {
        -string[] _visibleInStates
        -Selectable _firstSelected
    }
    UIView <|-- GameStateView
    UIView <|-- SettingsView
    class HealthBarView
    class ChannelButton {
        -VoidEventChannelSO _onClick
    }
    class LocalizedLabel {
        -LocalizedString _text
    }
    class GameSettings {
        <<Serializable>>
        +float MasterVolume
        +float SubtitleScale
        +bool InvertCameraY
        +Current$ GameSettings
        +Save()$ void
    }
    SettingsView ..> GameSettings
    ChannelButton ..> GameFlowService : EVT_UI_* (event channel)
    GameFlowService ..> GameStateView : EVT_GameStateChanged
    Health ..> HealthBarView : EVT_HealthChanged
    Health ..> CriticalHealthVolume : EVT_HealthChanged
```
เมนูไม่อ้างอิง gameplay — ปุ่มยิง `EVT_UI_*`, `GameFlowService` ฟังแล้วประกาศ state กลับผ่าน `EVT_GameStateChanged`

## 7. ภาพรวมการพึ่งพา Assembly
```mermaid
flowchart LR
    Core["HollowChains.Core"]
    Data["HollowChains.Data"] --> Core
    Gameplay["HollowChains.Gameplay"] --> Data
    Narrative["HollowChains.Narrative"] --> Data
    UI["HollowChains.UI"] --> Data
    Combat["HollowChains.Combat (P4)"] --> Data
    Gameplay -. "Event Channels" .- UI
    Gameplay -. "Event Channels" .- Narrative
    Gameplay -. "OnEncounterRequested" .- Combat
```
ลูกศรทึบ = อ้างอิงโค้ด · เส้นประ = คุยผ่าน Event Channel (ไม่อ้างอิงกัน)
