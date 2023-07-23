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
        // 隐藏所有页面
        foreach(GameObject gb in gbs)
        {
            gb.SetActive(false);
        }

        // 显示指定索引的页面
        if(index >= 0 && index < gbs.Length)
        {
            gbs[index].SetActive(true);
            currentPage = index;
        }
    }
}