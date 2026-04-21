using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum DrawType
    {
        Sphere,
        Icon
    }

    [SerializeField] private DrawType _drawType = DrawType.Sphere;
    [SerializeField] private Color _color = Color.green;
    [Range(1f, 5f)] [SerializeField] private float _radius = 1f;

    private void OnDrawGizmos()
    {
        // 기즈모 색상 설정
        Gizmos.color = _color;
        if (_drawType == DrawType.Sphere)
        {
            // Sphere 생성
            Gizmos.DrawSphere(transform.position, _radius);
        }
        else
        {
            // Icon 출력
            Gizmos.DrawIcon(transform.position, "Enemy.png");
        }
    }
}
