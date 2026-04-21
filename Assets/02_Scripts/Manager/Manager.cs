using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // 싱글턴을 접근하기 위한 변수
    // 인스턴스를 생성
    // 전역 접근 가능 (static)
    public static Manager Instance = null;
    
    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private GameObject _monsterPrefab;
    [SerializeField] private float _createTime = 3.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var spawnPointGroup = GameObject.Find("SpawnPointGroup").transform;
        spawnPointGroup.GetComponentsInChildren<Transform>(_points);
        _points.RemoveAt(0);
        
        // 몬스터 프리팹 로딩
        _monsterPrefab = Resources.Load<GameObject>("Monster");  
        
        // 델리게이트함수.Invoke();
        // Invoke("함수명", 지연시간)
        // InvokeRepeating("함수명", 지연시간, 반복간격)
        
        InvokeRepeating("CreateMonster", 1.0f, _createTime);
    }

    private void CreateMonster()
    {
        // 난수 발생
        int idx = Random.Range(1, _points.Count);

        //var monster = Instantiate(_monsterPrefab);
        var monster = MonsterPool.Instance.monsterPool.Get();
        
        monster.name = $"Monster_{idx}";

        monster.transform.position = _points[idx].position;
        monster.transform.rotation = Quaternion.identity;
    }
}
