using UnityEngine;
using UnityEngine.InputSystem;

public class LunaShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private SpriteRenderer spriteRenderer;
    private Vector3 firePointRightPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Guardamos la posición original del punto de disparo.
        firePointRightPosition = firePoint.localPosition;
    }

    private void Update()
    {
        UpdateFirePointPosition();

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void UpdateFirePointPosition()
    {
        Vector3 newPosition = firePointRightPosition;

        // Si Luna mira a la izquierda,
        // colocamos el FirePoint al lado contrario.
        if (spriteRenderer.flipX)
        {
            newPosition.x = -Mathf.Abs(firePointRightPosition.x);
        }
        else
        {
            newPosition.x = Mathf.Abs(firePointRightPosition.x);
        }

        firePoint.localPosition = newPosition;
    }

    private void Shoot()
    {
        Vector2 direction = spriteRenderer.flipX
            ? Vector2.left
            : Vector2.right;

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
        }
    }
}