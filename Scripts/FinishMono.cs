using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMono : MonoBehaviour
{
    public List<string> list_num;
    public int total_num;
    public int num;
    // Start is called before the first frame update
    void Start()
    {
        
        list_num = new List<string>();
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnFinishDel, OnFinishDel);
        //print("puv :" + total_num);
    }

    private bool OnFinishDel(int eventId, object arg)
    {
        num = 0;
        list_num.Clear();
        return false;
    }


    private void OnTriggerEnter(Collider other)
    {

        var _id = other.GetComponent<RoleMono>().jockey;
        //var objname = GameObject.Find(other.name);
        //print("жу╣Ц:" + _id);
        string _name = other.name;

        list_num.Add(_name);
        list_num.Add(_id.ToString());
        num++;
        if (num >= total_num  - 1)
        {
            GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnFinish_Last, 1);
        }

        ////print(list_num.Count);
        //if (list_num.Count > 2)
        //{
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnFinish, list_num);
        list_num.Clear();
        //}

    }
}
