using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnTime;

    public void SetEnemy(GameObject newEnemy)
    {
        enemy = newEnemy;
    }
    public void StartSpawn(int spawnLimit)
    {
        StartCoroutine(Spawn(spawnLimit));
    }

    IEnumerator Spawn(int spawnLimit)
    {
        int spawnCount = 0;
        Debug.Log("Spawneando enemigo" + spawnCount);

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