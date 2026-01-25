using UnityEngine;

public class Coins : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float homingStrength = 2f; // Nueva propiedad
    
    [Header("Efectos")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private AudioClip hitSound;
    
    private Vector3 direction;
    private Transform target;
    private bool hasTarget = false;
    private float creationTime;

    private void Awake()
    {
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
        
        // Rotar la moneda para efecto visual
        transform.Rotate(Vector3.up, 500 * Time.deltaTime);
        
        // Rotar para que mire en dirección de movimiento
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction) * 
                                Quaternion.Euler(90, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignorar colisiones con el jugador y otras monedas
        if (other.CompareTag("Player") || other.GetComponent<Coins>() != null)
            return;
        
        // Evitar colisiones inmediatas después de crearse
        if (Time.time - creationTime < 0.1f)
            return;
        
        // Si golpea un enemigo
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetDamage();
            
            // Efecto de golpe
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                
            if (hitSound != null)
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
                
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