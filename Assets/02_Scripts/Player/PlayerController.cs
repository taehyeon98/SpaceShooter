using System;
using System.Dynamic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Strafe = Animator.StringToHash("Strafe");
    private float v;
    private float h;
    private float r;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _rotateSpeed = 200f;

    private Animator _animator;

    //애니메이션 파라미터 해시값 추출
    private readonly int _hashSpeed = Animator.StringToHash("Speed");
    private readonly int _hashStrafe = Animator.StringToHash("Strafe");

    private float _initHP = 100f;
    private float _currHP = 100f;
    
    //delegate = 함수를 저장하기 위한 데이터를 정의
    //public void Sum();
    //delegate 변수명 = sum;
    //delegate SumDelegate = sum;
    
    //Delegate선언
    //public delegat 함수명;
    //public delegate void PlayerDieHandler();
    //delegate정의
    //public static event PlayerDieHandler OnPlayerDead;
    
    //Action : .NET 미리 정의된 델리게이트
    public static event Action OnPlayerDead;
    
    //public static event Action<T1,T2,...,T16> 

    //OnPlayerDead = PlayerDead();
    //OnPlayerDead();
    //OnplayerDead?.Invoke();

    #region 유니티 콜백 메서드

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        InputHandler();
        Movement();
        Animate();
    }

    #region 애니메이션

    private void Animate()
    {
        _animator.SetFloat(_hashSpeed, v);
        _animator.SetFloat(_hashStrafe, h);
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (_currHP > 0f && other.CompareTag("Punch"))
        {
            //Debug.Log(other.gameObject.name);
            PlayerDamaged(10f);
        }
    }

    private void PlayerDamaged(float Damage)
    { 
        _currHP -= Damage;
        //_hpBar.fillAmount = _currHP/_initHP;
        if (_currHP <= 0f)
        {
            //캐릭터 사망
            Debug.Log("사망.");
            //PlayerDead();
            
            //이벤트 발행(Event Raise)
            OnPlayerDead?.Invoke();
        }
    }

    /*private void PlayerDead()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (var monster in monsters)
        {
            //monster.SendMessage("OnPlayerDead",SendMessageOptions.DontRequireReceiver);
            monster.GetComponent<MonsterController>().OnPlayerDead();
        }
    }*/

    #endregion

    #region 입력처리

    private void InputHandler()
    {
        //Legacy Input System
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        r = Input.GetAxis("Mouse X");

        //New InputSystem
    }

    #endregion

    #region 이동처리

    private void Movement()
    {
        //좌표 += 방향*속도*변위*시간차
        /*transform.position += Vector3.forward * (5.0f * v * Time.deltaTime);
        transform.position += Vector3.right * (5.0f * h * Time.deltaTime);*/
        //transform.Translate(벡터 * 속도 * time.deltaTime);

        //방향벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        //이동처리
        transform.Translate(moveDir.normalized * (_speed * Time.deltaTime));
        //회전처리
        //Rotate(회전축 * 속도 * time.deltatime)
        transform.Rotate(Vector3.up * _rotateSpeed * r * Time.deltaTime);
    }
    //정규화 벡터, 단위벡터(Unit vector)
    //Vector3.forward = Vector3(0,0,1)
    //Vector3.right = vector3(1,0,0)
    //Vector3.up = Vector3(0,1,0)
    //Vector3.one = Vector3(1,1,1)
    //Vector3.zero = Vector3(0,0,0)

    #endregion
}