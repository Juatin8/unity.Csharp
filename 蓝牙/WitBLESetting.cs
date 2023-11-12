using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WitBLESetting 
{
    public static byte[] BatteryData = { (byte)0xFF, (byte)0xAA, (byte)0x27, (byte)0x64, (byte)0x00 };
    public static byte[] QuaternionData = { (byte)0xFF, (byte)0xAA, (byte)0x27, (byte)0x51, (byte)0x00 };
    public static byte[] TemperatureData = { (byte)0xFF, (byte)0xAA, (byte)0x27, (byte)0x40, (byte)0x00 };
    public static byte[] MagenetData = { (byte)0xFF, (byte)0xAA, (byte)0x27, (byte)0x3A, (byte)0x00 };
    public static byte[] SetDataRate01hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x01, (byte)0x00 };
    public static byte[] SetDataRate05hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x02, (byte)0x00 };
    public static byte[] SetDataRate1hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x03, (byte)0x00 };
    public static byte[] SetDataRate2hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x04, (byte)0x00 };
    public static byte[] SetDataRate5hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x05, (byte)0x00 };
    public static byte[] SetDataRate10hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x06, (byte)0x00 };
    public static byte[] SetDataRate20hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x07, (byte)0x00 };
    public static byte[] SetDataRate50hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x08, (byte)0x00 };
    public static byte[] SetDataRate100hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x09, (byte)0x00 };
    public static byte[] SetDataRate200hz = { (byte)0xFF, (byte)0xAA, (byte)0x03, (byte)0x0A, (byte)0x00 };


    public static void ProcessQuaternionData(byte[] BLEdata, ref float[] Q)
    {
        byte[] header = { 0x55, 0x71, 0x51, 0x00 };
        bool startsWithHeader = BLEdata.Take(4).SequenceEqual(header);

        if (startsWithHeader)
        {
            byte Q0L = BLEdata[4];
            byte Q0H = BLEdata[5];
            byte Q1L = BLEdata[6];
            byte Q1H = BLEdata[7];
            byte Q2L = BLEdata[8];
            byte Q2H = BLEdata[9];
            byte Q3L = BLEdata[10];
            byte Q3H = BLEdata[11];

            Q[0] = (float)(Q0H << 8 | Q0L) / 32768;
            Q[1] = (float)(Q1H << 8 | Q1L) / 32768;
            Q[2] = (float)(Q2H << 8 | Q2L) / 32768;
            Q[3] = (float)(Q3H << 8 | Q3L) / 32768;

            // int checksum = 0x55 + 0x71 + 0x51 + 0x00 + Q0L + Q0H + Q1L + Q1H + Q2L + Q2H + Q3L + Q3H;
        }
    }

        public static void ProcessTempData(byte[] BLEdata, ref float tempValue)
        {
            byte[] header = { 0x55, 0x71, 0x40, 0x00 };
            bool startsWithHeader = BLEdata.Take(4).SequenceEqual(header);

            if (startsWithHeader)
            {
                byte TL = BLEdata[4];
                byte TH = BLEdata[5];
                tempValue= (float)(TH << 8 | TL) / 100;
            }


        }

    public static void ProcessMagenetData(byte[] BLEdata, ref int[] M)
    {
        byte[] header = { 0x55, 0x71, 0x3A, 0x00 };
        bool startsWithHeader = BLEdata.Take(4).SequenceEqual(header);

        if (startsWithHeader)
        {
            byte Q0L = BLEdata[4];
            byte Q0H = BLEdata[5];
            byte Q1L = BLEdata[6];
            byte Q1H = BLEdata[7];
            byte Q2L = BLEdata[8];
            byte Q2H = BLEdata[9];

            M[0] = (Q0H << 8 | Q0L);
            M[1] = (Q1H << 8 | Q1L);
            M[2] = (Q2H << 8 | Q2L);
        }
    }

    public static void ProcessBatteryData(byte[] BLEdata, ref float B)
    {
        byte[] header = { 0x55, 0x71, 0x64, 0x00 };
        bool startsWithHeader = BLEdata.Take(4).SequenceEqual(header);

        if (startsWithHeader)
        {
            byte Q0L = BLEdata[4];
            byte Q0H = BLEdata[5];
            B = (Q0H << 8 | Q0L);
            float voltage = B / 100;
            if (voltage > 3.96)
                B= 100;
            else if (voltage > 3.93)
                B= 90;
            else if (voltage > 3.87)
                B= 75;
            else if (voltage > 3.82)
                B= 60;
            else if (voltage > 3.79)
                B= 50;
            else if (voltage > 3.73)
                B= 40;
            else if (voltage > 3.70)
                B= 30;
            else if (voltage > 3.68)
                B= 20;
            else if (voltage > 3.50)
                B= 15;
            else if (voltage > 3.40)
                B= 10;
            else if (voltage > 0)
                B= 5;
            else
                B= 0;
        }
    }


    public static void ProcessDefaultAccelData(byte[] BLEdata,ref float[] accel)
    {
        byte[] header = { 0x55, 0x61};
        bool startsWithHeader = BLEdata.Take(2).SequenceEqual(header);

        if (startsWithHeader)
        {
            byte Q0L = BLEdata[2];
            byte Q0H = BLEdata[3];
            byte Q1L = BLEdata[4];
            byte Q1H = BLEdata[5];
            byte Q2L = BLEdata[6];
            byte Q2H = BLEdata[7];

            accel[0] = (float)(Q0H << 8 | Q0L) / 32768 * 16 * 9.8f;
            accel[1] = (float)(Q1H << 8 | Q1L) / 32768 * 16 * 9.8f;
            accel[2] = (float)(Q2H << 8 | Q2L) / 32768 * 16 * 9.8f;
        }

    }


    public static void ProcessDefaultGryoData(byte[] BLEdata, ref float[] gryo)
    {
        byte[] header = { 0x55, 0x61 };
        bool startsWithHeader = BLEdata.Take(2).SequenceEqual(header);

        if (startsWithHeader)
        {
            byte Q0L = BLEdata[8];
            byte Q0H = BLEdata[9];
            byte Q1L = BLEdata[10];
            byte Q1H = BLEdata[11];
            byte Q2L = BLEdata[12];
            byte Q2H = BLEdata[13];

            gryo[0] = (float)(Q0H << 8 | Q0L) / 32768 * 2000;
            gryo[1] = (float)(Q1H << 8 | Q1L) / 32768 * 2000;
            gryo[2] = (float)(Q2H << 8 | Q2L) / 32768 * 2000;
        }

    }

    public static void ProcessDefaultAngleData(byte[] BLEdata, ref float[] angle)
    {
        byte[] header = { 0x55, 0x61 };
        bool startsWithHeader = BLEdata.Take(2).SequenceEqual(header);

        if (startsWithHeader)
        {
            byte Q0L = BLEdata[8];
            byte Q0H = BLEdata[9];
            byte Q1L = BLEdata[10];
            byte Q1H = BLEdata[11];
            byte Q2L = BLEdata[12];
            byte Q2H = BLEdata[13];

            angle[0] = (float)(Q0H << 8 | Q0L) / 32768 * 180;
            angle[1] = (float)(Q1H << 8 | Q1L) / 32768 * 180;
            angle[2] = (float)(Q2H << 8 | Q2L) / 32768 * 180;
        }

    }
}
