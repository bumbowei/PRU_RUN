using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    private UI_InGame inGameUI;

    [Header("Level managment")]
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private float levelTimer;
    private int nextLevelIndex;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay;
    public Player player;

    [Header("Fruit managerment")]
    public bool fruitsAreRandom;
    public int fruitsCollected;
    public int totalFruits;

    [Header("Checkpoints")]
    public bool canReactivate;

    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;

    //public int totalFruitsCollected;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        inGameUI = UI_InGame.instance;

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        if (respawnPoint == null) respawnPoint = FindFirstObjectByType<StartPoint>().transform;

        if(player == null) player = FindFirstObjectByType<Player>();
        nextLevelIndex = currentLevelIndex + 1;

        CollectFruitsInfo();
        CreateManagerIfNeeded();

    }

    private void Update()
    {
        levelTimer += Time.deltaTime;

        inGameUI.UpdateTimerUI(levelTimer);
    }

    private void CreateManagerIfNeeded()
    {
        if(AudioManager.instance == null)
            Instantiate(audioManager);
    }

    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;

        inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);

        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);

    }

    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
        
    public void RespawnPlayer()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;

        if (difficultyManager != null && difficultyManager.difficulty == DifficultyType.Hard)
            return;



        StartCoroutine(RespawCourutine());
    }
    private IEnumerator RespawCourutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }

    public void AddFruit()
    {
        fruitsCollected++;
        inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);
    }
    public void RemoveFruit()
    {
        fruitsCollected--;
        inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);
    }
    public int FruitsCollected() => fruitsCollected;

    public bool FruitHaveRandomLook() => fruitsAreRandom;

    public void LevelFinished()
    {
        LevelProgression();
        SaveBestTime();
        SaveFruitsInfo();

        LoadNextScene();
    }

    private void SaveFruitsInfo()
    {
        int fruisCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruitsCollected");

        if (fruitsCollected > fruisCollectedBefore)
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitsCollected", fruitsCollected);

        int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");
        PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsInBank + fruitsCollected);
    }

    private void SaveBestTime()
    {
        float lastTime = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTime",99);

        if (levelTimer < lastTime)
            PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTime", levelTimer);
    }

    private void LevelProgression() //fix this method to save level progression
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);

        if (NoMoreLevels() == false)
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
            SkinManager skinManager = SkinManager.instance;
            if(skinManager != null)
                PlayerPrefs.SetInt("LastUseSkin", skinManager.getSkinId());

    }

    public void RestartLevel()
    {
        UI_InGame.instance.fadeEffect.ScreenFade(1, .75f, LoadCurrentScene);
    }
    private void LoadCurrentScene() => SceneManager.LoadScene("Level_" + currentLevelIndex);
    private void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");
    private void loadNextLevel()
    {
        SceneManager.LoadScene("Level_" + nextLevelIndex);
    }

    private void LoadNextScene()
    {
        UI_FadeEffect fadeEffect = inGameUI.fadeEffect;



        if (!NoMoreLevels())
            inGameUI.fadeEffect.ScreenFade(1, 1.5f, loadNextLevel);
        else

            inGameUI.fadeEffect.ScreenFade(1, 1.5f, LoadTheEndScene);
    }

    private bool NoMoreLevels()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2;

        bool noMoreLevels = currentLevelIndex == lastLevelIndex;

        return noMoreLevels;
    }
}
