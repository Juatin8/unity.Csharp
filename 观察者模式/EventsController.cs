using UnityEngine;
using UnityEngine.Events;

public class EventsController : MonoBehaviour
{
    //----------------------����д��----------------------------
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

    // �¼�ͳһ���й���
    [HideInInspector]
    public UnityEvent CollideEvent, ChooseRightEvent, ChooseWrongEvent;         //���һ���ֲ���ײ�¼�, ѡ�����¼�, ѡ�����¼�
    public UnityEvent HoldphoneEvent, StillPhoneEvent;   //��һ������绰�����¼� & �绰��ԭλ��ֹ���¼�
    public UnityEvent DestroyAnimalsEvent;  //��һ�����ٶ�����¼�
    public UnityEvent LevelOverEvent;    //�����߼�
    public UnityEvent ButtonDownEvent;

    private void Start()
    {
        am = GameObject.Find("Animals").GetComponent<Animal>();
        te = GameObject.Find("RHand").GetComponent<TouchEvent>();      //�ҽű�
        ta = GameObject.Find("Teacher").GetComponent<TeacherAudio>();
        sa = GameObject.Find("Speaker").GetComponent<SpeakerAudio>();
        bn = GameObject.Find("ReplayBtn").GetComponent<Button3D>();
        hs[0] = GameObject.Find("LHand").GetComponent<Hand>();
        hs[1] = GameObject.Find("RHand").GetComponent<Hand>();

        HoldphoneEvent.AddListener(sa.PhoneSpeak);     //����绰��------������
        StillPhoneEvent.AddListener(sa.StopAudio);     //�绰��ԭλ------ֹͣ����
        ChooseRightEvent.AddListener(sa.AllowPlay);          //ѡ��-----�����ز�
        ChooseRightEvent.AddListener(sa.PhoneRingLater);    //ѡ��-----�������ʱ��绰��������
        ButtonDownEvent.AddListener(sa.AllowPlayAndNoGenerateName);     //����������ť---�����ز���ȡ���������ϳ�
        HoldphoneEvent.AddListener(hs[0].phoneVibarate);    //�绰��
        HoldphoneEvent.AddListener(hs[1].phoneVibarate);    //�绰��
        CollideEvent.AddListener(hs[0].touchVibrate); // ��
        HoldphoneEvent.AddListener(hs[1].touchVibrate);    //�绰��

        HoldphoneEvent.AddListener(Animal.Generate2Names);  //����绰��-----�����������
        HoldphoneEvent.AddListener(am.spawnAnimalsLater);     //����绰��------����Щ���ɶ���
        ChooseRightEvent.AddListener(am.AllowSpawnLater);     //ѡ��-----------��΢��һЩ�������ٴ����ɶ��� 
        ChooseRightEvent.AddListener(am.destroyAnimals);      //ѡ��-----------5s�����ٶ���

        //  sm = GameObject.Find("ScoreUICanvas").GetComponent<TimeManager>();


        //����绰��
        // HoldphoneEvent.AddListener(sm.TimeStart);         //����绰��-----��ʼ��ʱ
        //HoldphoneEvent.AddListener(hs[0].phoneVibarate);    //�绰��
        //HoldphoneEvent.AddListener(hs[1].phoneVibarate);    //�绰��
        // HoldphoneEvent.AddListener(Animal.Generate2Names);  //����绰��-----�����������
        // HoldphoneEvent.AddListener(sa.PhoneSpeak);     //����绰��------������
        // HoldphoneEvent.AddListener(am.spawnAnimalsLater);     //����绰��------����Щ���ɶ���

        // �绰��ԭλ
        //StillPhoneEvent.AddListener(sa.StopAudio);     //�绰��ԭλ------ֹͣ����

        // ѡ����
        //ChooseRightEvent.AddListener(sa.AllowPlay);          //ѡ��-----�����ز�
        //ChooseRightEvent.AddListener(sa.PhoneRingLater);    //ѡ��-----�������ʱ��绰��������
        // ChooseRightEvent.AddListener(am.AllowSpawnLater);     //ѡ��-----------��΢��һЩ�������ٴ����ɶ��� 
        ChooseRightEvent.AddListener(DataManager.TopicOver);         //ѡ���¼�-------�������
                                                                     // ChooseRightEvent.AddListener(ta.SayRight);         //ѡ��-----------��ʦ˵��
                                                                     // ChooseRightEvent.AddListener(sm.TimeEnd); //ѡ����---------������ʱ
                                                                     // ChooseRightEvent.AddListener(am.destroyAnimals);      //ѡ��-----------5s�����ٶ���

        // ѡ����
        //ChooseWrongEvent.AddListener(ta.SayWrong);        //ѡ��-----------��ʦ˵��
        // ���°�ť
        // bn.ButtonDownEvent.AddListener(sa.AllowPlayAndNoGenerateName);     //����������ť---�����ز���ȡ���������ϳ�
        // �ֲ���ײ�¼�
        //CollideEvent.AddListener(hs[0].touchVibrate); // ��
        //CollideEvent.AddListener(hs[1].touchVibrate); // ��

    }
    private void Update()
    {

    }

}
