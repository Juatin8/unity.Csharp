using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] gbs;
    private int currentPage = 0;
    void Awake() //必须是awake, start的话会导致面板中的一些脚本的start抢跑
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
