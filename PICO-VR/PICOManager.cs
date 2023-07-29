
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public static class PICOManager
{
    static InputDevice RHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    static InputDevice LHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    static InputDevice Head = InputDevices.GetDeviceAtXRNode(XRNode.Head);

    static bool isHeadOn;

    //--------------------------��PICO�豸------------------------------

    public static void getAccelaration()
    {
        float acceleration = Input.GetAxis("Acceleration");
    }

    public static void back2SystemMenu()
    {
        bool isdown;
        if(RHand.TryGetFeatureValue(CommonUsages.primaryButton, out isdown) && isdown)
        {
            SceneManager.LoadScene("EnterUI", LoadSceneMode.Single);
        }
    }

    //-------------------------дPICO�豸-------------------------
    public static void VolumeDown()
    {
        PXR_System.VolumeDown();
    }

    public static void VolumeUP()
    {
        PXR_System.VolumeUp();
    }


    public static class VibrationController
    {
        public static void HandVibrate(float intensity, int duration, int hand)     //�򻯷�װ�ֱ��𶯵ĺ���
        {
            if(hand == 0)
            {
                PXR_Input.SetControllerVibration(intensity, duration, PXR_Input.Controller.LeftController);
            }
            if(hand == 1)
            {
                PXR_Input.SetControllerVibration(intensity, duration, PXR_Input.Controller.RightController);
            }
        }
    }

}