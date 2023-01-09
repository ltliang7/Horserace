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
        //print("��ʼ��Ϸ");
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
        //print("������?");
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
        //print("����:" + changci_num);
        num.text = "��" + NumToChinese(changci_num.ToString()) + "��";
        chanci.text = "���� "+ changci_num.ToString();
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
        BeSideBar.transform.GetChild(1).Find("Text1").GetComponent<Text>().text = "ِ��: " +horseDatas[changci_num - 1].race_name;
        BeSideBar.transform.GetChild(1).Find("Text2").GetComponent<Text>().text = "���c: " + horseDatas[changci_num - 1].ground;
        BeSideBar.transform.GetChild(1).Find("Text3").GetComponent<Text>().text = "ِ��: " + horseDatas[changci_num - 1].distance + "��";
        
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
        //print("�D��");
        if (changci_num > 1)
        {
            changci_num--;
        }
        
        num.text = "��" + NumToChinese(changci_num.ToString()) + "��";
    }

    void Onturnright()
    {
        //print("�D��");
        if (changci_num < 9)
        {
            changci_num++;
        }
        num.text = "��" + NumToChinese(changci_num.ToString()) + "��";
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

        for (int i = 1; i < SortUI.transform.childCount; i++) //�h����������icon
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
        //print("�ڶ����L��:" + sort_list.Count);
        if (thefirst < 1)
        {
            //print("���ق��M��:" + sort_list.Count);
            csort_obj = new GameObject[sort_list.Count];
            for (int i = 0; i < sort_list.Count; i++)//��ʼ����߅��ICON
            {
                var csort = Instantiate(Resources.Load("Csort")) as GameObject;
                csort.transform.SetParent(BeSideBar.transform.GetChild(2).transform, false);
                csort.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-869.9f, -314.9f + i * -50);
                //Sprite sprite = Resources.Load(horseDatas[changci_num - 1].winnerinfos[i].jockey, typeof(Sprite)) as Sprite;
                csort_obj[i] = csort; //�@ȡ�˵�һ�ε�ICON����
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
        //����ת��Ϊ���ĺ������ //ת����ע������ http://www.shang11.com  
        //string[] P_array_num = new string[] { "��", "Ҽ", "��", "��", "��", "��", "½", "��", "��", "��" };
        string[] P_array_num = new string[] { "��", "һ", "��", "��", "��", "��", "��", "��", "��", "��" };
        //Ϊ����λ������һ��λ����  
        string[] P_array_digit = new string[] { "", "ʰ", "��", "Ǫ" };
        //Ϊ���ֵ�λ����һ����λ����  
        string[] P_array_units = new string[] { "", "��", "��", "����" };
        string P_str_returnValue = ""; //����ֵ  
        int finger = 0; //�ַ�λ��ָ��  
        int P_int_m = x.Length % 4; //ȡģ  
        int P_int_k = 0;
        if (P_int_m > 0)
            P_int_k = x.Length / 4 + 1;
        else
            P_int_k = x.Length / 4;
        //���ѭ��,��λһ��,ÿ�������ϵ�λ: ",����,",",��,",",��,"  
        for (int i = P_int_k; i > 0; i--)
        {
            int P_int_L = 4;
            if (i == P_int_k && P_int_m != 0)
                P_int_L = P_int_m;
            //�õ�һ����λ��  
            string four = x.Substring(finger, P_int_L);
            int P_int_l = four.Length;
            //�ڲ�ѭ���ڸ����е�ÿһλ����ѭ��  
            for (int j = 0; j < P_int_l; j++)
            {
                //�������е�ÿһλ���������ڵ�λ  
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
            //ÿ��������һ����λ:",��,",",��," ��  
            if (i < P_int_k) //����������λ��һ��  
            {
                if (Convert.ToInt32(four) != 0)
                    //�������4λ��ȫ��0����ϵ�λ",��,",",��,"��  
                    P_str_returnValue += P_array_units[i - 1];
            }
            else
            {
                //�������λ��һ��,��������ϵ�λ  
                P_str_returnValue += P_array_units[i - 1];
            }
        }
        return P_str_returnValue;
    }
}
