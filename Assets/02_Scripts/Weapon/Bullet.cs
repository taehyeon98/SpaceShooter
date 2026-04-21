using System;
using UnityEngine;
using Unity.Cinemachine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _force = 1200f;
    private CinemachineImpulseSource _impulseSource;
    private TrailRenderer _trail;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody>();
        _trail = GetComponent<TrailRenderer>();
    }

    public void Fire(Vector3 pos, Quaternion rot)
    {
        _impulseSource.GenerateImpulse();
        transform.SetPositionAndRotation(pos, rot);
        //물리속성 초기화
        _rb.linearVelocity = _rb.angularVelocity = Vector3.zero;
        _rb.rotation = rot;
        
        //트레일 렌더러 초기화
        _trail.Clear();
        
        //뉴튼(N)
        _rb.AddRelativeForce(Vector3.forward * _force);
    }
}
