using System.Collections;
using TMPro;
using UnityEngine;
public class NetworkDetector : MonoBehaviour
{
    private TextMeshProUGUI feedback;
    private void Start()
    {
        feedback= GameObject.Find("feedback").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            StartCoroutine(noInternetWarn());
        }
    }

    IEnumerator noInternetWarn()
    {
        feedback.text = "没有网络，请连接网络";
        yield return new WaitForSeconds(1);
    }

}