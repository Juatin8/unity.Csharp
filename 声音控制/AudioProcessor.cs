
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAudio : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource; //音频组件

    private AudioClip[] audioclips;
    private AudioClip clip;         //音频文件

    audioclips = new AudioClip[6];      
    ar = new List<float>();             //数组初始化
        
    //----------------------------合成加工 a+b+c 的逻辑 ------------------------------------------------
    public void CombineClips()    //加载音频资源
    {
        audioclips[0] = (AudioClip)Resources.Load("hello");
        audioclips[1] = (AudioClip)Resources.Load("animals/" + ang.CallAnimalName);
        audioclips[2] = (AudioClip)Resources.Load("friend");
        audioclips[3] = (AudioClip)Resources.Load("animals/" + ang.LostAnimalName);
        audioclips[4] = (AudioClip)Resources.Load("find");

        float[] data0 = new float[audioclips[0].samples * audioclips[0].channels];
        float[] data1 = new float[audioclips[1].samples * audioclips[1].channels];
        float[] data2 = new float[audioclips[2].samples * audioclips[2].channels];
        float[] data3 = new float[audioclips[3].samples * audioclips[3].channels];
        float[] data4 = new float[audioclips[4].samples * audioclips[4].channels];  //总的数据=样本中音频剪辑的长度*通道数量
        audioclips[0].GetData(data0, 0);
        audioclips[1].GetData(data1, 0);
        audioclips[2].GetData(data2, 0);
        audioclips[3].GetData(data3, 0);
        audioclips[4].GetData(data4, 0);                 //GetData 使用剪辑中的样本数据填充数组

        ar.Clear();                 //这是个坑，要清除它原来的内容，否则内容会不断累加
        ar.AddRange(data0);
        ar.AddRange(data1);
        ar.AddRange(data2);
        ar.AddRange(data3);
        ar.AddRange(data4);                 // 填充数组

        float[] datas = ar.ToArray();         //将List转化成Arrary，赋值给datas （实现了数据的压平整合）
        clip = AudioClip.Create("temp", datas.Length, 1, 16000, false);     //新建音频资源
        clip.SetData(datas, 0);                                           // SetData 在剪辑中设置样本数据
    }
}
