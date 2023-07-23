using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] gbs;
    void Start()
    {
        foreach(GameObject gb in gbs)
        {
            gb.SetActive(false);
        }
        gbs[0].SetActive(true);
    }

   public void page1()
    {
        foreach(GameObject gb in gbs)
        {
            gb.SetActive(false);
        }
        gbs[1].SetActive(true);
    }

    public void page2()
    {
        foreach(GameObject gb in gbs)
        {
            gb.SetActive(false);
        }
        gbs[2].SetActive(true);
    }
    public void page3()
    {
        foreach(GameObject gb in gbs)
        {
            gb.SetActive(false);
        }
        gbs[3].SetActive(true);
    }

    public void page4()
    {
        foreach(GameObject gb in gbs)
        {
            gb.SetActive(false);
        }
        gbs[4].SetActive(true);
    }
}
