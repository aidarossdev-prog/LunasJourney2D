using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 3f;

    private Vector2 direction = Vector2.right;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;

        // Invierte visualmente el proyectil cuando viaja a la izquierda
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else
        {
            transform.localScale = new Vector3(
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    private void Start()
    {
        // Evita que los proyectiles permanezcan indefinidamente en la escena
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)(direction * speed * Time.fixedDeltaTime);
    }
}