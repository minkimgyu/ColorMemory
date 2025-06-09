# 🎮 Color Memory

Unity를 사용하여 개발한 모바일 퍼즐 2D 게임입니다.  
현재 **Google Play Store**에서 서비스 진행 중입니다.

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

## 📱 플레이 스토어

https://play.google.com/store/apps/details?id=com.mozi.colormemory&hl=ko

### MVP 패턴 기반 UI 시스템

<img src="https://github.com/user-attachments/assets/b82aeb96-b1f3-4d3c-b873-5438c5b6e576" alt="Color Memory Screenshot" width="50%" height="50%" />

Model, View, Presenter의 책임을 명확히 나누어 UI 시스템을 구성하였으며,  
MockViewer 클래스 구현을 통해 단위 테스트를 수행할 수 있도록 설계하였습니다.

### BFS 알고리즘 기반 퍼즐 시스템

<img src="https://github.com/user-attachments/assets/7631885a-a931-4711-8106-19ae8c80ed93" alt="Color Memory Screenshot" width="50%" height="50%" />

퍼즐 게임에서 인접한 동일 색상 블록을 한 번에 색칠하기 위해  
BFS 알고리즘을 활용하여 효율적인 탐색을 구현했습니다.
