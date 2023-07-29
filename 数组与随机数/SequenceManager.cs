using System.Collections.Generic;
using UnityEngine;
public static class SequenceManager
{
    public static int[] GetRandomSequence(int Ocount, int RandomCount)   //���ɲ�����ļ�������,�������
    {
        int[] newNumbers = new int[RandomCount];
        int temp;
        for(int i = 0; i < RandomCount; i++)
        {
            temp = Random.Range(0, Ocount);                     //temp�������в�����һ�����ֵ
            while(System.Array.IndexOf(newNumbers, temp) > -1)     //�жϼ������������ɵ�����������������������������ֱ�����ɵ������list������û�в��˳�ѭ��
            {
                temp = Random.Range(0, Ocount);
            }
            newNumbers[i] = temp;
        }
        return newNumbers;
    }

    public static int[] GetRandomWithOneFixed(int Ocount, int fixedNumber, int totalRdNum)   //Ocount ���������е�������fixednumber��֪������totalRdNum�ܹ����ٲ��ظ���
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

    // ------------- �����������λ��ȡ��һ��Ԫ��----------------------
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
