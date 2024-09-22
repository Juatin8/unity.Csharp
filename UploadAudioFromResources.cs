using UnityEngine;
using LeanCloud.Storage;
using System.IO;
using System.Threading.Tasks;
using System;

public class UploadAudioFromResources : MonoBehaviour
{
    async void Start()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("magic"); // 加载 Resources 文件夹中的音频

        if (audioClip != null)
        {
            await SaveAndUploadAudio(audioClip); // 将 AudioClip 转换为 WAV 文件并保存
        }
        else
        {
            Debug.LogError("音频文件未找到！");
        }
    }

    async Task SaveAndUploadAudio(AudioClip audioClip)
    {
        // 构建保存的文件路径
        string filePath = Path.Combine(Application.persistentDataPath, "sound.wav");

        SaveWavFile(filePath, audioClip);    // 将 AudioClip 转换为 WAV 并保存
        Debug.Log("文件已保存到: " + filePath);

        LCFile lcFile = new LCFile("sound.wav", filePath);   // 初始化 LeanCloud 文件对象时需要确保正确地传递字节数组

        // 上传文件到云端
        try
        {
            await lcFile.Save();
            Debug.Log("上传成功: " + lcFile.Url);
        }
        catch (Exception e)
        {
            Debug.LogError("上传失败: " + e.Message);
        }
    }



    // 保存 WAV 文件的辅助函数
    public static void SaveWavFile(string filePath, AudioClip clip)
    {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            byte[] wavData = ConvertAudioClipToWav(clip);
            fileStream.Write(wavData, 0, wavData.Length);
        }
    }


    public static byte[] ConvertAudioClipToWav(AudioClip clip)    // 将 AudioClip 转换为 WAV 格式
    {
        // 获取音频数据
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        // 创建 WAV 文件头和数据
        int sampleCount = samples.Length;
        int byteRate = Mathf.FloorToInt(clip.frequency * clip.channels * 16 / 8);

        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // 写入 WAV 文件头
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + sampleCount * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);  // Subchunk1Size
                writer.Write((short)1); // AudioFormat (PCM)
                writer.Write((short)clip.channels); // NumChannels
                writer.Write(clip.frequency); // SampleRate
                writer.Write(byteRate); // ByteRate
                writer.Write((short)(clip.channels * 16 / 8)); // BlockAlign
                writer.Write((short)16); // BitsPerSample

                // 写入数据块
                writer.Write("data".ToCharArray());
                writer.Write(sampleCount * 2);

                // 将浮点音频数据转换为 16 位 PCM 格式并写入
                foreach (float sample in samples)
                {
                    short intSample = (short)(sample * 32767);
                    writer.Write(intSample);
                }

                writer.Flush();
            }

            return stream.ToArray();
        }
    }
}