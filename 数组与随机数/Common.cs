using UnityEngine;

public class Common
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
}
