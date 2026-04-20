using System;
using UnityEngine;
using Unity.Cinemachine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _force = 1200f;
    private CinemachineImpulseSource _impulseSource;

    private void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody>();
        //뉴튼(N)
        _rb.AddRelativeForce(Vector3.forward * _force);
        //같은 방법
        //GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * _force);
        _impulseSource.GenerateImpulse();
    }
}
