using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
{
    [SerializeField] private List<ArmyPatternSO> armyPatterns = new List<ArmyPatternSO>();
    [SerializeField] private GameObject enemyPrefab;

    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;

    private void Start()
    {
        LaunchNextWave();
    }

    private ArmyPatternSO ChooseRandomFractalPattern()
    {
        int index = Random.Range(0, armyPatterns.Count - 1);

        return armyPatterns[index];
    }

    private void LaunchNextWave()
    {
        ArmyPatternSO armyPattern = ChooseRandomFractalPattern();
        foreach (var position in armyPattern.positions)
        {
            CreateEnemy(enemyPrefab, position);
        }
        // currentEnemyCount += ...
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
            LaunchNextWave();
        }
    }

}