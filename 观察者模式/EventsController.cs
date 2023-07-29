using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour
{
    //----------------------单例写法----------------------------
    public static EventsController Instance = null;
    private void Awake()
    {
        Instance = this;
    }
    //------------------------------------------------------

    private Animal am;
    private TouchEvent te;
    private TeacherAudio ta;
    private SpeakerAudio sa;
    private Button3D bn;
    private Hand[] hs;
    private TimeManager sm;

    // 事件统一集中管理
    [HideInInspector]
    public UnityEvent CollideEvent, ChooseRightEvent, ChooseWrongEvent;         //添加一个手部碰撞事件, 选对了事件, 选错了事件
    public UnityEvent HoldphoneEvent, StillPhoneEvent;   //搞一个拿起电话机的事件 & 电话机原位静止的事件
    public UnityEvent DestroyAnimalsEvent;  //搞一个销毁动物的事件
    public UnityEvent LevelOverEvent;    //过关逻辑
    public UnityEvent ButtonDownEvent;

    private void Start()
    {
        am = GameObject.Find("Animals").GetComponent<Animal>();
        te = GameObject.Find("RHand").GetComponent<TouchEvent>();      //找脚本
        ta = GameObject.Find("Teacher").GetComponent<TeacherAudio>();
        sa = GameObject.Find("Speaker").GetComponent<SpeakerAudio>();
        bn = GameObject.Find("ReplayBtn").GetComponent<Button3D>();
        hs[0] = GameObject.Find("LHand").GetComponent<Hand>();
        hs[1] = GameObject.Find("RHand").GetComponent<Hand>();

        HoldphoneEvent.AddListener(sa.PhoneSpeak);     //拿起电话机------发声音
        StillPhoneEvent.AddListener(sa.StopAudio);     //电话机原位------停止播放
        ChooseRightEvent.AddListener(sa.AllowPlay);          //选对-----允许重播
        ChooseRightEvent.AddListener(sa.PhoneRingLater);    //选对-----允许过段时间电话铃声响起
        ButtonDownEvent.AddListener(sa.AllowPlayAndNoGenerateName);     //按下重听按钮---允许重播且取消新语音合成
        HoldphoneEvent.AddListener(hs[0].phoneVibarate);    //电话震动
        HoldphoneEvent.AddListener(hs[1].phoneVibarate);    //电话震动
        CollideEvent.AddListener(hs[0].touchVibrate); // 震动
        HoldphoneEvent.AddListener(hs[1].touchVibrate);    //电话震动

        HoldphoneEvent.AddListener(Animal.Generate2Names);  //拿起电话机-----随机生成名字
        HoldphoneEvent.AddListener(am.spawnAnimalsLater);     //拿起电话机------稍晚些生成动物
        ChooseRightEvent.AddListener(am.AllowSpawnLater);     //选对-----------稍微晚一些就允许再次生成动物 
        ChooseRightEvent.AddListener(am.destroyAnimals);      //选对-----------5s后销毁动物

        //  sm = GameObject.Find("ScoreUICanvas").GetComponent<TimeManager>();


        //拿起电话机
        // HoldphoneEvent.AddListener(sm.TimeStart);         //拿起电话机-----开始计时
        //HoldphoneEvent.AddListener(hs[0].phoneVibarate);    //电话震动
        //HoldphoneEvent.AddListener(hs[1].phoneVibarate);    //电话震动
        // HoldphoneEvent.AddListener(Animal.Generate2Names);  //拿起电话机-----随机生成名字
        // HoldphoneEvent.AddListener(sa.PhoneSpeak);     //拿起电话机------发声音
        // HoldphoneEvent.AddListener(am.spawnAnimalsLater);     //拿起电话机------稍晚些生成动物

        // 电话机原位
        //StillPhoneEvent.AddListener(sa.StopAudio);     //电话机原位------停止播放

        // 选对了
        //ChooseRightEvent.AddListener(sa.AllowPlay);          //选对-----允许重播
        //ChooseRightEvent.AddListener(sa.PhoneRingLater);    //选对-----允许过段时间电话铃声响起
        // ChooseRightEvent.AddListener(am.AllowSpawnLater);     //选对-----------稍微晚一些就允许再次生成动物 
        ChooseRightEvent.AddListener(DataManager.TopicOver);         //选对事件-------话题结束
                                                                     // ChooseRightEvent.AddListener(ta.SayRight);         //选对-----------老师说对
                                                                     // ChooseRightEvent.AddListener(sm.TimeEnd); //选对了---------结束计时
                                                                     // ChooseRightEvent.AddListener(am.destroyAnimals);      //选对-----------5s后销毁动物

        // 选错了
        //ChooseWrongEvent.AddListener(ta.SayWrong);        //选错-----------老师说错
        // 按下按钮
        // bn.ButtonDownEvent.AddListener(sa.AllowPlayAndNoGenerateName);     //按下重听按钮---允许重播且取消新语音合成
        // 手部碰撞事件
        //CollideEvent.AddListener(hs[0].touchVibrate); // 震动
        //CollideEvent.AddListener(hs[1].touchVibrate); // 震动

    }
    private void Update()
    {

    }

}
