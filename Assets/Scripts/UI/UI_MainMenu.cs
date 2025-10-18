using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{

    private UI_FadeEffect fadeEffect;
    public string firstLevelName;

    [SerializeField] private GameObject[] uiElenments;

    [SerializeField] private GameObject continueButton;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        if(HasLevelProgression())
            continueButton.SetActive(true);

        fadeEffect.ScreenFade(0, 1.5f);
    }

    public void SwicthUI(GameObject uiToEnable)
    {
        foreach (GameObject ui in uiElenments)
        {
            ui.SetActive(false);
        }

        uiToEnable.SetActive(true);
        AudioManager.instance.PlaySFX(4);
    }

    public void NewGame()
    {
        fadeEffect.ScreenFade(1, 1.5f, LoadLevelScene);
        AudioManager.instance.PlaySFX(4);

    }

    private void LoadLevelScene() => SceneManager.LoadScene(firstLevelName);

    private bool HasLevelProgression()
    {
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;
        return hasLevelProgression;
    }

    public void ContinueGame()
    {
        int difficultyIndex = PlayerPrefs.GetInt("GameDifficulty", 1);
        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0);

        DifficultyManager.instance.LoadDifficulty(difficultyIndex);

        SceneManager.LoadScene("Level_" + levelToLoad);
        AudioManager.instance.PlaySFX(4);

    }
}
