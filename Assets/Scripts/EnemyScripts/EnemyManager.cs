using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    public WaveScriptableObject[] waves;

    public event EventHandler<OnEnemyCountChangedArgs> OnEnemyCountChangedEvent;
    public class OnEnemyCountChangedArgs: EventArgs
    {
        public float waveProgress;
        public int waveIndex;
    }

    public event EventHandler<OnCoundownChangedArgs> OnCountdownChangedEvent;
    public class OnCoundownChangedArgs : EventArgs
    {
        public float countDownTime; 
        public bool showText;
    }

    public event EventHandler OnWavesFinishedAction;
    
    /// <summary>
    /// Wave Info Vars
    /// </summary>
    private WaveScriptableObject selectedWave;

    private int selectedWaveIndex;
    private float remainingTime;
    private float enemyCount;
    private int enemySpawnedCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        selectedWaveIndex = 0;

        SetWave(waves[selectedWaveIndex]);

        StartCoroutine(Countdown());
    }

    private void SetWave(WaveScriptableObject wave)
    {

        selectedWave = wave;

        enemyCount = selectedWave.enemyCount;

        OnEnemyCountChangedEvent?.Invoke(this, new OnEnemyCountChangedArgs
        {
            waveIndex = selectedWave.waveIndex,
            waveProgress = 0
        });
    }

    private IEnumerator Countdown()
    {
        
        remainingTime = selectedWave.waveWaitTime;

        while (remainingTime > 0)
        {
            OnCountdownChangedEvent?.Invoke(this, new OnCoundownChangedArgs { 
                showText = true,
                countDownTime = remainingTime
            });

            yield return new WaitForSeconds(1f);
            remainingTime --;
            Debug.Log(remainingTime);
        }

        OnCountdownChangedEvent?.Invoke(this, new OnCoundownChangedArgs
        {
            showText = false,
            countDownTime = 0
        });

        StartCoroutine(SpawnEnemy());
    }

    public void CheckEnemyCount()
    {
        enemyCount--;



        OnEnemyCountChangedEvent?.Invoke(this, new OnEnemyCountChangedArgs { 
            waveIndex = selectedWave.waveIndex,
            waveProgress = (selectedWave.enemyCount - enemyCount) / selectedWave.enemyCount
        });

        if (enemyCount <= 0)
        {
            Debug.Log("Wave finished");

            selectedWaveIndex++;

            if (selectedWaveIndex < waves.Length)
            {
                SetWave(waves[selectedWaveIndex]);

                StartCoroutine(Countdown());
            }

            else
            {
                Time.timeScale = 0;
                OnWavesFinishedAction?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private IEnumerator SpawnEnemy()
    {
        enemyCount = selectedWave.enemyCount;
        enemySpawnedCount = 0;

        while (enemySpawnedCount < selectedWave.enemyCount)
        {
            Enemy enemy = Instantiate(selectedWave.enemyTypePrefab, this.transform);
            Vector3 position = Vector3.zero;

            if (Random.Range(0f, 2f) % 2 == 0)
            {
                position.x = 25;
                position.y = 0;
                position.z = Random.Range(-25, 25);
            }

            else
            {
                position.x = -25;
                position.y = 0;
                position.z = Random.Range(-25, 25);
            }
            enemy.transform.position = position;
            enemySpawnedCount++;

            yield return new WaitForSeconds(2f);
        }
    }
}
