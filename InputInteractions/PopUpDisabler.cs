using UnityEngine;

public class PopUpDisabler : MonoBehaviour
{
   private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0)  
        {
            DisablePopupObjects();
        }
    }

    private void DisablePopupObjects()
    {
        GameObject[] popupObjects = GameObject.FindGameObjectsWithTag("popup");

        foreach(GameObject popupObject in popupObjects)
        {
            popupObject.SetActive(false);
        }
    }
}
