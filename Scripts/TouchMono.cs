using System.Collections.Generic;
using UnityEngine;
using Game;
public class TouchMono : MonoBehaviour
{
    public int id; //����ID���Á�ֶΙz�����R�ɹ���
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
        //print("���������ײ����");
        list_num.Clear();
        list_big_num.Clear();
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(total_big);
        //var _id = other.GetComponent<RoleMono>().id;
        if (id == 2) //��ӡһ������ ��һ�A��
        {
            //print("��һ��");
            //print(other.name);
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnSection, 1);
        }

        if (id == 7) //��ӡһ������ �ڶ��A��
        {
            //print("�ڶ���");
            //print(other.name);
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnSection, 2);
        }

        //if (id == 15) //��ӡһ������ �ڶ��A��
        //{
        //    print("������");
        //    print(other.name);
        //    GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnSection, 3);
        //}

        list_num.Add(other.name);
        list_big_num.Add(other.name);

        if (onlyone < 1)
        {
            //print("��һ�������M��:" + list_num.Count +"///max:" + total);
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
