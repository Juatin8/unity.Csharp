using UnityEngine;

public static class AudioManager
{
    private static AudioSource audioSource;
    public static void PlaySound(AudioClip clip)
    {
        if(audioSource == null)   // 如果没有音频源，就创建一个
        {
            GameObject audioObject = new GameObject("AudioSource");
            audioSource = audioObject.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(clip);  // 播放一次音效
    }

    public static void PlayMusic(AudioClip clip, bool loop = true)
    { 
        if(audioSource == null)  // 如果没有音频源，就创建一个
        {
            GameObject audioObject = new GameObject("AudioSource");
            audioSource = audioObject.AddComponent<AudioSource>();
        }
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();  // 播放音乐
    }

    public static void StopSound()   // 停止音效
    {
        if(audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private static float volume = 1.0f;  // 默认音量为1.0
    public static void SetVolume(float value)    //调节音量
    {
        volume = Mathf.Clamp01(value);  // 确保音量值在0到1之间
        if(audioSource != null)
        {
            audioSource.volume = volume;  // 设置音量
        }
    }
}