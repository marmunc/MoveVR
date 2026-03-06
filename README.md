# VR Test Task (Unity + XRI)

Небольшая сцена для проверки базового VR-взаимодействия в Unity: передвижение в VR и “нажатие кнопки” **лучом (Far)** или **рукой (Near/Poke)** с отображением способа активации на самой кнопке.

## Стек
- Unity: **6000.2.7f2**
- XR Interaction Toolkit (XRI): **3.1.2**
- XR Plug-in: **OpenXR**
- Input System: **New Input System**

## Что сделано
- Две сцены:
  - **`Main_VR`** — для запуска со шлемом (без симулятора).
  - **`Dev_Sim`** — для теста без шлема через XR Device Simulator.
- Взаимодействие с 3D-кнопкой (`XRSimpleInteractable`):
  - **Ray (Far)** — дальний выбор/нажатие.
  - **Hand (Near)** — ближний выбор/нажатие.
  - **Hand (Poke)** — тычок `XRPokeInteractor` (если присутствует).
- На кнопке отображается текст: `Activated by: Ray/Hand/Hand (Poke)`.
- Поведение кнопки “как клик”: после `SelectEnter` объект автоматически “отпускается” (без залипания луча).