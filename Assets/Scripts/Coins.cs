using UnityEngine;

public class Coins : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private float damage;
    [SerializeField] private float homingStrength = 2f; // Nueva propiedad

    private Quaternion baseRotation; // para guardar la rotacion de la moneica
    private Vector3 direction;
    private Transform target;
    private bool hasTarget = false;
    private float creationTime;

    private void Awake()
    {
        baseRotation = transform.rotation; // la rotacion del propio prefab
        creationTime = Time.time;
        Destroy(gameObject, lifetime);
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
        hasTarget = (target != null);

        if (hasTarget)
        {
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            direction = transform.forward;
        }
    }

    private void Update()
    {
        // Actualizar dirección con homing
        if (hasTarget && target != null && homingStrength > 0)
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;
            direction = Vector3.Slerp(direction, targetDirection, homingStrength * Time.deltaTime);
        }

        // Mover la moneda
        transform.position += direction * speed * Time.deltaTime;

        transform.rotation = baseRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignorar colisiones con el jugador y otras monedas
        if (other.CompareTag("Player") || other.GetComponent<Coins>() != null)
            return;

        // Evitar colisiones inmediatas después de crearse
        if (Time.time - creationTime < 0.1f)
            return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetDamage();

            Destroy(gameObject);
            return;
        }

        // Destruir si choca con paredes/obstáculos
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}