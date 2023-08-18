    // ----------------- 单例的写法 ---------------------
    private static TextManager _instance = null;
    public static TextManager Instance
    {
        get
        {
            if(_instance == null)
                Debug.Log("GameManager is null !");
            return _instance;
        }
    }
    //----------------------------------------------
