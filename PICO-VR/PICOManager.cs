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

    //--------------------------读PICO设备------------------------------

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

    //-------------------------写PICO设备-------------------------
    public static void VolumeDown()
    {
        PXR_System.VolumeDown();
    }

    public static void VolumeUP()
    {
        PXR_System.VolumeUp();
    }
public void EnableSeeThrough(bool enable)
{
PXR_Boundary.EnableSeeThroughManual(enable)；
    }
    
    public static class VibrationController
    {
        public static void HandVibrate(float intensity, int duration, int hand)     //简化封装手柄震动的函数
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
