using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    [SerializeField] private int bgmIndex;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if(bgm.Length <= 0)
            return;
        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
    }


    public void PlayMusicIfNeeded()
    {
        if(bgm[bgmIndex].isPlaying == false)
            PlayRandomBGM();
    }
    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlayBGM(int bgmToPlay)
    {
        if(bgm.Length <=0)
            return;
            Debug.Log("Khong co nhac sao choi be oi!!");

        for (int i = 0; i < bgm.Length; i++)
            bgm[i].Stop();


        bgmIndex = bgmToPlay;
        bgm[bgmToPlay].Play();

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
