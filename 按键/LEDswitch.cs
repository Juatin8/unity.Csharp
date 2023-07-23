private bool ledON;

public void OnLED()
{
    ledON = !ledON;
    if(ledON)
    {
        SendByte((byte)0x01);
    }
    else
    {
        SendByte((byte)0x00);
    }
}