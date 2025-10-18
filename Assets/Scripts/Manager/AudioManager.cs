using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource[] sfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(int sfxToPlay, bool boolPicth = true)
    {
        if (sfxToPlay >= sfx.Length)
            return;
        if(boolPicth)
            sfx[sfxToPlay].pitch = Random.Range(0.9f, 1.1f);

        sfx[sfxToPlay].Play();

    }
    public void StopSFX(int sfxToStop) => sfx[sfxToStop].Stop();

}
