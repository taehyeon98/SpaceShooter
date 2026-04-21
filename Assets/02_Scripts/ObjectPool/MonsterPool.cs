using UnityEngine;


public class MonsterPool : MonoBehaviour
{
    public static MonsterPool Instance;
    public PoolManager<MonsterController> monsterPool;

    private void Awake()
    {
        Instance = this;
        var monster = Resources.Load<MonsterController>("Monster");
        monsterPool = new PoolManager<MonsterController>(monster);
    }
}