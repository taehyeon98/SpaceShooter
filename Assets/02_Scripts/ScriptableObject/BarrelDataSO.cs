using UnityEngine;

[CreateAssetMenu(fileName = "BarrelDataSO", menuName = "Scriptable Objects/BarrelDataSO")]
public class BarrelDataSO : ScriptableObject
{
     [Header("폭발옵션")] 
     public float explosionForce = 1500f;
     public float upwardForce = 100f;
     public float radius = 10f;
     public GameObject explosionEffect;
     public LayerMask barrelLayerMask;
     
     [Header("효과음")] 
     public AudioClip explosionSfx;
}
