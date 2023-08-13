using System;
using System.Collections.Generic;
using System.Linq;
public static class SequenceManager
{
    public static T[] CombineArrays<T>(T[] array1, T[] array2)   // 将两个数组合并成一个新的数组
    {
        T[] combinedArray = array1.Concat(array2).ToArray(); 
        return combinedArray;
    }
    public static T[] CombineArrays<T>(params T[][] arrays)
    {
        T[] combinedArray = new T[arrays.Sum(a => a.Length)];
        int offset = 0;
        foreach(T[] array in arrays)
        {
            Array.Copy(array, 0, combinedArray, offset, array.Length);
            offset += array.Length;
        }
        return combinedArray;
    }

    public static TKey[] GetRandomKeys<TKey, TValue>(IDictionary<TKey, TValue> dict, int count) //从字典中获取若干作为array
    {
        if(count > dict.Count)
        {
            throw new ArgumentException("取得的键的数量不能超过字典中的键的数量");
        }

        List<TKey> keys = new List<TKey>(dict.Keys);
        List<TKey> resultKeys = new List<TKey>();
        System.Random random = new System.Random();

        for(int i = 0; i < count; i++)
        {
            int randomIndex = random.Next(keys.Count);
            resultKeys.Add(keys[randomIndex]);
            keys.RemoveAt(randomIndex);
        }

        return resultKeys.ToArray();
    }


    public static int[] GetRandomSequence(int Ocount, int RandomCount)   //生成不随机的几个数字,输出数组
    {
        int[] newNumbers = new int[RandomCount];
        int temp;
        for(int i = 0; i < RandomCount; i++)
        {
            temp = UnityEngine.Random.Range(0, Ocount);                     //temp是序列中产生的一个随机值
            while(System.Array.IndexOf(newNumbers, temp) > -1)     //判断集合中有无生成的随机数，若有则重新生成随机数，直到生成的随机数list集合中没有才退出循环
            {
                temp = UnityEngine.Random.Range(0, Ocount);
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
            currentRandomNumber = UnityEngine.Random.Range(0, Ocount);
            while((generatedNumbers.Contains(currentRandomNumber)) || (fixedNumber == currentRandomNumber))
            {
                currentRandomNumber = UnityEngine.Random.Range(0, Ocount);
            }
            generatedNumbers.Add(currentRandomNumber);
        }
        int[] generatedNums = generatedNumbers.ToArray();
        return generatedNums;
    }

    // ---------------随机打乱数组-------------------Fisher-Yates随机算法
    public static void ShuffleArray<T>(T[] array)   // Array
    {
        for(int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public static void ShuffleList<T>(List<T> list)    // List,解释下这个算法，就是从最后一个元素开始，随机选取一个元素，然后交换，然后再从倒数第二个元素开始，随机选取一个元素，然后交换，以此类推，直到第一个元素。
    {
        for(int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];      // 取出最后一个元素
            list[i] = list[j];     // 让最后一个元组的值等于随机选取的元素
            list[j] = temp;         // 让随机选取的元素等于最后一个元素
        }
    }

    // ------------- 从数组中随机位置取出一个元素----------------------
    public static T GetRandomItem<T>(List<T> list)
    {
        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }
    public static T GetRandomItem<T>(T[] array)
    {
        int index = UnityEngine.Random.Range(0, array.Length);
        return array[index];
    }
    public static T GetRandomItem<T>(T[] array, ref int rd)
    {
        int index = UnityEngine.Random.Range(0, array.Length);
        rd = index;
        return array[index];
    }

    // ------------- 从数组中随机取出一个新的数组----------------------
    public static T[] ExtractRandomArray<T>(T[] originalArray, int numberOfElements)
    {
        int length = originalArray.Length;
        // 如果原始数组长度小于需要抽取的数量，则直接返回原始数组
        if(length <= numberOfElements)
        {
            return originalArray;
        }
        T[] randomArray = new T[numberOfElements];
        int[] indices = new int[length];
        // 初始化索引数组
        for(int i = 0; i < length; i++)
        {
            indices[i] = i;
        }
        // 随机抽取元素
        for(int i = 0; i < numberOfElements; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, length);
            int temp = indices[randomIndex];
            indices[randomIndex] = indices[i];
            indices[i] = temp;
            randomArray[i] = originalArray[indices[i]];
        }
        return randomArray;
    }
}
