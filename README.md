# 2D_Charactor_Controller_SouthPawGames
 
# 결과물
https://user-images.githubusercontent.com/56676158/163569869-94c10d88-eb8b-4a4f-b025-2258418f65ae.mp4

# 충돌
## 목표
- 플레이어가 벽에 부딪치거나, 땅을 밟고 있을 수 있도록 구현.
## 구현
- 오브젝트 바운딩 박스 주변에 나오는 레이를 활용하여, 충돌할 레이어를 감지 시스템 구현. 
- 충돌 감지 지점의 위치가 고정되어 벽을 벗어나지 않도록 구현.

<p align="center">
 <img src= "https://user-images.githubusercontent.com/56676158/163570610-53e5003b-c82b-4c6e-9557-7a274dfcb0bd.png">
 </p>
<div align="center"> 바운딩 박스와 좌측,하측을 감지하는 중인 모습 </div>

# 중력
## 목표
- 플레이어가 공중에서 중력을 받을 수 있도록 구현.
## 구현
- 오브젝트가 중력 가속도 -9.81f를 받고서 하강 할 수 있도록 구현.
- 하측 충돌이 감지되지 않는 경우, 오브젝트는 중력의 영향을 받음.

<p align="center"><img src="https://user-images.githubusercontent.com/56676158/163573184-8346028c-2cc3-44c8-8372-d25d96d514ab.gif"</p>
<div align="center"> 중력을 받는 플레이어의 모습</div>
 
 # 이동
 ## 목표
- 플레이어의 좌,우 이동 및 점프 구현.
## 구현
 - 멀티 플랫폼을 제작에 빠른 개발이 가능하도록 InputSystem을 활용하여 키 입력을 설정.

![image](https://user-images.githubusercontent.com/56676158/163574104-f5ff1992-5c6d-43d0-b269-1ce8096a1db0.png)

- 좌,우 입력에 따라 좌측이나 우측 일정 속도 증가.
- 좌, 우 입력에 따라 물체의 스프라이트 이미지 반전.

https://user-images.githubusercontent.com/56676158/163577656-4ae4d5d3-0879-41c4-ab88-9420eef5573f.mp4

- 하측에 충돌이 되어 있는 경우에만 일정 높이까지 점프가 가능.

https://user-images.githubusercontent.com/56676158/163577683-253bdb37-422d-4d95-9c4c-fac3b6a346f1.mp4

- 수직, 수평의 속도를 오브젝트의 위치에 더해줌으로써 오브젝트의 위치가 이동되도록 구현.

# 애니메이션
## 목표
- 상황에 맞는 애니메이션 재생.
## 구현
- 기본상태로 Idle 무한 재생.
- bool 파라미터를 활용하여 플레이어의 상태에 맞는 애니메이션 재생 구현

![image](https://user-images.githubusercontent.com/56676158/163581495-ab114ecd-1ae7-4ff8-a29c-cac21a3bd8cb.png)

- float 파라미터와 Blend Tree를 활용하여 수직 하강 속도에 따라 다른 애니메이션 재생되도록 구현.
![image](https://user-images.githubusercontent.com/56676158/163581374-c703994f-2dae-4170-bf8c-6b58b5757b44.png)







