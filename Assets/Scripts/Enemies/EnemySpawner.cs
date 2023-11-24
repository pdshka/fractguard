using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
{
    [SerializeField] private List<GameObject> armyPatterns = new List<GameObject>();

    [HideInInspector] public int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int currentWaveNumber = 0;
    private float wavesCooldown = 5f;
    private GameObject currentArmyPattern;

    private void Start()
    {
        LaunchNextWave();
    }

    // возвращает другой рандомный фрактал
    private GameObject ChooseRandomFractalPattern()
    {
        int index = Random.Range(0, armyPatterns.Count - 1);

        while (armyPatterns[index] == currentArmyPattern)
        {
            index = Random.Range(0, armyPatterns.Count - 1);
        }

        return armyPatterns[index];
    }

    private void LaunchNextWave()
    {
        currentWaveNumber++;
        Debug.Log(currentWaveNumber);
        if (currentWaveNumber % 5 == 1)
        {
            currentArmyPattern = ChooseRandomFractalPattern();
        }
        if (currentWaveNumber % 2 == 1)
        {
            /*foreach (SplineInstantiate.InstantiableItem enemy in currentArmyPattern.GetComponent<SplineInstantiate>().itemsToInstantiate)
            {
                enemy.Prefab.GetComponent<Enemy>()
            }*/
            
        }
        if (currentWaveNumber % 3 == 1)
        {
            // Увеличить территорию замка
        }
        //Instantiate(currentArmyPattern);
    }



    /// <summary>
    /// Create an enemy in the specified position
    /// </summary>
    private void CreateEnemy(GameObject enemyPrefab, Vector3 position)
    {

        // Instantiate enemy
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity, transform);

        // subscribe to enemy destroyed event
        enemy.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_OnDestroyed;

    }

    /// <summary>
    /// Process enemy destroyed
    /// </summary>
    private void Enemy_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        // Unsubscribe from event
        destroyedEvent.OnDestroyed -= Enemy_OnDestroyed;

        // reduce current enemy count
        currentEnemyCount--;

        if (currentEnemyCount <= 0)
        {
            StartCoroutine(LaunchNextWaveAfterCooldown());
        }
    }

    private IEnumerator LaunchNextWaveAfterCooldown()
    {
        yield return new WaitForSeconds(wavesCooldown);
        LaunchNextWave();
    }


}