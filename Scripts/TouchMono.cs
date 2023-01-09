using System.Collections.Generic;
using UnityEngine;
using Game;
public class TouchMono : MonoBehaviour
{
    public int id; //特殊ID，用來分段檢查造馬成功率
    public int total;
    public int total_big;
    public List<string> list_num;
    public List<string> list_big_num;
    public int onlyone;
    
    // Start is called before the first frame update
    void Start()
    {

        list_num = new List<string>();
        list_big_num = new List<string>();
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnTouchDel, OnTouchDel);
        //onlyone = 0;
    }

    private bool OnTouchDel(int eventId, object arg)
    {
        //print("清楚所有碰撞數據");
        list_num.Clear();
        list_big_num.Clear();
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(total_big);
        //var _id = other.GetComponent<RoleMono>().id;
        if (id == 2) //打印一下排名 第一階段
        {
            //print("第一段");
            //print(other.name);
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnSection, 1);
        }

        if (id == 7) //打印一下排名 第二階段
        {
            //print("第二段");
            //print(other.name);
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnSection, 2);
        }

        //if (id == 15) //打印一下排名 第二階段
        //{
        //    print("第三段");
        //    print(other.name);
        //    GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnSection, 3);
        //}

        list_num.Add(other.name);
        list_big_num.Add(other.name);

        if (onlyone < 1)
        {
            //print("第一次後再進來:" + list_num.Count +"///max:" + total);
            if (list_num.Count > total)
            {
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnTouchPoint, list_num);
                list_num.Clear();
                onlyone++;
            }

        }

        if (list_big_num.Count > total_big - 1)
        {
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnTouchPoint_Big, list_big_num);
            list_big_num.Clear();
        }
    }
}
