using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Spawner[] spawners;
    private int currentWave = 0;
    void Start()
    {
        spawners[0].StartSpawn(3);
    }
    public void StartWave()
    {
        int min = 1, max = 3;

        if (currentWave == 1)
        {
            min = 2; max = 5;
        }

        if (currentWave == 3)
        {
            min = 6; max = 6;
        }
        if (currentWave == 4)
        {
            min = 6; max = 6;
        }

        // activo los spawns con el rango aleatorio segun lo asignado en las oleadas
        // +1 para que tenga en cuenta el max tb dentro del rango
        int activeSpawns = Random.Range(min, max + 1);

        List<int> usedIndexes = new List<int>();

        for (int i = 0; i < activeSpawns; i++)
        {
            int indexSpawn;
            do
            {
                indexSpawn = Random.Range(0, spawners.Length);
                // bloquea un spawner usado para que salga en otro nuevo
            } while (usedIndexes.Contains(indexSpawn));

            // lo agrego para que no vuelvan a salir del mismo
            usedIndexes.Add(indexSpawn);
            spawners[indexSpawn].StartSpawn(5);

        }

        currentWave++;
    }

}
