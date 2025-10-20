using UnityEngine;
using UnityEditor.SearchService;

public class SkinManager : MonoBehaviour
{   

    public int choosenSkinId;
    public static SkinManager instance;
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void setSkinId(int id) => choosenSkinId = id;
    public int getSkinId() => choosenSkinId;

}
