using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button survivalButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        survivalButton.onClick.AddListener(OnSurvivalButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    private void OnQuitButtonPressed()
    {
        UIManager.Instance.QuitGame();
    }

    private void OnSurvivalButtonPressed()
    {
        UIManager.Instance.LoadScene(1);
    }
}
