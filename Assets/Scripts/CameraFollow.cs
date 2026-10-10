using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float minX;
[SerializeField] private float maxX;

    private float offsetX;
    private float fixedY;
    private float fixedZ;

    private void Start()
    {
        offsetX = transform.position.x - target.position.x;
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(
            target.position.x + offsetX,
            fixedY,
            fixedZ
        );

        targetPosition.x = Mathf.Clamp(
        targetPosition.x,
        minX,
        maxX
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}