using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    [Header("Level managment")]
    [SerializeField] private int currentLevelIndex;

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
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        CollectFruitsInfo();
    }

    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint) => respawnPoint = newRespawnPoint;
        
    public void RespawnPlayer() => StartCoroutine(RespawCourutine());
    private IEnumerator RespawCourutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        player = newPlayer.GetComponent<Player>();
    }

    public void AddFruit() => fruitsCollected++;
    public bool FruitHaveRandomLook() => fruitsAreRandom;

    private void LoadTheEndScene() => UnityEngine.SceneManagement.SceneManager.LoadScene("TheEnd");
    private void loadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        SceneManager.LoadScene("Level_" + nextLevelIndex);
    }
    public void LevelFinished()
    {
        UI_FadeEffect fadeEffect = UI_InGame.instance.fadeEffect;

        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2;

        bool noMoreLevels = currentLevelIndex == lastLevelIndex;

        if (!noMoreLevels)
            UI_InGame.instance.fadeEffect.ScreenFade(1, 1.5f, loadNextLevel);
        else

            UI_InGame.instance.fadeEffect.ScreenFade(1, 1.5f, LoadTheEndScene);
    }
}
