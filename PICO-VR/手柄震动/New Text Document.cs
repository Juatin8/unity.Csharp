public void HandVibrate(float intensity, int duration, int hand)     //简化封装手柄震动的函数
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
