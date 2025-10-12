using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Credits : MonoBehaviour
{

    //private UI_FadeEffect fadeEffect;
    [SerializeField] private RectTransform rect;
    [SerializeField] private float scrollSpeed = 200;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private float offScreenPosition = 1800;


    private bool creditsSkipped;


    //private void Awake()
    //{
    //    fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    //    fadeEffect.ScreenFade(0, 1);
    //}

    private void Update()
    {
        rect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        if(rect.anchoredPosition.y >= offScreenPosition)
        {
            GoToMainMenu();
        }
    }

    public void SkipCredits()
    {
        if (creditsSkipped == false)
        {
            scrollSpeed *= 10;
            creditsSkipped = true;
        }
        else
        {
            GoToMainMenu();
        }
    }

    //private void GoToMainMenu() => fadeEffect.ScreenFade(1, 1, SwitchToMenuScene);
    private void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}