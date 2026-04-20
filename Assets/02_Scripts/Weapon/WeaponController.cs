using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

//중요한 Component가 있으면 RequireComponent로 설정해준다
[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform _firePos;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private AudioClip _fireSfx;
    [SerializeField] private MeshRenderer _muzzleFlash;
    
    
    //연사속도
    [SerializeField] private float _fireRate = 0.1f;
    //다음 발사 시각
    private float _nextFire;
    
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.maxDistance = 50f;
        _audio.minDistance = 10f;
       
        
        _firePos = transform.Find("FirePos").transform;
        //Childeren뿐 아니라 자기자신의 Transform반환
        //_firePos = GetComponentInChildren<Transform>();
        
        //둘다 같은 역할.
        _muzzleFlash = transform.Find("FirePos/MuzzleFlash").GetComponent<MeshRenderer>();
        //t_muzzleFlash = _firePos.GetComponentInChildren<MeshRenderer>();
        _muzzleFlash.enabled = false;
    }
    private void Update()
    {
        FireBullet();
    }

    private void FireBullet()
    {
        //Legacy INputManager 사용방법
        //0은 왼쪽,1은 오른쪽.2는 가운데휠버튼
        /*if (Input.GetMouseButtonDown(0))
        {
            //Instantiate(생성객체,위치,각도,부모게임오브젝트)
            Instantiate(_bulletPrefab, _firePos.position, _firePos.rotation);
        }*/
        
        //New InputSystem
        /*if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Instantiate(_bulletPrefab, _firePos.position, _firePos.rotation);
            //음원재생
            //AudioSource.Play("음원이름"); - BGM
            //AudioSource.PlayOneShot(AudioClip,볼륨); - 총소리
            _audio.PlayOneShot(_fireSfx,0.8f);
            //총구 화염 효과
            StartCoroutine(ShowMuzzleFlash());
        }*/

        if (Mouse.current.leftButton.isPressed)
        {
            //Time.time = 흘러가는 시간.
            if (Time.time > _nextFire)
            { 
                _nextFire = Time.time + _fireRate;
                Instantiate(_bulletPrefab, _firePos.position, _firePos.rotation);
                _audio.PlayOneShot(_fireSfx,0.8f);
                StartCoroutine(ShowMuzzleFlash());
            }
        }
    }

    /*
     * 동기방식(sync)
     * 함수 1 (5초)
     * 함수 2 (1초)
     * 순서대로 함수1 실행 후 함수 2 실행
     *
     * 비동기방식(async)
     * 함수 1 (5초)
     * 함수 2 (1초_
     * 함수1,함수2가 실행된다.
     *
     *
     * 대표적 = Thread
     * 여러개가 한번에 도는방식 = multiThread
     * 1. Thread 프로그래밍
     * 2. async / await / Task
     * 3. Co-routine
     * 코루틴 != 멀티쓰레드
     * 
     */
    
    
    private IEnumerator ShowMuzzleFlash()
    {
        //텍스쳐변경
        //Random.Range(정수,정수)   Random.Range(1,10) = 이상~미만
        //Random.Range(실수,실수)   Random.Range(1f~10f) = 이상~이하
        
        //(0,0),(0.5,0),(0.5,0.5),(0,0.5)
        Vector2 offset = new Vector2(Random.Range(0,2),Random.Range(0,2)) * 0.5f;
        _muzzleFlash.material.mainTextureOffset = offset;
        
        //크기조절
        float scale = Random.Range(1.2f, 2.5f);
        _muzzleFlash.transform.localScale = Vector3.one * scale;
        
        //회전각도 설정
        float angle = Random.Range(0,360);
        _muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);
        
        _muzzleFlash.enabled = true;
        //Wait
        yield return new WaitForSeconds(0.2f);
        _muzzleFlash.enabled = false;
    }
}
