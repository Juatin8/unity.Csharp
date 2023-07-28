using UnityEngine;

public static class AudioManager
{
    private static AudioSource audioSource;
    public static void PlaySound(AudioClip clip)
    {
        if(audioSource == null)   // ���û����ƵԴ���ʹ���һ��
        {
            GameObject audioObject = new GameObject("AudioSource");
            audioSource = audioObject.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(clip);  // ����һ����Ч
    }

    public static void PlayMusic(AudioClip clip, bool loop = true)
    { 
        if(audioSource == null)  // ���û����ƵԴ���ʹ���һ��
        {
            GameObject audioObject = new GameObject("AudioSource");
            audioSource = audioObject.AddComponent<AudioSource>();
        }
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();  // ��������
    }

    public static void StopSound()   // ֹͣ��Ч
    {
        if(audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private static float volume = 1.0f;  // Ĭ������Ϊ1.0
    public static void SetVolume(float value)    //��������
    {
        volume = Mathf.Clamp01(value);  // ȷ������ֵ��0��1֮��
        if(audioSource != null)
        {
            audioSource.volume = volume;  // ��������
        }
    }
}