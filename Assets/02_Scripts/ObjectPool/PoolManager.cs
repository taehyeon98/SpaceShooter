using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class PoolManager<T> : IDisposable where T : MonoBehaviour
{
    //풀 변수 선언
    private readonly IObjectPool<T> _pool;
    
    //생성자 선언 (Constructor)
    public PoolManager(T prefab, int defaultCapacity = 10, int maxSize = 20)
    {
        //풀 초기화
        _pool = new UnityEngine.Pool.ObjectPool<T>
            (
                //defaultCapacity가 10을 넘어가면 동작.즉, 풀에서 사용가능한 오브젝트가 없을때 호출됨.
                createFunc: () => Object.Instantiate(prefab),
                //풀에서 오브젝트를 꺼낼때 호출
                actionOnGet: obj => obj.gameObject.SetActive(true),
                //풀에 오브젝트를 다시 넣을때 호출
                actionOnRelease: obj => obj.gameObject.SetActive(false),
                //개수가 넘게 생성된 오브젝트를 삭제할때
                actionOnDestroy: obj => Object.Destroy(obj.gameObject),
                //중복 반납 체크
                collectionCheck: true,
                //초기 생성 개수 
                defaultCapacity: defaultCapacity,
                //최대 개수
                maxSize: maxSize
            );

    }

    //외부 접근 메서드 선언
    public T Get()
    {
        return _pool.Get();
    }
    //객체 반납
    public void Release(T obj)
    {
        _pool.Release(obj);
    }
    
    //Dispose가 되었는지 확인필요
    private bool _isDisposed = false;
    
    public void Dispose()
    {
        //메모리 해제 처리
        if (!_isDisposed)
        {
            return;
        }
        _pool.Clear();
        _isDisposed = true;
    }
}