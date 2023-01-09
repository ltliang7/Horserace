using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using Game;
//using UnityEngine.Networking;
using System.Collections;


//贝塞尔曲线就是对多个线段同时做差值
public class MainGame : MonoBehaviour
{
    //我们让其运动的物体
    public GameObject[] Role;
    public Text time_text;
    public GameObject PRF_StartGate;
    public GameObject paizi2; //終點線碰撞
    public GameObject[] door;
    public InputField inputField;

    public GameObject Content;
    public GameObject Content2;
    //public GameObject[] Sorticon;
    public GameObject Touchpoint;
    public GameObject[] Camera_n;
    public GameObject FinishMgr; //終點canvas ui
    //我们实现创建好的几个点,这里需要实现定义五个点
    public List<Transform> gameOjbet_tran = new List<Transform>();
    //private List<Transform> gameOjbet_tran2 = new List<Transform>();
    //private List<Transform> gameOjbet_tran3 = new List<Transform>();
    //private List<Transform> gameOjbet_tran4 = new List<Transform>();
    //private List<Transform> gameOjbet_tran5 = new List<Transform>();
    //private List<Transform> gameOjbet_tran6 = new List<Transform>();
    //private List<Transform> gameOjbet_tran7 = new List<Transform>();
    //private List<Transform> gameOjbet_tran8 = new List<Transform>();
    //private List<Transform> gameOjbet_tran9 = new List<Transform>();
    //private List<Transform> gameOjbet_tran10 = new List<Transform>();
    //private List<Transform> gameOjbet_tran11 = new List<Transform>();
    //private List<Transform> gameOjbet_tran12 = new List<Transform>();
    //private List<Transform> gameOjbet_tran13 = new List<Transform>();
    //private List<Transform> gameOjbet_tran14 = new List<Transform>();
    //小球的运动点,在Init中计算
    private List<Vector3> point = new List<Vector3>();
    private List<Vector3> point2 = new List<Vector3>();

    private int start2end;
    private float time;
    private int Cam_index =0;
    public Winnerinfo[] winnerinfos;
    private List<HorseData> horseList = new List<HorseData>();
    //private List<int> sort_num = new List<int>();
    private List<string> finish_sort_num = new List<string>();
    private Vector3[] allroleVector3; //記錄最初馬的座標

    private Quaternion[] allroleRota3;//記錄最初馬的角度

    private bool isHttpJson = true; // false是本地读JSON  ,  true是网络读取
    private JsonData json;
    private int horse_index;
    //private GameObject pointmgr;

    private int sort_num_index = 0;

    private float minute;
    private float second;

    private bool isFinish;
    private int index;
    private float fixedDeltaTime;
    private float finish_vr_time; //慢動作時間

    private List<List<Vector3>> list_trans = new List<List<Vector3>>();
    private List<GameObject> list_obj = new List<GameObject>();

