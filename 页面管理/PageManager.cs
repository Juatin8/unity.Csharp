using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] gbs;
    private int currentPage = 0;
    void Start()
    {
        ShowPage(currentPage);
    }
    public void ShowPage(int index)
    {
        // ��������ҳ��
        foreach(GameObject gb in gbs)
        {
            gb.SetActive(false);
        }

        // ��ʾָ��������ҳ��
        if(index >= 0 && index < gbs.Length)
        {
            gbs[index].SetActive(true);
            currentPage = index;
        }
    }
}