using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelSelection : MonoBehaviour
{
    [SerializeField] private UI_LevelButton buttonPrefab;
    [SerializeField] private Transform buttonParrent;


    private void Start()
    {
        CreateLevelButton();
    }
    private void CreateLevelButton()
    {
        int levelsAmount = SceneManager.sceneCountInBuildSettings - 1;

        for (int i = 1; i < levelsAmount; i++)
        {
            UI_LevelButton newButton = Instantiate(buttonPrefab, buttonParrent);
            newButton.SetupButton(i);
        }
    }
}
