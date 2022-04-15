# 2D_Charactor_Controller_SouthPawGames
 
# 결과물
https://user-images.githubusercontent.com/56676158/163569869-94c10d88-eb8b-4a4f-b025-2258418f65ae.mp4

# 충돌
플레이어가 벽에 부딪치거나, 땅 위에 있도록 하기위해 구현하였다.

오브젝트 바운딩 박스 주변에 나오는 레이를 활용하여, 충돌할 레이어를 감지합니다.
충돌을 감지하면, 해당 지점의 위치가 고정되도록 한다. 바운딩 박스로 둘러쌓인 오브젝트는 충돌지점을 넘어갈 수 없게 된다.
<p align="center">
 <img src= "https://user-images.githubusercontent.com/56676158/163570610-53e5003b-c82b-4c6e-9557-7a274dfcb0bd.png">
 </p>
<div align="center"> 바운딩 박스와 좌측,하측을 감지하는 중인 모습 </div>

# 중력
플레이어가 공중에서 받게 되는 중력을 구현하였다.

y값에 중력가속도인 -9.81을 하측에 충돌을 받을 때까지 적용하므로, 오브젝트가 중력을 받을 수 있도록 제작하였다.

<p align="center"><img src="https://user-images.githubusercontent.com/56676158/163573184-8346028c-2cc3-44c8-8372-d25d96d514ab.gif"</p>
<div align="center"> 중력을 받는 플레이어의 모습</div>
 
 # 이동
 ## 플레이어 입력
 
