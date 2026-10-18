using UnityEngine;
using UnityEngine.InputSystem;

public class LunaShooting : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Shooting Settings")]
    [SerializeField] private float fireInterval = 0.35f;
    [SerializeField] private float shootPreparationTime = 0.375f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 firePointRightPosition;

    private bool isShooting;
    private float nextFireTime;
    private float preparationEndTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Guardamos la posición original del punto de disparo.
        firePointRightPosition = firePoint.localPosition;
    }

    private void Update()
    {
        UpdateFirePointPosition();

        if (Keyboard.current == null)
        {
            StopShooting();
            return;
        }

        bool spacePressed = Keyboard.current.spaceKey.isPressed;

        // Comenzar la animación al presionar Space.
        if (spacePressed && !isShooting)
        {
            StartShooting();
        }

        // Detener el disparo al soltar Space.
        if (!spacePressed && isShooting)
        {
            StopShooting();
        }

        // Generar proyectiles mientras Space permanece presionada.
        if (isShooting &&
            Time.time >= preparationEndTime &&
            Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireInterval;
        }
    }

    private void StartShooting()
    {
        isShooting = true;

        animator.SetBool("IsShooting", true);
        animator.SetTrigger("StartShoot");

        // Esperamos a que Luna termine de levantar el brazo.
        preparationEndTime = Time.time + shootPreparationTime;
        nextFireTime = preparationEndTime;
    }

    private void StopShooting()
    {
        if (!isShooting)
            return;

        isShooting = false;

        animator.SetBool("IsShooting", false);
        animator.ResetTrigger("StartShoot");
    }

    private void UpdateFirePointPosition()
    {
        Vector3 newPosition = firePointRightPosition;

        // Colocamos FirePoint según la orientación de Luna.
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