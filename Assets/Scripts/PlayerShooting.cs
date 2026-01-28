using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Camera cam;
    [Header("Configuración")]
    [SerializeField] private LayerMask layerGround, layerShoot;
    [SerializeField] private Transform[] shootPoints;
    [SerializeField] private GameObject coinPrefab; // Prefab de la moneica
    [SerializeField] private AudioClip clipCoin;

    [Header("ConfiguraciónDisparo")]
    [SerializeField] private float cadency = 0.5f;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool multiShot = true; // Disparar desde todos los puntos

    [Header("Efectos")]
    [SerializeField] private float lightDuration = 0.1f;
    private AudioSource audioS;
    private Light[] lightShoot; // Luz que brilla al disparar
    private bool canShoot = true;
    private List<Transform> currentTargets = new List<Transform>();
    private float shootTimer = 0f;
    private List<Enemy> enemiesInRange = new List<Enemy>();

    private void Awake()
    {

        cam = Camera.main;
        lightShoot = new Light[shootPoints.Length];
        for (int i = 0; i < shootPoints.Length; i++)
        {
            if (shootPoints[i] != null)
                lightShoot[i] = shootPoints[i].GetComponent<Light>();
        }
        audioS = GetComponent<AudioSource>();

    }
    void Update()
    {
        // Actualizar temporizador
        if (!canShoot)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= cadency)
            {
                canShoot = true;
                shootTimer = 0f;
            }
        }

        // Buscar enemigos en rango
        FindEnemiesInRange();

        // Rotar hacia el enemigo más cercano o el mouse
        RotatePlayer();

        if (canShoot && enemiesInRange.Count > 0)
        {
            ShootFromAllPoints();
        }
    }

    void FindEnemiesInRange()
    {
        enemiesInRange.Clear();
        currentTargets.Clear();

        // Buscar todos los enemigos en el rango
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, layerShoot);

        foreach (Collider col in hits)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy) && enemy.isAlive)
            {
                enemiesInRange.Add(enemy);
            }
        }

        // Ordenar enemigos por distancia
        enemiesInRange.Sort((a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        // Asignar targets a cada punto de disparo
        AssignTargetsToShootPoints();
    }

    void AssignTargetsToShootPoints()
    {
        currentTargets.Clear();

        if (enemiesInRange.Count == 0 || shootPoints.Length == 0)
            return;

        // Opción 1: Cada punto dispara al enemigo más cercano a él
        if (multiShot)
        {
            for (int i = 0; i < shootPoints.Length; i++)
            {
                if (i < enemiesInRange.Count)
                {
                    // Asignar enemigo a este punto de disparo
                    currentTargets.Add(enemiesInRange[i].transform);
                }
                else if (enemiesInRange.Count > 0)
                {
                    // Si hay más puntos que enemigos, algunos puntos disparan al mismo
                    currentTargets.Add(enemiesInRange[0].transform);
                }
                else
                {
                    currentTargets.Add(null);
                }
            }
        }
        // Opción 2: Todos disparan al mismo enemigo (el más cercano al jugador)
        else
        {
            Transform closestEnemy = enemiesInRange.Count > 0 ? enemiesInRange[0].transform : null;
            for (int i = 0; i < shootPoints.Length; i++)
            {
                currentTargets.Add(closestEnemy);
            }
        }
    }

    void RotatePlayer()
    {
        if (cam != null)
        {
            // Rotar hacia el mouse
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, layerGround))
            {
                Vector3 lookPos = hit.point - transform.position;
                lookPos.y = 0;

                if (lookPos != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                        rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void ShootFromAllPoints()
    {
        if (!canShoot || shootPoints.Length == 0)
            return;

        // Iniciar cooldown
        canShoot = false;
        shootTimer = 0f;

        // Sonido (uno solo)
        if (audioS != null && clipCoin != null)
        {
            audioS.PlayOneShot(clipCoin);
        }

        // Disparar desde cada punto
        for (int i = 0; i < shootPoints.Length; i++)
        {
            if (shootPoints[i] != null && currentTargets.Count > i && currentTargets[i] != null)
            {
                StartCoroutine(ShootFromPoint(i));
            }
            else if (shootPoints[i] != null && enemiesInRange.Count > 0)
            {
                // Disparar al enemigo más cercano si no hay target asignado
                StartCoroutine(ShootFromPoint(i, enemiesInRange[0].transform));
            }
        }

        Debug.Log($"Disparando desde {shootPoints.Length} puntos. Enemigos: {enemiesInRange.Count}");
    }

    IEnumerator ShootFromPoint(int pointIndex, Transform specificTarget = null)
    {
        Transform shootPoint = shootPoints[pointIndex];
        Transform target = specificTarget != null ? specificTarget :
                          (currentTargets.Count > pointIndex ? currentTargets[pointIndex] : null);

        if (shootPoint == null || target == null || coinPrefab == null)
            yield break;

        // Efecto de luz en este punto
        if (lightShoot.Length > pointIndex && lightShoot[pointIndex] != null)
        {
            StartCoroutine(FlashLight(pointIndex));
        }

        // Pequeño delay entre disparos para efecto estético
        yield return new WaitForSeconds(pointIndex * 0.05f);

        // Crear la moneda
        GameObject coin = Instantiate(coinPrefab, shootPoint.position, shootPoint.rotation);

        // Orientar la moneda hacia el enemigo
        Vector3 direction = (target.position - shootPoint.position).normalized;
        coin.transform.rotation = Quaternion.LookRotation(direction);

        // Configurar la moneda
        Coins coinScript = coin.GetComponent<Coins>();
        if (coinScript == null)
            coinScript = coin.AddComponent<Coins>();

        // Pasar el target a la moneda (para homing opcional)
        coinScript.SetTarget(target);

        // DEBUG: Visualizar disparo
        Debug.DrawLine(shootPoint.position, target.position, Color.green, 0.5f);
    }

    IEnumerator FlashLight(int lightIndex)
    {
        if (lightIndex < lightShoot.Length && lightShoot[lightIndex] != null)
        {
            // Obtener el componente Light correctamente
            Light lightComponent = lightShoot[lightIndex];
            lightComponent.enabled = true;
            yield return new WaitForSeconds(lightDuration);
            lightComponent.enabled = false;
        }
    }

    // Visualizar el rango de detección y puntos de disparo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Dibujar puntos de disparo
        if (shootPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform point in shootPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.1f);
                    Gizmos.DrawRay(point.position, point.forward * 2);
                }
            }
        }
    }
}