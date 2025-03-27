using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIManager : MonoBehaviour
{
    public static GamePlayUIManager Instance;

    [Header("Player Vars")]
    [SerializeField] private Image healthBarSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI killsText;

    [Header("Gun Info")]
    [SerializeField] private TextMeshProUGUI remainingBulletText;
    [SerializeField] private TextMeshProUGUI totalBulletText;

    [Header("CountDown")]
    [SerializeField] private TextMeshProUGUI countDownText;

    [Header("Wave Info")]
    [SerializeField] private Image waveProgressBar;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Game pause UI")]
    [SerializeField] private GameObject pasueUI;


    /// <summary>
    /// temp vars
    /// </summary>
    /// 
    private int totalKill = 0;

    private void Awake()
    {
        Instance = this;

        
    }

    private void Start()
    {
        EnemyManager.Instance.OnEnemyCountChangedEvent += UpdateWaveInfo;
        EnemyManager.Instance.OnCountdownChangedEvent += UpdateCoundownText;
        EnemyManager.Instance.OnWavesFinishedAction += Instance_OnWavesFinishedAction;

        PlayerController.Instance.OnEnemyKilledAction += UpdateKillsAndMoney;

        GunSystem.Instance.OnBulletsChangedAction += UpdateBulletInfo;
    }

    private void OnDisable()
    {
        EnemyManager.Instance.OnEnemyCountChangedEvent -= UpdateWaveInfo;
        EnemyManager.Instance.OnCountdownChangedEvent -= UpdateCoundownText;
        EnemyManager.Instance.OnWavesFinishedAction -= Instance_OnWavesFinishedAction;

        PlayerController.Instance.OnEnemyKilledAction -= UpdateKillsAndMoney;

        GunSystem.Instance.OnBulletsChangedAction -= UpdateBulletInfo;
    }

    //public void UpdateKillsAndMoney(int totalkilledEnemy, int money)
    //{
    //    killsText.text = totalkilledEnemy.ToString();
    //    moneyText.text = money.ToString();
    //}

    private void UpdateKillsAndMoney(object sender, PlayerController.OnEnemyKilledActionArgs e)
    {
        killsText.text = e.totalKills.ToString();
        moneyText.text = string.Format("${0}", (e.totalKills * 12));
    }

    private void UpdateWaveInfo(object sender, EnemyManager.OnEnemyCountChangedArgs e)
    {
        waveProgressBar.fillAmount = e.waveProgress;
        waveText.text = string.Format("Wave {0}", e.waveIndex);
    }

    private void UpdateCoundownText(object sender, EnemyManager.OnCoundownChangedArgs e)
    {
        countDownText.gameObject.SetActive(e.showText);

        countDownText.text = Mathf.FloorToInt(e.countDownTime % 60).ToString();
    }

    private void UpdateBulletInfo(object sender, GunSystem.OnBulletsChangedArgs e)
    {
        remainingBulletText.text = e.bulletsInMagazine.ToString();
        //totalBulletText.text = totalAmmo.ToString();
        totalBulletText.text = "/\u221E";
    }

    private void Instance_OnWavesFinishedAction(object sender, EventArgs e)
    {
        pasueUI.SetActive(true);
    }

    public void UpdateHealthBar(float remainingHealth)
    {
        healthBarSlider.fillAmount = remainingHealth;
        healthText.text = string.Format("%{0}", (remainingHealth * 100));
    }
}
