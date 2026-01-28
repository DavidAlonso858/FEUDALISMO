using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent; // ya se encarga de la velocidad y demas
    private Transform player;
    private Animator animator;

    [Header("Health")]
    [SerializeField] private float maxHealth;

    [SerializeField] private AudioClip clipDeath;
    private AudioSource audioS;

    private float currentHealth;
    public bool isAlive => currentHealth > 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // para que el enemigo siga al jugador/player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioS = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentHealth <= 0) return;

        agent.SetDestination(player.position);

        // variable del animator para que cambie la animacion dependiendo de la velocidad
        animator.SetFloat("velocity", agent.velocity.magnitude);
    }

    public void GetDamage()
    {
        // evita doble damage a un enemigo muerto ya
        if (currentHealth <= 0) return;

        currentHealth--;

        if (currentHealth <= 0)
        {
            // MUERTE
            agent.enabled = false;
            animator.Play("Mini Simple Characters Armature|Death");
            GetComponent<Collider>().enabled = false;
            audioS.clip = clipDeath;
            StartSinking();
        }
       
        audioS.Play();
    }

    public void StartSinking()
    {
        StartCoroutine(Sinking());
    }

    IEnumerator Sinking()
    {
        // lo puedo llamar porque es static, sin tener que serializarlo etc
        // GameManager.instance.EnemyDeath();
        // para que se ejecute despues de 1 segundo en la animacion de muerte
        yield return new WaitForSeconds(1f);
        while (transform.position.y > -2.5f)
        {
            transform.Translate(Vector3.down * 0.1f); // velocidad a la que se hunde
            yield return new WaitForSeconds(0.04f); // suavidad del hundimiento
        }
        Destroy(gameObject);
    }

}