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
    //-----------------------------------------------------

    // 事件统一集中管理
    [HideInInspector]
    public UnityEvent CollideEvent, ChooseRightEvent, ChooseWrongEvent;         //添加一个手部碰撞事件, 选对了事件, 选错了事件
    public UnityEvent HoldphoneEvent, StillPhoneEvent;   //搞一个拿起电话机的事件 & 电话机原位静止的事件
    public UnityEvent DestroyAnimalsEvent;  //搞一个销毁动物的事件
    public UnityEvent LevelOverEvent;    //过关逻辑
    public UnityEvent ButtonDownEvent;
}
