using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnTime;

  
    public void StartSpawn(int spawnLimit)
    {
        StartCoroutine(Spawn(spawnLimit));
    }

    IEnumerator Spawn(int spawnLimit)
    {
        Debug.Log("Spawneando enemigo");
        int spawnCount = 0;

        while (spawnCount < spawnLimit)
        {
            // creo el prefab del esqueletillo con la posicion y rotacion del spawn
            Instantiate(enemy, transform.position, transform.rotation);
            spawnCount++;
            // agrego el tiempo entre esqueletillos en el spawn
            yield return new WaitForSeconds(spawnTime);
        }
    }

}