    //测试专用
    public Text testdata;
    void Awake()
    {
        GlobalDispatcher.Create();
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnFinish, OnFinish);
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnStart,OnStart);
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnLogoStar, OnLogoStar);
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnFinish_Last, OnFinish_Last);

        this.fixedDeltaTime = Time.fixedDeltaTime;

        GetPathPoints();
        
        if (isHttpJson)
        {
            StartCoroutine(GetInterface("http://www.yileapp.cn/jockey/horse2.json"));
            //StartCoroutine(GetInterface(Application.streamingAssetsPath + "/horse2.json"));
        }
        else
        {
            var path = Application.streamingAssetsPath + "/horse2.json";
            StreamReader tmpReader = File.OpenText(path);
            string result = tmpReader.ReadToEnd();
            json = JsonMapper.ToObject(result);

            for (int i = 0; i < json.Count; i++)
            {
                var session = (string)json[i]["session"];
                var time = (string)json[i]["time"];
                var level = (string)json[i]["level"];
                var distance = (string)json[i]["distance"];
                var ground = (string)json[i]["ground"];
                var ground_type = (string)json[i]["ground type"];
                var race_name = (string)json[i]["race name"];

                JsonData jd = json[i]["winner"];
                winnerinfos = new Winnerinfo[jd.Count];

                for (int j = 0; j < jd.Count; j++)
                {
                    winnerinfos[j] = new Winnerinfo();
                    winnerinfos[j].no = (string)jd[j]["no"];
                    winnerinfos[j].forecast_ranking = (string)jd[j]["forecast ranking"];
                    winnerinfos[j].session_ranking = new int[jd[j]["session ranking"].Count];
                    for (int k = 0; k < jd[j]["session ranking"].Count; k++)
                    {
                        //print(jd[j]["session ranking"][0] + "/"+ jd[j]["session ranking"][1] + "/" + jd[j]["session ranking"][2] + "/" + jd[j]["session ranking"][3] +"/"+ jd[j]["session ranking"][4]);
                        winnerinfos[j].session_ranking[k] = (int)jd[j]["session ranking"][k];
                        //print("認識:" + winnerinfos[j].session_ranking[k]);
                    }

                    winnerinfos[j].model = (string)jd[j]["model"];
                    //print("厅:" + winnerinfo.model);
                    winnerinfos[j].jockey = (string)jd[j]["jockey"];
                    winnerinfos[j].name = (string)jd[j]["name"];
                }

                //存入信息
                HorseData horseData = new HorseData(session, time, level, distance, ground,
                                                    ground_type, race_name, winnerinfos);
                horseList.Add(horseData);
            }

            //----------------------------以上json读取和保存-------------------------//
           

            allroleVector3 = new Vector3[Role.Length];
            allroleRota3 = new Quaternion[Role.Length];
            for (int i = 0; i < Role.Length; i++)//首先 全部隐藏起来
            {
                allroleVector3[i] = Role[Role.Length - i - 1].transform.position; //记录开始坐标
                allroleRota3[i] = Role[Role.Length - i - 1].transform.rotation; //把整個trans記錄起來
                Role[i].SetActive(false);
            }

            inputField.text = "0";
            //InputNumToinitData(0);
        }

        Camera_n[0].transform.parent.GetComponent<AudioSource>().Stop();
        //start2end = 1;
        //time = 10f;
        //index = 0;
        //print("打印信息:" + horseList[0].winnerinfos[0].model);

    }


    //计算出指定个点,将他们练成一条直线,使其开起来像是曲线
    void GetPathPoints()
    {
        point = new List<Vector3>();
        float pointNumber = 70;
        for (int i = 0; i <= (int)pointNumber; i++)
        {
            //Debug.Log(i / pointNumber);//他的值从0 - 1
            //一
            Vector3 pos1 = Vector3.Lerp(gameOjbet_tran[0].position, gameOjbet_tran[1].position, i / pointNumber);
            Vector3 pos2 = Vector3.Lerp(gameOjbet_tran[1].position, gameOjbet_tran[2].position, i / pointNumber);
            Vector3 pos3 = Vector3.Lerp(gameOjbet_tran[2].position, gameOjbet_tran[3].position, i / pointNumber);
            Vector3 pos4 = Vector3.Lerp(gameOjbet_tran[3].position, gameOjbet_tran[4].position, i / pointNumber);
            Vector3 pos5 = Vector3.Lerp(gameOjbet_tran[4].position, gameOjbet_tran[5].position, i / pointNumber);
            Vector3 pos6 = Vector3.Lerp(gameOjbet_tran[5].position, gameOjbet_tran[6].position, i / pointNumber);
            Vector3 pos7 = Vector3.Lerp(gameOjbet_tran[6].position, gameOjbet_tran[7].position, i / pointNumber);
            Vector3 pos8 = Vector3.Lerp(gameOjbet_tran[7].position, gameOjbet_tran[8].position, i / pointNumber);
            //二
            var pos1_0 = Vector3.Lerp(pos1, pos2, i / pointNumber);
            var pos1_1 = Vector3.Lerp(pos2, pos3, i / pointNumber);
            var pos1_2 = Vector3.Lerp(pos3, pos4, i / pointNumber);
            var pos1_3 = Vector3.Lerp(pos4, pos5, i / pointNumber);
            var pos1_4 = Vector3.Lerp(pos5, pos6, i / pointNumber);
            var pos1_5 = Vector3.Lerp(pos6, pos7, i / pointNumber);
            var pos1_6 = Vector3.Lerp(pos7, pos8, i / pointNumber);
            //三
            var pos2_0 = Vector3.Lerp(pos1_0, pos1_1, i / pointNumber);
            var pos2_1 = Vector3.Lerp(pos1_1, pos1_2, i / pointNumber);
            var pos2_2 = Vector3.Lerp(pos1_2, pos1_3, i / pointNumber);
            var pos2_3 = Vector3.Lerp(pos1_3, pos1_4, i / pointNumber);
            var pos2_4 = Vector3.Lerp(pos1_4, pos1_5, i / pointNumber);
            var pos2_5 = Vector3.Lerp(pos1_5, pos1_6, i / pointNumber);
            //四
            var pos3_0 = Vector3.Lerp(pos2_0, pos2_1, i / pointNumber);
            var pos3_1 = Vector3.Lerp(pos2_1, pos2_2, i / pointNumber);
            var pos3_2 = Vector3.Lerp(pos2_2, pos2_3, i / pointNumber);
            var pos3_3 = Vector3.Lerp(pos2_3, pos2_4, i / pointNumber);
            var pos3_4 = Vector3.Lerp(pos2_4, pos2_5, i / pointNumber);
            //五
            var pos4_1 = Vector3.Lerp(pos3_0, pos3_1, i / pointNumber);
            var pos4_2 = Vector3.Lerp(pos3_1, pos3_2, i / pointNumber);
            var pos4_3 = Vector3.Lerp(pos3_2, pos3_3, i / pointNumber);
            var pos4_4 = Vector3.Lerp(pos3_3, pos3_4, i / pointNumber);
            //六
            var pos5_1 = Vector3.Lerp(pos4_1, pos4_2, i / pointNumber);
            var pos5_2 = Vector3.Lerp(pos4_2, pos4_3, i / pointNumber);
            var pos5_3 = Vector3.Lerp(pos4_3, pos4_4, i / pointNumber);
            //七
            var pos6_1 = Vector3.Lerp(pos5_1, pos5_2, i / pointNumber);
            var pos6_2 = Vector3.Lerp(pos5_2, pos5_3, i / pointNumber);

            Vector3 find = Vector3.Lerp(pos6_1, pos6_2, i / pointNumber);
            point.Add(find);
        }
    }

    void GetPathPoints2(List<Transform> gameOjbet_tran2)
    {
        point2 = new List<Vector3>();
        float pointNumber = 70;
        for (int i = 0; i <= (int)pointNumber; i++)
        {
            //Debug.Log(i / pointNumber);//他的值从0 - 1
            //一
            Vector3 pos1 = Vector3.Lerp(gameOjbet_tran2[0].position, gameOjbet_tran2[1].position, i / pointNumber);
            Vector3 pos2 = Vector3.Lerp(gameOjbet_tran2[1].position, gameOjbet_tran2[2].position, i / pointNumber);
            Vector3 pos3 = Vector3.Lerp(gameOjbet_tran2[2].position, gameOjbet_tran2[3].position, i / pointNumber);
            Vector3 pos4 = Vector3.Lerp(gameOjbet_tran2[3].position, gameOjbet_tran2[4].position, i / pointNumber);
            Vector3 pos5 = Vector3.Lerp(gameOjbet_tran2[4].position, gameOjbet_tran2[5].position, i / pointNumber);
            Vector3 pos6 = Vector3.Lerp(gameOjbet_tran2[5].position, gameOjbet_tran2[6].position, i / pointNumber);
            Vector3 pos7 = Vector3.Lerp(gameOjbet_tran2[6].position, gameOjbet_tran2[7].position, i / pointNumber);
            Vector3 pos8 = Vector3.Lerp(gameOjbet_tran2[7].position, gameOjbet_tran2[8].position, i / pointNumber);
            //二
            var pos1_0 = Vector3.Lerp(pos1, pos2, i / pointNumber);
            var pos1_1 = Vector3.Lerp(pos2, pos3, i / pointNumber);
            var pos1_2 = Vector3.Lerp(pos3, pos4, i / pointNumber);
            var pos1_3 = Vector3.Lerp(pos4, pos5, i / pointNumber);
            var pos1_4 = Vector3.Lerp(pos5, pos6, i / pointNumber);
            var pos1_5 = Vector3.Lerp(pos6, pos7, i / pointNumber);
            var pos1_6 = Vector3.Lerp(pos7, pos8, i / pointNumber);
            //三
            var pos2_0 = Vector3.Lerp(pos1_0, pos1_1, i / pointNumber);
            var pos2_1 = Vector3.Lerp(pos1_1, pos1_2, i / pointNumber);
            var pos2_2 = Vector3.Lerp(pos1_2, pos1_3, i / pointNumber);
            var pos2_3 = Vector3.Lerp(pos1_3, pos1_4, i / pointNumber);
            var pos2_4 = Vector3.Lerp(pos1_4, pos1_5, i / pointNumber);
            var pos2_5 = Vector3.Lerp(pos1_5, pos1_6, i / pointNumber);
            //四
            var pos3_0 = Vector3.Lerp(pos2_0, pos2_1, i / pointNumber);
            var pos3_1 = Vector3.Lerp(pos2_1, pos2_2, i / pointNumber);
            var pos3_2 = Vector3.Lerp(pos2_2, pos2_3, i / pointNumber);
            var pos3_3 = Vector3.Lerp(pos2_3, pos2_4, i / pointNumber);
            var pos3_4 = Vector3.Lerp(pos2_4, pos2_5, i / pointNumber);
            //五
            var pos4_1 = Vector3.Lerp(pos3_0, pos3_1, i / pointNumber);
            var pos4_2 = Vector3.Lerp(pos3_1, pos3_2, i / pointNumber);
            var pos4_3 = Vector3.Lerp(pos3_2, pos3_3, i / pointNumber);
            var pos4_4 = Vector3.Lerp(pos3_3, pos3_4, i / pointNumber);
            //六
            var pos5_1 = Vector3.Lerp(pos4_1, pos4_2, i / pointNumber);
            var pos5_2 = Vector3.Lerp(pos4_2, pos4_3, i / pointNumber);
            var pos5_3 = Vector3.Lerp(pos4_3, pos4_4, i / pointNumber);
            //七
            var pos6_1 = Vector3.Lerp(pos5_1, pos5_2, i / pointNumber);
            var pos6_2 = Vector3.Lerp(pos5_2, pos5_3, i / pointNumber);

            Vector3 find = Vector3.Lerp(pos6_1, pos6_2, i / pointNumber);
            point2.Add(find);
        }
    }


    //画线
    //void OnDrawGizmos()
    //{
        //GetPathPoints();
        //Gizmos.color = Color.yellow;
        //for (int i = 0; i < point.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point[i], point[i + 1]);
        //}


        //GameObject pointmgr = GameObject.Find("PointMgr1");
        ////print(pointmgr.transform.childCount);
        //for (int i = 0; i < pointmgr.transform.childCount; i++)
        //{
        //    gameOjbet_tran2.Add(pointmgr.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran2);

        //Gizmos.color = Color.cyan;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr2 = GameObject.Find("PointMgr2");
        //for (int i = 0; i < pointmgr2.transform.childCount; i++)
        //{
        //    gameOjbet_tran3.Add(pointmgr2.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran3);

        //Gizmos.color = Color.white;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr3 = GameObject.Find("PointMgr3");
        //for (int i = 0; i < pointmgr3.transform.childCount; i++)
        //{
        //    gameOjbet_tran4.Add(pointmgr3.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran4);

        //Gizmos.color = Color.black;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr4 = GameObject.Find("PointMgr4");
        //for (int i = 0; i < pointmgr4.transform.childCount; i++)
        //{
        //    gameOjbet_tran5.Add(pointmgr4.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran5);

        //Gizmos.color = Color.red;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr5 = GameObject.Find("PointMgr5");
        //for (int i = 0; i < pointmgr5.transform.childCount; i++)
        //{
        //    gameOjbet_tran6.Add(pointmgr5.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran6);

        //Gizmos.color = Color.blue;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr6 = GameObject.Find("PointMgr6");
        //for (int i = 0; i < pointmgr6.transform.childCount; i++)
        //{
        //    gameOjbet_tran7.Add(pointmgr6.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran7);

        //Gizmos.color = Color.gray;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr7 = GameObject.Find("PointMgr7");
        //for (int i = 0; i < pointmgr7.transform.childCount; i++)
        //{
        //    gameOjbet_tran8.Add(pointmgr7.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran8);

        //Gizmos.color = Color.green;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr8 = GameObject.Find("PointMgr8");
        //for (int i = 0; i < pointmgr8.transform.childCount; i++)
        //{
        //    gameOjbet_tran9.Add(pointmgr8.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran9);

        //Gizmos.color = Color.magenta;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr9 = GameObject.Find("PointMgr9");
        //for (int i = 0; i < pointmgr9.transform.childCount; i++)
        //{
        //    gameOjbet_tran10.Add(pointmgr9.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran10);

        //Gizmos.color = Color.yellow;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr10 = GameObject.Find("PointMgr10");
        //for (int i = 0; i < pointmgr10.transform.childCount; i++)
        //{
        //    gameOjbet_tran11.Add(pointmgr10.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran11);

        //Gizmos.color = Color.cyan;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr11 = GameObject.Find("PointMgr11");
        //for (int i = 0; i < pointmgr11.transform.childCount; i++)
        //{
        //    gameOjbet_tran12.Add(pointmgr11.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran12);

        //Gizmos.color = Color.white;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr12 = GameObject.Find("PointMgr12");
        //for (int i = 0; i < pointmgr12.transform.childCount; i++)
        //{
        //    gameOjbet_tran13.Add(pointmgr12.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran13);

        //Gizmos.color = Color.black;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

        //GameObject pointmgr13 = GameObject.Find("PointMgr13");
        //for (int i = 0; i < pointmgr13.transform.childCount; i++)
        //{
        //    gameOjbet_tran14.Add(pointmgr13.transform.GetChild(i));
        //}
        //GetPathPoints2(gameOjbet_tran14);

        //Gizmos.color = Color.grey;
        //for (int i = 0; i < point2.Count - 1; i++)
        //{
        //    Gizmos.DrawLine(point2[i], point2[i + 1]);
        //}

    //}

    private bool OnLogoStar(int eventId, object arg) //这里就只有一次调用
    {

        InputNumToinitData(0);
        start2end = 1;
        time = 10f;
        index = 0;
        Camera_n[0].transform.parent.GetComponent<AudioSource>().Play();
        return false;
    }


    private IEnumerator GetInterface(string uri)
    {
        //using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        using (WWW www = new WWW(uri))
        {
            //yield return webRequest.SendWebRequest();
            yield return www;
            //if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            if(www.error != null)
            {
                //Debug.LogError(uri + "请求错误：" + webRequest.error);
                Debug.LogError(uri + "请求错误：" + www.error);
                testdata.text = www.error;
            }
            else if (www.isDone)
            {
                //Debug.Log(webRequest.downloadHandler.text);
                //Debug.Log(www.text);
                testdata.text = www.text;
                //保存本地
                //string savePath = Application.streamingAssetsPath + "/" + "horse2.json";
                //File.WriteAllText(savePath, Regex.Unescape(webRequest.downloadHandler.text));
                //读取
                //StreamReader streamReader = new StreamReader(savePath);
                //string str = streamReader.ReadToEnd();
                //json = JsonMapper.ToObject(webRequest.downloadHandler.text);
                json = JsonMapper.ToObject(www.text);
                //streamReader.Close();

                for (int i = 0; i < json.Count; i++)
                {
                    var session = (string)json[i]["session"];
                    var time = (string)json[i]["time"];
                    var level = (string)json[i]["level"];
                    var distance = (string)json[i]["distance"];
                    var ground = (string)json[i]["ground"];
                    var ground_type = (string)json[i]["ground type"];
                    var race_name = (string)json[i]["race name"];

                    JsonData jd = json[i]["winner"];
                    winnerinfos = new Winnerinfo[jd.Count];

                    for (int j = 0; j < jd.Count; j++)
                    {
                        winnerinfos[j] = new Winnerinfo();
                        winnerinfos[j].no = (string)jd[j]["no"];
                        winnerinfos[j].forecast_ranking = (string)jd[j]["forecast ranking"];
                        winnerinfos[j].session_ranking = new int[jd[j]["session ranking"].Count];
                        for (int k = 0; k < jd[j]["session ranking"].Count; k++)
                        {
                            //print(jd[j]["session ranking"][0] + "/"+ jd[j]["session ranking"][1] + "/" + jd[j]["session ranking"][2] + "/" + jd[j]["session ranking"][3] +"/"+ jd[j]["session ranking"][4]);
                            winnerinfos[j].session_ranking[k] = (int)jd[j]["session ranking"][k];
                            //print("認識:" + winnerinfos[j].session_ranking[k]);
                        }
                        winnerinfos[j].model = (string)jd[j]["model"];
                        //print("厅:" + winnerinfo.model);
                        winnerinfos[j].jockey = (string)jd[j]["jockey"];
                        winnerinfos[j].name = (string)jd[j]["name"];
                    }

                    //存入信息
                    HorseData horseData = new HorseData(session, time, level, distance, ground,
                                                        ground_type, race_name, winnerinfos);
                    horseList.Add(horseData);
                }

                //----------------------------以上json读取和保存-------------------------//
                inputField.text = 0.ToString();

                allroleVector3 = new Vector3[Role.Length];
                allroleRota3 = new Quaternion[Role.Length];
                for (int i = 0; i < Role.Length; i++)//首先 全部隐藏起来
                {
                    allroleVector3[i] = Role[Role.Length - i - 1].transform.position; //记录开始坐标
                    allroleRota3[i] = Role[Role.Length - i - 1].transform.rotation;
                    Role[i].SetActive(false);
                }

                InputNumToinitData(0);
            }
        }
    }
    //---------------------------------------------------------------
    List<Transform> gameOjbet_tranX = new List<Transform>();
    public void InputNumToinitData(int num) //再次啟動重要的方法
    {

        for (int i = 1; i < 13; i++)
        {
            GameObject pointmgr = GameObject.Find("PointMgr" + i.ToString()); //第二條開始

            for (int j = 0; j < pointmgr.transform.childCount; j++)
            {
                gameOjbet_tranX.Add(pointmgr.transform.GetChild(j));
            }
            GetPathPoints2(gameOjbet_tranX);
            list_trans.Add(point2);
            list_obj.Add(pointmgr);
            gameOjbet_tranX.Clear();
        }


        inputField.text = num.ToString();
        for (int i = 0; i < Role.Length; i++)//首先 全部隐藏起来
        {
            Role[i].SetActive(false);
            Role[i].GetComponent<AudioSource>().Stop();
        }

        //print("最开始:"+ horseList[num].winnerinfos.Length);
        if (paizi2.transform.GetChild(0).gameObject.GetComponent<FinishMono>() == null)
        {
            paizi2.transform.GetChild(0).gameObject.AddComponent<FinishMono>();
        }
        
        paizi2.transform.GetChild(0).gameObject.GetComponent<FinishMono>().total_num = horseList[num].winnerinfos.Length;

        for (int i = 0; i < horseList[num].winnerinfos.Length; i++)
        {
            Role[i].SetActive(true);
            Role[i].transform.position = allroleVector3[i];
            Role[i].transform.rotation = allroleRota3[i];
            //Role[i].transform = allroleTrans[i];
            //var speed = Random.Range(11, 13);
            if (Role[i].GetComponent<RoleMono>() == null)
            {
                Role[i].AddComponent<RoleMono>();
            }

            //print(horseList[num].winnerinfos[0].session_ranking[0]);

            Role[i].GetComponent<RoleMono>().lineroadmgr = this.gameObject;
            if (i > 0)
            {
                Role[i].GetComponent<RoleMono>().lineroadmgr = list_obj[list_obj.Count -i];
            }
            Role[i].GetComponent<RoleMono>().point = point; //弯路赋值
            if (i > 0)
            {
                Role[i].GetComponent<RoleMono>().point = list_trans[list_trans.Count -i ]; //弯路赋值
            }

            //Role[i].GetComponent<RoleMono>().speed = speed; //速度需要造假
            Role[i].GetComponent<RoleMono>().forecast_ranking = int.Parse(horseList[num].winnerinfos[i].forecast_ranking); //造馬證明

            Role[i].GetComponent<RoleMono>().session_ranking = horseList[num].winnerinfos[i].session_ranking;

            Role[i].GetComponent<RoleMono>().id = int.Parse(horseList[num].winnerinfos[horseList[num].winnerinfos.Length - i - 1].no);

            Role[i].GetComponent<RoleMono>().name = horseList[num].winnerinfos[i].name;
            Role[i].GetComponent<RoleMono>().jockey = horseList[num].winnerinfos[i].jockey;
        }

        //---------------------------------以下初始化碰撞點touchpoint-----------------------------------//

        for (int i = 0; i < Touchpoint.transform.childCount - 1; i++)
        {
            if (Touchpoint.transform.GetChild(i).gameObject.GetComponent<TouchMono>() == null)
            {
                Touchpoint.transform.GetChild(i).gameObject.AddComponent<TouchMono>();
            }
            
            if (horseList[num].winnerinfos.Length > 3)
            {
                Touchpoint.transform.GetChild(i).gameObject.GetComponent<TouchMono>().total = 2;
            }
            else
                Touchpoint.transform.GetChild(i).gameObject.GetComponent<TouchMono>().total = horseList[num].winnerinfos.Length -1;

            Touchpoint.transform.GetChild(i).gameObject.GetComponent<TouchMono>().onlyone = 0;
            Touchpoint.transform.GetChild(i).gameObject.GetComponent<TouchMono>().total_big = horseList[num].winnerinfos.Length;
            Touchpoint.transform.GetChild(i).gameObject.GetComponent<TouchMono>().id = i;

        }

        isFinish = false;
       
    }


    private bool OnStart(int eventId, object arg)
    {
        index = (int)arg;
        finish_vr_time = 0f;
        FinishMgr.SetActive(false);
        sort_num_index = 0;
        horse_index = 0;
        minute = 0; //分
        second = 0;//秒
        Camera_n[0].transform.parent.GetComponent<AudioSource>().Play();

        for (int i = 0; i < Camera_n.Length; i++)
        {
            if (i != 4)
                Camera_n[i].SetActive(false);

        }
        Camera_n[0].SetActive(true);

        for (int i = 0; i < Content.transform.childCount; i++) //先刪除所有終點的item
        {
            Destroy(Content.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < Content2.transform.childCount; i++)//先刪除所有終點的item
        {
            Destroy(Content2.transform.GetChild(i).gameObject);
        }

        //UIContrl.changci_num = int.Parse(inputField.text);
        if (isFinish) //過了初始化才可以選擇場
        {
            start2end = 1;
            time = 10f;
            InputNumToinitData(index);
        }

        return false;
    }

    private bool OnFinish_Last(int eventId, object arg)
    {

        for (int i = 0; i < Role.Length; i++)//首先 全部隐藏起来
        {
            Role[i].GetComponent<AudioSource>().Stop();
        }

        return false;
    }

    private bool OnFinish(int eventId, object arg)
    {
        
        finish_sort_num = (List<string>)arg;
        sort_num_index++;
        isFinish = true;
        

        if (sort_num_index > horseList[index].winnerinfos.Length - 1)
        {
            FinishMgr.SetActive(true);
        }

            if (sort_num_index < 2) //上面的只需要賦值一次
            {
                
                FinishMgr.transform.Find("title_num").GetChild(0).GetComponent<Text>().text = "  用時:" + "00:" + minute.ToString("#0") + ":" + second.ToString("#0.00");
                FinishMgr.transform.Find("changci").GetComponent<Text>().text = horseList[index].session;
                FinishMgr.transform.Find("banci").GetComponent<Text>().text = horseList[index].level;
                FinishMgr.transform.Find("toptime").GetComponent<Text>().text = horseList[index].time;
                FinishMgr.transform.Find("saishi").GetComponent<Text>().text = "賽事: " +horseList[index].race_name;
                FinishMgr.transform.Find("didian").GetComponent<Text>().text = "地點: " + horseList[index].ground;
                FinishMgr.transform.Find("saidao").GetComponent<Text>().text = "賽道: " + horseList[index].distance;
            }

            
            if (sort_num_index < 8)
            {
                //var Content = GameObject.FindWithTag("Conent");
                GameObject item = Instantiate(Resources.Load("item")) as GameObject;
                item.transform.parent = Content.transform;
                item.transform.GetChild(0).GetComponent<Text>().text = sort_num_index.ToString();
                Sprite sprite = Resources.Load(finish_sort_num[1], typeof(Sprite)) as Sprite;
                item.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
                item.transform.GetChild(2).GetComponent<Text>().text = finish_sort_num[0];
                //item.transform.GetChild(3).GetComponent<Text>().text = "00:" + minute.ToString("#0") + ":"+ second.ToString("#0.00");

            }
            else if (sort_num_index > 7)
            {
                //var Content = GameObject.FindWithTag("Conent2");
                GameObject item = Instantiate(Resources.Load("item")) as GameObject;
                item.transform.parent = Content2.transform;
                item.transform.GetChild(0).GetComponent<Text>().text = sort_num_index.ToString();
                Sprite sprite = Resources.Load(finish_sort_num[1], typeof(Sprite)) as Sprite;
                item.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
                item.transform.GetChild(2).GetComponent<Text>().text = finish_sort_num[0];
            //item.transform.GetChild(3).GetComponent<Text>().text = "00:" + minute.ToString("#0") + ":"+ second.ToString("#0.00");
                
            }

            if (sort_num_index == horseList[index].winnerinfos.Length - 2)
            {
                Camera_n[0].transform.parent.GetComponent<AudioSource>().Stop();//关闭声音
            }
            
            finish_sort_num.Clear();
        
        return false;
    }

    private void LateUpdate()
    {
        if (!isFinish)
        {
            if (Camera_n[0] != null && Role[0] != null) //為了拍片
                Camera_n[0].transform.position = new Vector3(Role[horse_index].transform.position.x - 8,
                                                            Camera_n[0].transform.position.y,
                                                             Role[horse_index].transform.position.z + 5);

            if (Camera_n[1] != null && Role[0] != null) //為了拍片
                Camera_n[1].transform.position = new Vector3(Role[horse_index].transform.position.x - 14,
                                                            Camera_n[1].transform.position.y,
                                                             Role[horse_index].transform.position.z);

            if (Camera_n[2] != null && Role[0] != null)//為了拍片
                Camera_n[2].transform.position = new Vector3(Role[horse_index].transform.position.x - 7,
                                                            Camera_n[2].transform.position.y,
                                                             Role[horse_index].transform.position.z - 17);

            if (Camera_n[4] != null && Role[0] != null)//為了拍片
                Camera_n[4].transform.position = new Vector3(Role[horse_index].transform.position.x,
                                                            Camera_n[4].transform.position.y,
                                                             Role[horse_index].transform.position.z + 7);
        }
        else
        {
            for (int i = 0; i < Camera_n.Length; i++)
            {
                if (i != 4)
                    Camera_n[i].SetActive(false);

            }
            Camera_n[4].SetActive(true);
            Camera_n[4].transform.position = new Vector3(108.75f, 12f, -120f);
            Camera_n[4].transform.rotation = Quaternion.Euler(17.4f, 180f, 0);

            finish_vr_time = finish_vr_time + 1 * Time.deltaTime;
            //print("時間:" + finish_vr_time);
            if (finish_vr_time < 1.5f)
            {
                Time.timeScale = 0.2f;
                Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            }
            else
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            }

        }
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (horse_index < horseList[int.Parse(inputField.text)].winnerinfos.Length -1)
            {
                horse_index++;
            }
            else
                horse_index = 0; 
        }

        //控制摄像头
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Cam_index < Camera_n.Length)
            {
                if (Cam_index < Camera_n.Length - 1)
                    Cam_index++;
                else
                    Cam_index = 0;
                if (Cam_index !=0 )
                    Camera_n[Cam_index - 1].SetActive(false);
                else
                    Camera_n[Camera_n.Length - 1].SetActive(false);
            }
            Camera_n[Cam_index].SetActive(true);
        }

        if (start2end == 1) //开始待机状态
        {
            time_text.gameObject.SetActive(true);
            //print("長度:" + horseList[int.Parse(inputField.text)].winnerinfos.Length);
            for (int i = 0; i < horseList[int.Parse(inputField.text)].winnerinfos.Length; i++)
            {
                
                Role[i].GetComponent<RoleMono>().part = 0;
                Role[i].GetComponent<RoleMono>().ma_state = 1;
                var t = door[i].GetComponent<Animation>();
                t.Play("stop");
            }
            start2end = 2;
        }
        else if (start2end == 2) //倒計時
        {

            time = time - 1 * Time.deltaTime;
            time_text.text = time.ToString("#0");
            if (time.ToString("#0").Equals("0"))
            {
                GlobalDispatcher.Instance.Dispatch(GlobalEvent.OnTotal, horseList);
                //Camera_n[0].transform.parent.GetComponent<AudioSource>().Stop();
               
                    for (int i = 0; i < Role.Length; i++)//首先 全部隐藏起来
                    {
                        Role[i].GetComponent<AudioSource>().Play();
                    }
                
                start2end = 3;
            }
        }
        else if (start2end == 3) //开跑
        {
            
            time_text.gameObject.SetActive(false);
            for (int i = 0; i < horseList[int.Parse(inputField.text)].winnerinfos.Length; i++)
            {
                Role[i].GetComponent<RoleMono>().part = 1;
                Role[i].GetComponent<RoleMono>().ma_state = 2;
                var t = door[i].GetComponent<Animation>();
                t.Play("open");
                t["open"].speed = 2;
            }

            start2end = 4;
        }
        else if (start2end == 4)
        {
            //print(minute);
            if (second < 60)
            {
                second = second + 1 * Time.deltaTime;
            }
            else
            {
                minute += 1;
                second = 0;
            }

        }

    }

}

