using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;
    public UI_FadeEffect fadeEffect { get; private set; }
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI fruitText;

    [SerializeField] private GameObject pauseUI;
    private bool isPaused;

    private void Awake()
    {
        instance = this;
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0, 1.5f);
    }

    public void UpdateFruitUI(int collectedFruits, int totalFruits)
    {
        fruitText.text = collectedFruits + "/" + totalFruits;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseButton();
        }
    }

    public void PauseButton()
    {
        if(isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseUI.SetActive(true);
        }
    }

    public void GoToMainMenuButtom()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateTimerUI(float timer)
    {
        timerText.text = timer.ToString("00" + " s");
    }
}
