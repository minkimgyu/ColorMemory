using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    void Active(bool nowActive); // 오브젝트 활성화 / 비활성화
    void SetParent(Transform parent); // 부모 지정
    void ReturnToPool();
    void InjectReturnToPoolEvent(System.Action<IPoolObject> ReturnToPool); // 비활성화 이벤트 주입
}

public interface IScrollItem : IPoolObject
{
    //void ChangeSize(Vector2 size); // 위치가 새롭게 지정되는 경우 호출
    //void OnRefresh(int index); // 위치가 새롭게 지정되는 경우 호출
    void ChangeLocalScale(Vector2 scale); // 스케일 변경
    void ChangeLocalPosition(Vector2 pos); // 포지션 변경
    Vector2 GetLocalPosition(); // 포지션 가져오기
    void ChangeSibiling(bool toTtop); // 하이라키상 인덱스 변경
}
