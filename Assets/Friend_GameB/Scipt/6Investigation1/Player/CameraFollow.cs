using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // 따라갈 대상을 Inspector 창에서 드래그해서 넣어주세요
    public Transform target;

    // 카메라가 얼마나 부드럽게 따라올지 조절하는 값 (작을수록 느릿하게 따라옴)
    public float smoothSpeed = 0.125f;

    // 카메라와 대상 사이의 거리(Offset)
    public Vector3 offset = new Vector3(0, 0, -10);

    void LateUpdate()
    {
        if (target != null)
        {
            // 대상의 위치에 offset을 더한 목표 위치 계산
            Vector3 desiredPosition = target.position + offset;

            // 현재 위치에서 목표 위치까지 부드럽게 보간(Lerp)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // 카메라 위치 업데이트
            transform.position = smoothedPosition;
        }
    }
}