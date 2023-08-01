# Unity中数据管理，用静态类比较方便

public static class StatisticsManager
{
    //服药信息
    //时段一服药信息
    public static string t1 = "08:00";
    public static string t1_meds1_name = "泮托拉唑";
    public static string t1_meds2_name = null;
    public static string t1_meds3_name = null;
    public static int t1_meds1_qty = 1;
    public static int t1_meds2_qty = 0;
    public static int t1_meds3_qty = 0;

    //时段2服药信息
    public static string t2 = "12:00";
    public static string t2_meds1_name = "泮托拉唑";
    public static string t2_meds2_name = "氯雷他定";
    public static string t2_meds3_name = "克林霉素";
    public static int t2_meds1_qty = 2;
    public static int t2_meds2_qty = 1;
    public static int t2_meds3_qty = 1;

    //时段3服药信息
    public static string t3 = "19:00";
    public static string t3_meds1_name = "泮托拉唑";
    public static string t3_meds2_name = null;
    public static string t3_meds3_name = "克林霉素";
    public static int t3_meds1_qty = 1;
    public static int t3_meds2_qty = 0;
    public static int t3_meds3_qty = 1;

    //所有时段服药完成
    public static string timebktAllDone = "今日服药完成";
    public static string done = null;
}

