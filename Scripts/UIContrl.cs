using UnityEngine.UI;
using UnityEngine;
using Game;
using System.Collections.Generic;
using System;

public class UIContrl : MonoBehaviour
{
    public GameObject TopBar;
    public Button Star;
    public GameObject BeSideBar;
    public GameObject SortUI;
    public Button LogoStar;
    private int changci_num;
    public GameObject JockeyMgr;
    private Text num;
    private int index_page;
    private Image[] iconUI;
    private Text chanci;
    private int thefirst;
    Button turnleft;
    Button turnright;
    private int Role_len;
    // Start is called before the first frame update
   
    void Start()
    {
        changci_num = 1;
        Star.onClick.AddListener(OnStar);
        Button exit = TopBar.transform.Find("exitbtn").GetComponent<Button>();
        exit.onClick.AddListener(OnExit);
        turnleft = TopBar.transform.Find("turnleft").GetComponent<Button>();
        turnright = TopBar.transform.Find("turnright").GetComponent<Button>();
        Button turnpage = BeSideBar.transform.Find("turnpage").GetComponent<Button>();
        turnleft.onClick.AddListener(Onturnleft);
        turnright.onClick.AddListener(Onturnright);
        turnpage.onClick.AddListener(Onturnpage);
        turnleft.gameObject.SetActive(false);
        turnright.gameObject.SetActive(false);
        num = TopBar.transform.Find("turnbtn").GetChild(0).GetComponent<Text>();
        chanci = TopBar.transform.Find("chanci").GetComponent<Text>();
        num.text = "";
        Button setbtn = TopBar.transform.Find("setbtn").GetComponent<Button>();
        setbtn.onClick.AddListener(OnSetbtn);
        LogoStar.onClick.AddListener(OnLogoStar);
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnTotal, OnTotal);
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnTouchPoint, OnTouchpoint);
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnFinish_Last, OnFinish);
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnTouchPoint_Big, OnTouchPoint_Big);
        SortUI.transform.GetChild(0).gameObject.SetActive(false);
        Star.gameObject.SetActive(false);
        index_page = 1;
        thefirst = 0;
        BeSideBar.SetActive(false);
    }

    private void OnLogoStar()
    {
        Star.gameObject.SetActive(false);
        //print("开始游戏");
        BeSideBar.SetActive(false);
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnLogoStar, "");
        LogoStar.transform.parent.gameObject.SetActive(false);
    }

    private bool OnTouchPoint_Big(int eventId, object arg)
    {
        var sort_list = (List<string>)arg;
        for (int i = 0; i < sort_list.Count; i++)
        {
            var jockey = JockeyMgr.transform.Find(sort_list[i]).GetComponent<RoleMono>().jockey;
            Sprite sprite = Resources.Load(jockey, typeof(Sprite)) as Sprite;
            iconUI[i].GetComponent<Image>().sprite = sprite;
            //iconUI[i].transform.GetChild(0).GetComponent<Text>().text = sort_list[i].ToString();
        }
        sort_list.Clear();

        return false;
    }

    private bool OnFinish(int eventId, object arg)
    {
        //print("跑完了?");
        Star.gameObject.SetActive(true);
        turnleft.gameObject.SetActive(true);
        turnright.gameObject.SetActive(true);
        
        BeSideBar.SetActive(false);
        SortUI.SetActive(false);
        return false;
    }
    private bool OnTotal(int eventId, object arg)
    {
        var horseDatas = (List<HorseData>)arg;
        //print(horseDatas);
        Role_len = horseDatas[changci_num - 1].winnerinfos.Length;
        //print("場次:" + changci_num);
        num.text = "第" + NumToChinese(changci_num.ToString()) + "場";
        chanci.text = "場次 "+ changci_num.ToString();
        iconUI = new Image[horseDatas[changci_num - 1].winnerinfos.Length];
        for (int i = 0; i < horseDatas[changci_num - 1].winnerinfos.Length; i++)
        {
            var icon = Instantiate(Resources.Load("icon")) as GameObject;
            icon.transform.SetParent(SortUI.transform, false);
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-495f + i * 75, -388.44f);
            Sprite sprite = Resources.Load(horseDatas[changci_num - 1].winnerinfos[i].jockey, typeof(Sprite)) as Sprite;
            icon.GetComponent<Image>().sprite = sprite;
            iconUI[i] = icon.GetComponent<Image>();
        }
        SortUI.transform.GetChild(0).gameObject.SetActive(true);
        BeSideBar.transform.GetChild(1).Find("Text1").GetComponent<Text>().text = "賽事: " +horseDatas[changci_num - 1].race_name;
        BeSideBar.transform.GetChild(1).Find("Text2").GetComponent<Text>().text = "地點: " + horseDatas[changci_num - 1].ground;
        BeSideBar.transform.GetChild(1).Find("Text3").GetComponent<Text>().text = "賽道: " + horseDatas[changci_num - 1].distance + "米";
        
        //horseDatas.Clear();
        return false;
    }

    void Onturnpage()
    {
        if (index_page == 1)
        {
            //print("page1");
            BeSideBar.transform.GetChild(1).gameObject.SetActive(false);
            BeSideBar.transform.GetChild(2).gameObject.SetActive(true);
            index_page = 2;
            return;
        }
        if (index_page == 2)
        {
            //print("page2");
            BeSideBar.transform.GetChild(1).gameObject.SetActive(true);
            BeSideBar.transform.GetChild(2).gameObject.SetActive(false);
            
            index_page = 1;
            return;
        }

    }

    void OnSetbtn()
    {
        
    }

    void Onturnleft()
    {
        //print("轉左");
        if (changci_num > 1)
        {
            changci_num--;
        }
        
        num.text = "第" + NumToChinese(changci_num.ToString()) + "場";
    }

    void Onturnright()
    {
        //print("轉右");
        if (changci_num < 9)
        {
            changci_num++;
        }
        num.text = "第" + NumToChinese(changci_num.ToString()) + "場";
    }

    void OnStar()
    {
        thefirst = 0;
        turnleft.gameObject.SetActive(false);
        turnright.gameObject.SetActive(false);
        //BeSideBar.SetActive(true);
        Star.gameObject.SetActive(false);
        SortUI.SetActive(true);
        SortUI.transform.GetChild(0).gameObject.SetActive(false);
        if (Role_len > 3)
        {
            Role_len = 3;
        }

        for (int i = 2; i < Role_len + 2; i++)
        {
            Destroy(BeSideBar.transform.GetChild(2).GetChild(i).gameObject);
        }

        for (int i = 1; i < SortUI.transform.childCount; i++) //刪除所有排名icon
        {
            Destroy(SortUI.transform.GetChild(i).gameObject);
        }

        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnFinishDel, "");
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnTouchDel,"");
        GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnStart, changci_num - 1);
        Star.gameObject.SetActive(false);
    }

    void OnExit()
    {
        //Application.Quit();
        Application.OpenURL("about:blank");
    }

    private GameObject[] csort_obj;
    private bool OnTouchpoint(int eventId, object arg)
    {
        BeSideBar.SetActive(true);
        
        var sort_list = (List<string>)arg;
        //print("第二次長度:" + sort_list.Count);
        if (thefirst < 1)
        {
            //print("多少個進來啊:" + sort_list.Count);
            csort_obj = new GameObject[sort_list.Count];
            for (int i = 0; i < sort_list.Count; i++)//初始化左邊的ICON
            {
                var csort = Instantiate(Resources.Load("Csort")) as GameObject;
                csort.transform.SetParent(BeSideBar.transform.GetChild(2).transform, false);
                csort.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-869.9f, -314.9f + i * -50);
                //Sprite sprite = Resources.Load(horseDatas[changci_num - 1].winnerinfos[i].jockey, typeof(Sprite)) as Sprite;
                csort_obj[i] = csort; //獲取了第一次的ICON對象
            }
            
        }
        thefirst++;


        for (int i = 0; i < sort_list.Count; i++)
        {
            var jockey = JockeyMgr.transform.Find(sort_list[i]).GetComponent<RoleMono>().jockey;
            Sprite sprite = Resources.Load(jockey, typeof(Sprite)) as Sprite;
            csort_obj[i].GetComponent<Image>().sprite = sprite;
            csort_obj[i].transform.GetChild(0).GetComponent<Text>().text = sort_list[i].ToString();
        }
        sort_list.Clear();

        return false;
    }


    public string NumToChinese(string x)
    {
        //数字转换为中文后的数组 //转载请注明来自 http://www.shang11.com  
        //string[] P_array_num = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        string[] P_array_num = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        //为数字位数建立一个位数组  
        string[] P_array_digit = new string[] { "", "拾", "佰", "仟" };
        //为数字单位建立一个单位数组  
        string[] P_array_units = new string[] { "", "万", "亿", "万亿" };
        string P_str_returnValue = ""; //返回值  
        int finger = 0; //字符位置指针  
        int P_int_m = x.Length % 4; //取模  
        int P_int_k = 0;
        if (P_int_m > 0)
            P_int_k = x.Length / 4 + 1;
        else
            P_int_k = x.Length / 4;
        //外层循环,四位一组,每组最后加上单位: ",万亿,",",亿,",",万,"  
        for (int i = P_int_k; i > 0; i--)
        {
            int P_int_L = 4;
            if (i == P_int_k && P_int_m != 0)
                P_int_L = P_int_m;
            //得到一组四位数  
            string four = x.Substring(finger, P_int_L);
            int P_int_l = four.Length;
            //内层循环在该组中的每一位数上循环  
            for (int j = 0; j < P_int_l; j++)
            {
                //处理组中的每一位数加上所在的位  
                int n = Convert.ToInt32(four.Substring(j, 1));
                if (n == 0)
                {
                    if (j < P_int_l - 1 && Convert.ToInt32(four.Substring(j + 1, 1)) > 0 && !P_str_returnValue.EndsWith(P_array_num[n]))
                        P_str_returnValue += P_array_num[n];
                }
                else
                {
                    if (!(n == 1 && (P_str_returnValue.EndsWith(P_array_num[0]) | P_str_returnValue.Length == 0) && j == P_int_l - 2))
                        P_str_returnValue += P_array_num[n];
                    P_str_returnValue += P_array_digit[P_int_l - j - 1];
                }
            }
            finger += P_int_L;
            //每组最后加上一个单位:",万,",",亿," 等  
            if (i < P_int_k) //如果不是最高位的一组  
            {
                if (Convert.ToInt32(four) != 0)
                    //如果所有4位不全是0则加上单位",万,",",亿,"等  
                    P_str_returnValue += P_array_units[i - 1];
            }
            else
            {
                //处理最高位的一组,最后必须加上单位  
                P_str_returnValue += P_array_units[i - 1];
            }
        }
        return P_str_returnValue;
    }
}
