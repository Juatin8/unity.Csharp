using UnityEngine;

public class Common
{
    public static int[] GetRandomSequence(int Ocount, int RandomCount)   //生成不随机的几个数字,输出数组
    {
        int[] newNumbers = new int[RandomCount];
        int temp;
        for(int i = 0; i < RandomCount; i++)
        {
            temp = Random.Range(0, Ocount);                     //temp是序列中产生的一个随机值
            while(System.Array.IndexOf(newNumbers, temp) > -1)     //判断集合中有无生成的随机数，若有则重新生成随机数，直到生成的随机数list集合中没有才退出循环
            {
                temp = Random.Range(0, Ocount);
            }
            newNumbers[i] = temp;
        }
        return newNumbers;
    }
}
