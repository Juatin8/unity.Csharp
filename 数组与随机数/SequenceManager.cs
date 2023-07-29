using System.Collections.Generic;
using UnityEngine;
public static class SequenceManager
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

    public static int[] GetRandomWithOneFixed(int Ocount, int fixedNumber, int totalRdNum)   //Ocount 本来的数列的数量，fixednumber已知的数，totalRdNum总共多少不重复数
    {
        List<int> generatedNumbers = new List<int>();
        generatedNumbers.Add(fixedNumber);
        for(int i = 0; i < totalRdNum - 1; i++)
        {
            int currentRandomNumber;
            currentRandomNumber = Random.Range(0, Ocount);
            while((generatedNumbers.Contains(currentRandomNumber)) || (fixedNumber == currentRandomNumber))
            {
                currentRandomNumber = Random.Range(0, Ocount);
            }
            generatedNumbers.Add(currentRandomNumber);
        }
        int[] generatedNums = generatedNumbers.ToArray();
        return generatedNums;
    }

    // ------------- 从数组中随机位置取出一个元素----------------------
    public static T GetRandomItem<T>(List<T> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }
    public static T GetRandomItem<T>(T[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }
    public static T GetRandomItem<T>(T[] array,ref int rd)
    {
        int index = Random.Range(0, array.Length);
        rd = index;
        return array[index];
    }
}
