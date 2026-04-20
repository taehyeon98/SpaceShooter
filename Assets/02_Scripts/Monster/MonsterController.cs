using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    //몬스터 상태 정의
    public enum MonsterState
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD
    }

    //몬스터 상태저장 변수
    [SerializeField] private MonsterState _monsterState;

    //추적 사정거리
    [SerializeField] private float _traceDistance = 10f;

    //공격 사정거리
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private float _monsterHP = 100f;

    //컴포넌트 캐싱
    private Transform _monsterTransform;
    private Transform _playerTransform;
    
    //WaitForSeconds 캐싱
    private WaitForSeconds _ws;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    
    //몬스터의 모든 Collider 저장할 컬렉션
    private List<Collider> _mColliders = new List<Collider>(); // new();
    
    private void OnEnable()
    {
        //이벤트구독(subscribe)
        PlayerController.OnPlayerDead += OnPlayerDead;
    }

    private void OnDisable()
    {
        //이벤트 구독 해지(unsubscribe)
        PlayerController.OnPlayerDead -= OnPlayerDead;
    }

    //애니메이터 파라메터 해시값 추출
    private readonly int hashIsTrace = Animator.StringToHash("IsTrace");
    private readonly int hashIsAttack = Animator.StringToHash("IsAttack");
    private readonly int hashDead = Animator.StringToHash("Dead");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDead = Animator.StringToHash("PlayerDead");
    
    private void Awake()
    {
        _ws = new WaitForSeconds(0.3f);
        _monsterTransform = GetComponent<Transform>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        //Collider Component 추출후 리스트에 저장.
        _monsterTransform.GetComponentsInChildren<Collider>(_mColliders);

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    //코루틴1 - 몬스터의 상태갱신
    private IEnumerator CheckMonsterState()
    {
        while (!_isDead)
        {
            if (_monsterState == MonsterState.DEAD)
            {
                yield break;
            }

            //Player와 Moster간의 거리계산
            //float dist = Vector3.Distance(_monsterTransform.position, _playerTransform.position);
            //벡터의 뺄셈연산 A-B = A와B간의 벡터
            float dist = (_monsterTransform.position - _playerTransform.position).sqrMagnitude;

            if (dist <= _attackDistance * _attackDistance)
            {
                //공격사정거리 이내
                _monsterState = MonsterState.ATTACK;
            }
            else if (dist <= _traceDistance * _traceDistance)
            {
                //추적사정거리 이내
                _monsterState = MonsterState.TRACE;
            }
            else
            {
                //모든 사정거리 외
                _monsterState = MonsterState.IDLE;
            }

            yield return _ws;
        }
    }

    //코루틴2 - 몬스터의 상태에 따라 행동
    private IEnumerator MonsterAction()
    {
        while (!_isDead)
        {
            switch (_monsterState)
            {
                case MonsterState.IDLE:
                    //Idle 로직
                    _navMeshAgent.isStopped = true;
                    _animator.SetBool(hashIsTrace, false);
                    break;
                case MonsterState.TRACE:
                    //Trace 로직
                    _navMeshAgent.SetDestination(_playerTransform.position);
                    //_navMeshAgent.destination = _playerTransform.position;
                    _navMeshAgent.isStopped = false;
                    //Walking Animation
                    _animator.SetBool(hashIsAttack, false);
                    _animator.SetBool(hashIsTrace, true);
                    break;
                case MonsterState.ATTACK:
                    //Attack 로직
                    _navMeshAgent.isStopped = true;
                    _animator.SetBool(hashIsAttack, true);
                    break;
                case MonsterState.DEAD:
                    _isDead = true;
                    _navMeshAgent.isStopped = true;
                    _animator.SetTrigger(hashDead);
                    ToggleColliders(false);
                    //TODO: 오브젝트 풀로 반환.
                    break;
            }

            yield return _ws;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (!_isDead && other.collider.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            _animator.SetTrigger(hashHit);
            _monsterHP -= 10.0f;
            
            if (_monsterHP <= 0f)
            {
                _monsterState = MonsterState.DEAD;
            }
        }
    }

    private void ToggleColliders(bool active)
    {
        //TODO: 오류수정
        foreach (var coll in _mColliders )
        {
            coll.enabled = active;
        }
    }

    public void OnPlayerDead()
    {
        //댄스 애니메이션 처리
        _animator.SetTrigger(hashPlayerDead);
        //네비메시 정지
        _navMeshAgent.isStopped = true;
        //코루틴 정지
        /*StopCoroutine(CheckMonsterState());
        StopCoroutine(MonsterAction());*/
        
        StopAllCoroutines();
    }
}