using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("AudioSource")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource seSource;

    private void Awake()
    {
        //シングルトーン設定
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //BGM再生
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.loop= true;
        bgmSource.Play();
    }

    //BGM停止
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    //SE再生
    public void PlaySE(AudioClip clip) 
    {
        if (clip == null) return;

        seSource.PlayOneShot(clip);
    }
}
