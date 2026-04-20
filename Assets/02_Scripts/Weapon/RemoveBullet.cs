using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    private const string Tag_Bullet = "Bullet";
    [SerializeField] private GameObject _sparkEffect;
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tag_Bullet))
        {
            //contacts[]배열 사용하면 가비지컬렉터 발생.
            //GetContact()로 사용해야 가비지컬렉터 발생X.
            //GetContact의 인덱스 0 = 0번째 충돌지점의 정보 가져오는것
            ContactPoint contactPoint = other.GetContact(0);
            //충돌좌표
            Vector3 point = contactPoint.point;
            //법선벡터
            Vector3 normal = contactPoint.normal * -1;
            //법선벡터가 바로보는 각도 산출(Quaternion)
            Quaternion rot = Quaternion.LookRotation(normal);
            //스파크 생성
            GameObject spark = Instantiate(_sparkEffect,point,rot);
            Destroy(spark, 0.5f);
            
            //충돌한 게임오브젝트 
            Destroy(other.gameObject);
            //스크립트 삭제
            //Destroy(this);
            //스크립트가 들어가있는 오브젝트 삭제
            //Destroy(this.gameObject);
        }
    }
}
