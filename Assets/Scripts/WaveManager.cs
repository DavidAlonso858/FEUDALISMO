using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Spawner[] spawners;
    private int currentWave = 0;
    [SerializeField] private int totalWaves = 4;
    [SerializeField] private TMP_Text enemiesText;

    // cambio de enemigo por ronda (valido al ser pocas rondas)
    [SerializeField] private GameObject enemyWave1;
    [SerializeField] private GameObject enemyWave2;
    [SerializeField] private GameObject enemyFinalWave;

    [SerializeField] private UpgradePanel upgradePanel;
    [SerializeField] private GamePanelVictory gamePanelVictory;

    private int currentEnemies;
    private int maxEnemiesWave;

    public static WaveManager instance;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartWave();
    }

    private void UpdateEnemiesText()
    {
        enemiesText.text = currentEnemies + " / " + maxEnemiesWave;
    }
    public void EnemyDeath()
    {
        currentEnemies--;
        UpdateEnemiesText();

        if (currentEnemies <= 0)
        {
            Debug.Log("Oleada terminada. Oleada actual: " + currentWave);

            if (currentWave >= totalWaves)
            {
                Debug.Log("Todas las oleadas completadas");
                gamePanelVictory.Show();
                return; // no sigo al ganar el juego
            }


            // Mostrar panel de upgrades antes de la siguiente oleada
            if (upgradePanel != null) // Asumiendo que hay 3-4 oleadas
            {
                Debug.Log("Mostrando panel de upgrades");
                upgradePanel.Show();
            }
            else
            {
                Debug.LogError("¡upgradePanel es NULL! Asigna el panel en el Inspector");
                // Si no hay panel, continuar directamente
                StartWave();
            }
        }

    }

    public void StartWave()
    {
        if (currentWave >= totalWaves) return;

        Debug.Log("StartWave llamado. Oleada: " + currentWave);
        int min = 1, max = 2;
        GameObject enemyToSpawn = enemyWave1;
        if (currentWave == 1)
        {
            min = 3; max = 5;
        }

        if (currentWave == 2)
        {
            min = 6; max = 6;
            enemyToSpawn = enemyWave2;
        }
        if (currentWave == 3)
        {
            min = 1; max = 1; // 5 minibosses que pongo en el inspector de Unity
            enemyToSpawn = enemyFinalWave;
        }

        // activo los spawns con el rango aleatorio segun lo asignado en las oleadas
        // +1 para que tenga en cuenta el max tb dentro del rango
        int activeSpawns = Random.Range(min, max + 1);

        List<int> usedIndexes = new List<int>();
        currentEnemies = 0;
        maxEnemiesWave = 0;
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

            int enemiesPerSpawner = 5;
            spawners[indexSpawn].SetEnemy(enemyToSpawn);
            spawners[indexSpawn].StartSpawn(enemiesPerSpawner);

            currentEnemies += enemiesPerSpawner;
            maxEnemiesWave += enemiesPerSpawner;

        }

        UpdateEnemiesText();
        currentWave++;
    }
}
