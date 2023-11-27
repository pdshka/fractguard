using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
{
    [SerializeField] private List<GameObject> armyPatterns = new List<GameObject>();

    [SerializeField] private BaseGenerator baseGeneratorReference;
    [SerializeField] private TextMeshProUGUI wavesText;
    [HideInInspector] public int currentEnemyCount;
    [HideInInspector] public int enemiesSpawnedSoFar;
    [SerializeField] private int armyPatternChangeFrequency = 3;
    [SerializeField] private int enemyBoostFrequency = 2;
    [SerializeField] private int castleExpandFrequency = 5;
    [SerializeField] private int wavesCooldown = 30;
    private int wavesCooldownTimer;

    private int currentWaveNumber = 0;
    
    private GameObject currentArmyPattern;

    private void Start()
    {
        currentArmyPattern = ChooseRandomFractalPattern();
        StartCoroutine(LaunchNextWaveAfterCooldown());
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
        wavesText.text = "Волна " + currentWaveNumber;
        //Debug.Log(currentWaveNumber);
        if (currentWaveNumber % armyPatternChangeFrequency == 1 && currentWaveNumber > 1)
        {
            currentArmyPattern = ChooseRandomFractalPattern();
        }

        if (currentWaveNumber % enemyBoostFrequency == 1 && currentWaveNumber > 1)
        {
            /*foreach (SplineInstantiate.InstantiableItem enemy in currentArmyPattern.GetComponent<SplineInstantiate>().itemsToInstantiate)
            {
                enemy.Prefab.GetComponent<Enemy>()
            }*/
            
        }
        if (currentWaveNumber % castleExpandFrequency == 1 && currentWaveNumber > 1)
        {
            currentArmyPattern.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            var splineInst = currentArmyPattern.GetComponent<SplineInstantiate>();
            splineInst.MinSpacing = splineInst.MaxSpacing -= 0.02f;
            if (currentArmyPattern.name == "WolfHead")
            {
                currentArmyPattern.transform.position += new Vector3(0f, 1f);
            }
            baseGeneratorReference.RunProceduralGeneration();
        }
        Instantiate(currentArmyPattern);
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

    public void SubscribeOnDestroyedEvent(DestroyedEvent destroyedEvent)
    {
        destroyedEvent.OnDestroyed += Enemy_OnDestroyed;
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

        Debug.Log(currentEnemyCount);

        if (currentEnemyCount <= 0)
        {
            StartCoroutine(LaunchNextWaveAfterCooldown());
        }
    }

    private void UpdateWaveTimerText()
    {
        wavesText.text = string.Format("00:{0:d2}\nдо волны {1:d}", wavesCooldownTimer, currentWaveNumber + 1);
    }

    private IEnumerator LaunchNextWaveAfterCooldown()
    {
        wavesCooldownTimer = wavesCooldown;
        
        while (wavesCooldownTimer > 0)
        {
            UpdateWaveTimerText();
            yield return new WaitForSeconds(1f);
            wavesCooldownTimer--;
        }

        LaunchNextWave();
    }


}