# 🎮 Color Memory

Unity를 사용하여 개발한 모바일 퍼즐 2D 게임입니다.  
[Google Play Store](https://play.google.com/store/apps/details?id=com.mozi.colormemory&hl=ko)에서 서비스 진행 중입니다.

<img src="https://github.com/user-attachments/assets/173de931-1802-4369-99ea-5dad9840e0a0" alt="Color Memory Screenshot"/>

## 📆 개발 기간
2025년 2월 ~ 2025년 6월


## 🧑‍🤝‍🧑 팀 구성
- 총 3명  
  - 클라이언트 프로그래머 1명  
  - 서버 프로그래머 1명  
  - 아트 디자이너 1명


## 🛠️ 개발 도구
- Unity (C#)


## 👨‍💻 담당 역할 및 기여도

- ✅ **Scroll Rect 최적화를 위한 Infinite Scroll 개발** (기여도 100%)
- ✅ **Remote Addressable을 활용한 에셋 시스템 개발 및 빌드 용량 최적화** (기여도 80%)
- ✅ **MVP 패턴을 활용하여 UI 시스템 개발** (기여도 100%)
- ✅ **GitHub Actions을 활용한 테스트, 빌드 자동화 구축** (기여도 100%)
- ✅ **AI 도구를 활용한 테스트 코드 작성 (기여도 100%)** (기여도 100%)
- ✅ **Breadth First Search 알고리즘을 활용한 퍼즐 시스템 개발** (기여도 100%)
- ✅ **DOTween을 활용한 UI 연출 적용** (기여도 100%)
- ✅ **Google Play Store 출시를 위한 구글 로그인, 인앱 업데이트 적용** (기여도 50%)

---

## 🛠️ Scroll Rect 최적화를 위한 Infinite Scroll 개발

저사양 기기에서 Horizontal, Vertical Scroll을 드래그 시 60 fps에서 30 fps까지 떨어지는 문제가 발생했습니다.

### 문제 분석 및 원인 파악 🔎
<img src="https://github.com/user-attachments/assets/ceb95792-7b2c-4a9a-b18e-a7c8d4351002" alt="Color Memory Screenshot"/>
<img src="https://github.com/user-attachments/assets/c4d63c24-17fb-474e-8f51-ba6b4fca8f17" alt="Profiler 분석 결과"/>

UGUI.Rendering.UpdateBatches (32.38ms)와 Canvas.RenderOverlays (20.73ms) 구간에서 병목 현상이 발생했습니다.

스크롤 시 Content 내부 UI 요소 위치 변경으로 인해 Dirty Flag가 활성화되며, 약 1872개의 UI 위치·크기·클리핑 영역이 갱신되는 문제를 확인했습니다.

### Infinite Scroll 구현 ⚙️
<img src="https://github.com/user-attachments/assets/300ebfa8-6ad2-495d-97e7-5be494558bcc" alt="Infinite Scroll 개발"/>

스크롤 끝에 도달하면 UI가 반복되도록 구현하여 실제로는 보이는 UI만 생성·표시하도록 최적화했습니다.

UI 생성·파괴 반복으로 인한 Garbage Collector 과다 호출을 방지하기 위해 Object Pool을 적용했습니다.

<img src="https://github.com/user-attachments/assets/8394907d-eafb-44ac-bfaf-827d34c032d1" alt="Infinite Scroll 개발"/>

적용 결과, UI 수를 기존 750개 → 20개로 대폭 축소할 수 있었습니다.

### 프로파일링 최적화 과정 ⚡
<img src="https://github.com/user-attachments/assets/3b4df04a-7063-47a9-8d5a-73a8958ee0d9" alt="적용 후 프로파일링 결과"/>

Infinite Scroll 적용 후 UI와 Others 영역의 병목이 크게 줄어듦을 확인했습니다.

<img src="https://github.com/user-attachments/assets/92d0f8d9-a61f-4a6a-89c5-fe310942a992" alt="적용 후 프로파일링 결과"/>

Object Pool 반환 시 발생하는 SetParent 호출이 불필요한 연산으로 확인되어 제거하여 최적화했습니다.

### 최종 성능 개선 결과 🚀
<img src="https://github.com/user-attachments/assets/fbd7732d-8efd-485b-b102-1a476728c573" alt="최종 성능 개선 결과"/>

UGUI.Rendering.UpdateBatches: 32.38ms → 9.75ms

VerticalInfiniteScroll.Update: 9.91ms → 4.38ms

Canvas.RenderOverlays: 20.73ms → 8.24ms

프레임당 실행 시간 66.55ms → 20.24ms

<img src="https://github.com/user-attachments/assets/9f199220-dff7-4a1e-9838-89b846b9424f" alt="최종 성능 개선 결과"/>

FPS 15.12fps → 49.41fps 로 최적화 완료했습니다.

---

## 📦 Remote Addressable을 활용한 에셋 시스템 개발 및 빌드 용량 최적화

기존 81MB에 달했던 옛 번들 포트 용량 최적화 및 텍스처 압축을 통해 빌드 용량을 **28MB까지** 줄였습니다.

* 최적화된 에셋을 Amazon S3에 배포하여 서버에서 불러올 수 있도록 구현했습니다.
* 그 결과, 1.42MB에 달했던 웹 번들 크기를 **50MB로** 최적화할 수 있었습니다.

### 빌드 용량 최적화 결과 📊
<img src="https://github.com/user-attachments/assets/c20a2fbf-3de8-4fb4-8a79-1fd371b86699" alt="Color Memory Screenshot"/>

---

## 📦 MVP 패턴 기반 UI 시스템

<img src="https://github.com/user-attachments/assets/b82aeb96-b1f3-4d3c-b873-5438c5b6e576" alt="Color Memory Screenshot"/>

Model, View, Presenter의 책임을 명확히 나누어 UI 시스템을 구성하였으며,  
MockViewer 클래스 구현을 통해 단위 테스트를 수행할 수 있도록 설계하였습니다.

[Collect MVP 구현 코드](https://github.com/minkimgyu/ColorMemory/blob/e4bd29b9868dd1b1104bfdf3d92b1175ab1ff253/ColorMemory/Assets/Scripts/Mode/CollectMode.cs#L237C6-L237C67)

[CollectStageUIModel 코드](https://github.com/minkimgyu/ColorMemory/blob/e4bd29b9868dd1b1104bfdf3d92b1175ab1ff253/ColorMemory/Assets/Scripts/MVP/Collect/CollectStageUIModel.cs#L5)
[CollectStageUIViewer 코드](https://github.com/minkimgyu/ColorMemory/blob/e4bd29b9868dd1b1104bfdf3d92b1175ab1ff253/ColorMemory/Assets/Scripts/MVP/Collect/CollectStageUIViewer.cs#L9C14-L9C34)
[CollectStageUIPresenter 코드](https://github.com/minkimgyu/ColorMemory/blob/e4bd29b9868dd1b1104bfdf3d92b1175ab1ff253/ColorMemory/Assets/Scripts/MVP/Collect/CollectStageUIPresenter.cs#L7)

---

## 🚀 GitHub Actions을 활용한 테스트, 빌드 자동화 구축

테스트 과정을 통과하면 웹 번들 파일이 자동으로 생성되도록 개발했습니다.

* 이를 통해 매번 수동으로 빌드할 필요 없이 `Build` 브랜치에 한하여 자동으로 테스트와 빌드 과정이 실행됩니다.
* 결과적으로 수동 빌드 및 배포에 소요되던 시간을 단축하고 안정적인 빌드 환경을 구축하는 데 기여했습니다.

### GitHub Actions 워크플로우 🛠️
<img src="https://github.com/user-attachments/assets/66477a81-daee-4018-bb1a-95b4c3c266e2" alt="Color Memory Screenshot"/>

---

## 🧪 AI 도구를 활용한 테스트 코드 작성

AI 도구를 활용해서 엣지 케이스 및 테스트 항목을 제안받고 이를 기반으로 다양한 테스트 코드를 생성하여 검증했습니다.

* **Play Mode 테스트:**
    * 각 스테이지마다 알맞은 레벨 데이터가 생성되는지 테스트합니다.
    * 웹 서버에서 데이터를 가져와 클라이언트에서 사용 가능한지 테스트합니다.
* **Edit Mode 테스트:**
    * Challenge, Collect 모드 UI 시스템에서 각 레이어(Model, View, Presenter) 간의 데이터 흐름이 명확한지 테스트했습니다.

결과적으로 테스트 코드 작성을 효율적으로 할 수 있었고, 코드 상 문제점을 사전에 검증할 수 있었습니다.

[LevelTest 코드](https://github.com/minkimgyu/ColorMemory/blob/9e38ff733c811e34c43812a776ded6daeb1b4874/ColorMemory/Assets/Tests/PlayMode/LevelTest.cs#L29)

[WebServerTest 코드](https://github.com/minkimgyu/ColorMemory/blob/9e38ff733c811e34c43812a776ded6daeb1b4874/ColorMemory/Assets/Tests/PlayMode/WebServerTest.cs#L195)

[ChallengeModeMVPTest 코드](https://github.com/minkimgyu/ColorMemory/blob/33e192c557a53b4c02b6c399165b04eb60b5ed0a/ColorMemory/Assets/Tests/EditMode/ChallengeModeMVPTest.cs#L7)

[CollectModeMVPTest 코드](https://github.com/minkimgyu/ColorMemory/blob/33e192c557a53b4c02b6c399165b04eb60b5ed0a/ColorMemory/Assets/Tests/EditMode/CollectModeMVPTest.cs#L4)

### Unity Test Runner 결과 🟢
<img src="https://github.com/user-attachments/assets/e341a1e0-0f85-4195-8a39-dfb9fc564a48" alt="Color Memory Screenshot" />

---

## 🛠️ Breadth First Search 알고리즘 기반 퍼즐 시스템

<img src="https://github.com/user-attachments/assets/7631885a-a931-4711-8106-19ae8c80ed93" alt="Color Memory Screenshot"/>

퍼즐 게임에서 인접한 동일 색상 블록을 한 번에 색칠하기 위해  
Breadth First Search 알고리즘을 활용하여 효율적인 탐색을 구현했습니다.

[BFS 구현 코드](https://github.com/minkimgyu/ColorMemory/blob/33e192c557a53b4c02b6c399165b04eb60b5ed0a/ColorMemory/Assets/Scripts/FSM/CollectState/PaintState.cs#L248)

---

## 💫 DOTween을 활용한 UI 연출 적용

DOTween을 활용해서 퍼즐 게임에 알맞은 UI 연출을 제작했습니다.

### UI 연출 예시 🎬

<img src="https://github.com/user-attachments/assets/4c264b09-318f-47bc-99dc-d20ad856845f" alt="Color Memory Screenshot"/>
</br>
연출 영상: https://www.youtube.com/watch?v=pQLR3cqxy_I
