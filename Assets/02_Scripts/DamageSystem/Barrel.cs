using System;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Barrel : MonoBehaviour
{
    //스크립터블오브젝트 참조
    [SerializeField] private BarrelDataSO _barrelData;
    //[SerializeField] private GameObject _explosionEffect;
    //[SerializeField] private AudioClip _explosionSfx;
    private const string Tag_Bullet = "Bullet";
    private int _hitCount;
    private Rigidbody _rb;
    private AudioSource _audio;
    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.maxDistance = 50f;
        _audio.minDistance = 10f;
        
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag(Tag_Bullet))
        {
            _hitCount++;
            if (_hitCount == 3)
            {
                //폭발효과 연출
                ExpBarrel();
            }
        }
    }

    private void ExpBarrel()
    {
        //폭발 원점
        Vector3 explosionPos = transform.position + Random.insideUnitSphere * 2f;
        //주변 폭발력 전달
        Collider[] barrels = Physics.OverlapSphere(explosionPos, _barrelData.radius, _barrelData.barrelLayerMask);

        foreach (Collider col in barrels)
        {
            var rb = col.GetComponent<Rigidbody>();
            
            //폭발 효과
            rb.mass = 2f;
            //Rigidbody.AddExplosionForce(폭발력, 폭발원점, 반경, 위로솟구치는힘)
            rb.AddExplosionForce(_barrelData.explosionForce,explosionPos,_barrelData.radius,_barrelData.upwardForce);
        }
        
        //폭발 파티클 생성
        GameObject effect = Instantiate(_barrelData.explosionEffect,transform.position,transform.rotation);
        _audio.PlayOneShot(_barrelData.explosionSfx,1.2f);
        
        //폭발 충격파 처리
        _impulseSource.GenerateImpulse();
        
        Destroy(effect, 5f);
        Destroy(gameObject, 1.5f);
    }
}
