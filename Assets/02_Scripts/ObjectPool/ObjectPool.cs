using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//제너릭 타입으로 클래스 생성.
//<T> where로 조건 설정 가능
public class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T _Prefab;
    [SerializeField] private int _initialSize = 10;
    
    //Queue 자료형(FIFO 선입선출 <==> Stack 자료형(LIFO 후입선출)
    private Queue<T> _pool = new Queue<T>();

    public virtual void Awake()
    {
        //초기화작업
        for (int i = 0; i < _initialSize; i++)
        { 
            //생성
            T obj = Instantiate(_Prefab);
            // 큐에 넣기전에 비활성화
            obj.gameObject.SetActive(false); //Awake,OnEnable,Ondisable만 호출. 
            _pool.Enqueue(obj);
        }
    }
    
    //풀에서 사용가능한 객체를 리턴하는 메서드
    public T Get()
    {
        //풀 개수가 남아있는지 확인
        if (_pool.Count > 0)
        {
            //풀에서 가져오기
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        //_pool이 비어있으면 미리 만들어둔 객체를 다 사용했음.
        //풀이 비어있으면 새로 생성.
        return Instantiate(_Prefab);
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
