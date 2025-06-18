# 🎮 Color Memory

Unity를 사용하여 개발한 모바일 퍼즐 2D 게임입니다.  
[Google Play Store](https://play.google.com/store/apps/details?id=com.mozi.colormemory&hl=ko)에서 서비스 진행 중입니다.

<img src="https://github.com/user-attachments/assets/05950e19-3251-4573-a6cc-9a71cb2ed951" alt="Color Memory Screenshot" width="85%" height="85%" />

## 📆 개발 기간
2025년 2월 ~ 2025년 5월


## 🧑‍🤝‍🧑 팀 구성
- 총 3명  
  - 클라이언트 프로그래머 1명  
  - 서버 프로그래머 1명  
  - 아트 디자이너 1명


## 🛠️ 개발 도구
- Unity (C#)


## 👨‍💻 담당 역할 및 기여도

- ✅ **MVP 패턴을 활용하여 UI 시스템 개발** (기여도 100%)
- ✅ **Breadth First Search 알고리즘을 활용한 퍼즐 시스템 개발** (기여도 100%)
- ✅ **Remote Addressable을 활용한 에셋 시스템 개발 및 빌드 용량 최적화** (기여도 80%)
- ✅ **DOTween을 활용한 UI 연출 적용** (기여도 100%)
- ✅ **GitHub Actions을 활용한 테스트, 빌드 자동화 구축** (기여도 100%)
- ✅ **Google Play Store 출시를 위한 구글 로그인, 인앱 업데이트 적용** (기여도 50%)

---

## 📦 MVP 패턴 기반 UI 시스템

<img src="https://github.com/user-attachments/assets/b82aeb96-b1f3-4d3c-b873-5438c5b6e576" alt="Color Memory Screenshot" width="50%" height="50%" />

Model, View, Presenter의 책임을 명확히 나누어 UI 시스템을 구성하였으며,  
MockViewer 클래스 구현을 통해 단위 테스트를 수행할 수 있도록 설계하였습니다.

[Collect MVP 구현 코드](https://github.com/minkimgyu/ColorMemory/blob/e4bd29b9868dd1b1104bfdf3d92b1175ab1ff253/ColorMemory/Assets/Scripts/Mode/CollectMode.cs#L237C6-L237C67)

[CollectStageUIModel 코드](https://github.com/minkimgyu/ColorMemory/blob/e4bd29b9868dd1b1104bfdf3d92b1175ab1ff253/ColorMemory/Assets/Scripts/MVP/Collect/CollectStageUIModel.cs#L5)
[CollectStageUIViewer 코드](https://github.com/minkimgyu/ColorMemory/blob/e4bd29b9868dd1b1104bfdf3d92b1175ab1ff253/ColorMemory/Assets/Scripts/MVP/Collect/CollectStageUIViewer.cs#L9C14-L9C34)
[CollectStageUIPresenter 코드](https://github.com/minkimgyu/ColorMemory/blob/e4bd29b9868dd1b1104bfdf3d92b1175ab1ff253/ColorMemory/Assets/Scripts/MVP/Collect/CollectStageUIPresenter.cs#L7)

---

## 🛠️ Breadth First Search 알고리즘 기반 퍼즐 시스템

<img src="https://github.com/user-attachments/assets/7631885a-a931-4711-8106-19ae8c80ed93" alt="Color Memory Screenshot" width="50%" height="50%" />

퍼즐 게임에서 인접한 동일 색상 블록을 한 번에 색칠하기 위해  
Breadth First Search 알고리즘을 활용하여 효율적인 탐색을 구현했습니다.

---

## 📦 Remote Addressable을 활용한 에셋 시스템 개발 및 빌드 용량 최적화

기존 81MB에 달했던 옛 번들 포트 용량 최적화 및 텍스처 압축을 통해 빌드 용량을 **28MB까지** 줄였습니다.

* 최적화된 에셋을 Amazon S3에 배포하여 서버에서 불러올 수 있도록 구현했습니다.
* 그 결과, 1.42MB에 달했던 웹 번들 크기를 **50MB로** 최적화할 수 있었습니다.

### 빌드 용량 최적화 결과 📊
<img src="https://github.com/user-attachments/assets/c20a2fbf-3de8-4fb4-8a79-1fd371b86699" alt="Color Memory Screenshot" width="50%" height="50%" />

---

## 💫 DOTween을 활용한 UI 연출 적용

DOTween을 활용해서 퍼즐 게임에 알맞은 UI 연출을 제작했습니다.

### UI 연출 예시 🎬

<img src="https://github.com/user-attachments/assets/4c264b09-318f-47bc-99dc-d20ad856845f" alt="Color Memory Screenshot" width="50%" height="50%" />
</br>
연출 영상: https://www.youtube.com/watch?v=pQLR3cqxy_I

---

## 🚀 GitHub Actions을 활용한 테스트, 빌드 자동화 구축

테스트 과정을 통과하면 웹 번들 파일이 자동으로 생성되도록 개발했습니다.

* 이를 통해 매번 수동으로 빌드할 필요 없이 `Build` 브랜치에 한하여 자동으로 테스트와 빌드 과정이 실행됩니다.
* 결과적으로 수동 빌드 및 배포에 소요되던 시간을 단축하고 안정적인 빌드 환경을 구축하는 데 기여했습니다.

### GitHub Actions 워크플로우 🛠️
<img src="https://github.com/user-attachments/assets/66477a81-daee-4018-bb1a-95b4c3c266e2" alt="Color Memory Screenshot" width="50%" height="50%" />

---

## 🧪 AI 도구를 활용한 테스트 코드 작성

AI 도구를 활용해서 엣지 케이스 및 테스트 항목을 제안받고 이를 기반으로 다양한 테스트 코드를 생성하여 검증했습니다.

* **Play Mode 테스트 (런타임 테스트):**
    * 각 스테이지마다 알맞은 레벨 데이터가 생성되는지 테스트합니다.
    * 웹 서버에서 데이터를 가져와 클라이언트에서 사용 가능한지 테스트합니다.
* **Edit Mode 테스트 (에디터 테스트):**
    * Challenge, Collect 모드 UI 시스템에서 각 레이어(Model, View, Presenter) 간의 데이터 흐름이 명확한지 테스트했습니다.

* 결과적으로 테스트 코드 작성을 효율적으로 할 수 있었고, 코드 상 문제점을 사전에 검증할 수 있었습니다.

### Unity Test Runner 결과 🟢
<img src="https://github.com/user-attachments/assets/e341a1e0-0f85-4195-8a39-dfb9fc564a48" alt="Color Memory Screenshot" width="50%" height="50%" />
