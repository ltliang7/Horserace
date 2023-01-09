using UnityEngine;
using System.Collections.Generic;
using Game;

public class RoleMono : MonoBehaviour
{
    public GameObject lineroadmgr;
    public int id; //唯 一值
    public int num; //第几场
    public string Rolename;
    public string jockey;
    public int part;
    public int ma_state;
    public int forecast_ranking; //造假跑馬的最終排名
    public int[] session_ranking;
    private Animation ani;
    private Animation animation2;
    private int part_one_index;
    public float speed;
    //private float Xspeed;
    public List<Vector3> point;
    //public List<Vector3> point2;
    private int point_index;
    private float move_offset;

    //private float time;
    //private float Timer;

    private int section; //看馬跑到第幾階段
    private bool IsSection;

    private float speed_1;
    // Start is called before the first frame update
    void Start()
    {
        var jockey = transform.GetChild(1).GetChild(0); //骑师
        var horse = transform.GetChild(0).GetChild(0); //马匹
        ani = jockey.GetComponent<Animation>();
        animation2 = horse.GetComponent<Animation>();
        part_one_index = 0;
        point_index = 0;
        part = 0;
        //move_offset = Random.Range(0, 3);
        //move_offset = id;
        move_offset = 0;
        GlobalDispatcher.Instance.AddListener(GlobalEvent.OnSection, OnSection);
        section = 0;
        IsSection = true;
        speed_1 = Random.Range(0.5f,2.5f);
       
    }

    private bool OnSection(int eventId, object arg)
    {

        section = (int)arg;
        IsSection = true;
        return false;
    }

    private float angle = 0;
    private float ascent = 0;
    private float anispeed;
    // Update is called once per frame
    void Update()
    {

        //赛前准备
        if (ma_state == 1)
        {
            //print(speed_1);
            ani.Play("Jockey_Idle01");
            ani["Jockey_Idle01"].speed = speed_1;
            animation2.Play("Horse_Idle01");
            animation2["Horse_Idle02"].speed = speed_1;
            part_one_index = 0;
            point_index = 0;
            anispeed = Random.Range(3.5f, 4.2f);
        }

        //if (IsSection)
        //{
        //    if (section == 0)
        //    {
        //        //print("過了第一階段了?");
        //        if (session_ranking[0] == 1)
        //        {
        //            speed = 12.7f;
        //        }
        //        if (session_ranking[0] == 2)
        //        {
        //            speed = 12.66f;
        //        }
        //        if (session_ranking[0] == 3)
        //        {
        //            speed = 12.64f;
        //        }
        //        if (session_ranking[0] == 4)
        //        {
        //            speed = 12.60f;
        //        }
        //        if (session_ranking[0] == 5)
        //        {
        //            speed = 12.56f;
        //        }
        //        if (session_ranking[0] == 6)
        //        {
        //            speed = 12.52f;
        //        }
        //        if (session_ranking[0] == 7)
        //        {
        //            speed = 12.48f;
        //        }
        //        if (session_ranking[0] == 8)
        //        {
        //            speed = 12.44f;
        //        }
        //        if (session_ranking[0] == 9)
        //        {
        //            speed = 12.40f;
        //        }
        //        if (session_ranking[0] == 10)
        //        {
        //            speed = 12.36f;
        //        }
        //        if (session_ranking[0] == 11)
        //        {
        //            speed = 12.32f;
        //        }
        //        if (session_ranking[0] == 12)
        //        {
        //            speed = 12.28f;
        //        }
        //        if (session_ranking[0] == 13)
        //        {
        //            speed = 12.24f;
        //        }
        //        if (session_ranking[0] == 14)
        //        {
        //            speed = 12.20f;
        //        }
        //    }
        //    if (section == 1)
        //    {
        //        //print("到第二段");
        //        if (session_ranking[1] == 1)
        //        {
        //            speed = 12.7f;
        //        }
        //        if (session_ranking[1] == 2)
        //        {
        //            speed = 12.69f;
        //        }
        //        if (session_ranking[1] == 3)
        //        {
        //            speed = 12.68f;
        //        }
        //        if (session_ranking[1] == 4)
        //        {
        //            speed = 12.67f;
        //        }
        //        if (session_ranking[1] == 5)
        //        {
        //            speed = 12.66f;
        //        }
        //        if (session_ranking[1] == 6)
        //        {
        //            speed = 12.65f;
        //        }
        //        if (session_ranking[1] == 7)
        //        {
        //            speed = 12.64f;
        //        }
        //        if (session_ranking[1] == 8)
        //        {
        //            speed = 12.63f;
        //        }
        //        if (session_ranking[1] == 9)
        //        {
        //            speed = 12.62f;
        //        }
        //        if (session_ranking[1] == 10)
        //        {
        //            speed = 12.61f;
        //        }
        //        if (session_ranking[1] == 11)
        //        {
        //            speed = 12.60f;
        //        }
        //        if (session_ranking[1] == 12)
        //        {
        //            speed = 12.59f;
        //        }
        //        if (session_ranking[1] == 13)
        //        {
        //            speed = 12.58f;
        //        }
        //        if (session_ranking[1] == 14)
        //        {
        //            speed = 12.57f;
        //        }
        //    }
        //    if (section == 2)
        //    {
        //        //print("到第三段");
        //        if (session_ranking[2] == 1)
        //        {
        //            anispeed = 4.6f;
        //            speed = 14f;
        //        }
        //        if (session_ranking[2] == 2)
        //        {
        //            anispeed = 4.3f;
        //            speed = 13.6f;
        //        }
        //        if (session_ranking[2] == 3)
        //        {
        //            anispeed = 4.2f;
        //            speed = 12.5f;
        //        }
        //        if (session_ranking[2] == 4)
        //        {
        //            anispeed = 4.1f;
        //            speed = 11.4f;
        //        }
        //        if (session_ranking[2] == 5)
        //        {
        //            anispeed = 4f;
        //            speed = 11.3f;
        //        }
        //        if (session_ranking[2] == 6)
        //        {
        //            speed = 11.2f;
        //        }
        //        if (session_ranking[2] == 7)
        //        {
        //            speed = 11.1f;
        //        }
        //        if (session_ranking[2] == 8)
        //        {
        //            speed = 11f;
        //        }
        //        if (session_ranking[2] == 9)
        //        {
        //            speed = 10.9f;
        //        }
        //        if (session_ranking[2] == 10)
        //        {
        //            speed = 10.8f;
        //        }
        //        if (session_ranking[2] == 11)
        //        {
        //            speed = 10.7f;
        //        }
        //        if (session_ranking[2] == 12)
        //        {
        //            speed = 10.6f;
        //        }
        //        if (session_ranking[2] == 13)
        //        {
        //            speed = 10.5f;
        //        }
        //        if (session_ranking[2] == 14)
        //        {
        //            speed = 10.4f;
        //        }
        //    }
            
        //    IsSection = false;
        //}

        if (IsSection) //拍片需要做假的改動
        {
            if (section == 0)
            {
                //print("過了第一階段了?");
                if (session_ranking[0] == 1)
                {
                    speed = 13.7f;
                }
                if (session_ranking[0] == 2)
                {
                    speed = 13.66f;
                }
                if (session_ranking[0] == 3)
                {
                    speed = 12.64f;
                }
                if (session_ranking[0] == 4)
                {
                    speed = 12.60f;
                }
                if (session_ranking[0] == 5)
                {
                    speed = 12.56f;
                }
                if (session_ranking[0] == 6)
                {
                    speed = 12.52f;
                }
                if (session_ranking[0] == 7)
                {
                    speed = 12.48f;
                }
                if (session_ranking[0] == 8)
                {
                    speed = 12.44f;
                }
                if (session_ranking[0] == 9)
                {
                    speed = 12.40f;
                }
                if (session_ranking[0] == 10)
                {
                    speed = 12.36f;
                }
                if (session_ranking[0] == 11)
                {
                    speed = 12.32f;
                }
                if (session_ranking[0] == 12)
                {
                    speed = 12.28f;
                }
                if (session_ranking[0] == 13)
                {
                    speed = 12.24f;
                }
                if (session_ranking[0] == 14)
                {
                    speed = 12.20f;
                }
            }
            if (section == 1)
            {
                //print("到第二段");
                if (session_ranking[1] == 1)
                {
                    speed = 13.7f;
                }
                if (session_ranking[1] == 2)
                {
                    speed = 13.69f;
                }
                if (session_ranking[1] == 3)
                {
                    speed = 13.68f;
                }
                if (session_ranking[1] == 4)
                {
                    speed = 12.67f;
                }
                if (session_ranking[1] == 5)
                {
                    speed = 12.66f;
                }
                if (session_ranking[1] == 6)
                {
                    speed = 12.65f;
                }
                if (session_ranking[1] == 7)
                {
                    speed = 12.64f;
                }
                if (session_ranking[1] == 8)
                {
                    speed = 12.63f;
                }
                if (session_ranking[1] == 9)
                {
                    speed = 12.62f;
                }
                if (session_ranking[1] == 10)
                {
                    speed = 12.61f;
                }
                if (session_ranking[1] == 11)
                {
                    speed = 12.60f;
                }
                if (session_ranking[1] == 12)
                {
                    speed = 12.59f;
                }
                if (session_ranking[1] == 13)
                {
                    speed = 12.58f;
                }
                if (session_ranking[1] == 14)
                {
                    speed = 12.57f;
                }
            }
            if (section == 2)
            {
                //print("到第三段");
                if (session_ranking[2] == 1)
                {
                    anispeed = 4.6f;
                    speed = 13.8f;
                }
                if (session_ranking[2] == 2)
                {
                    anispeed = 4.3f;
                    speed = 13.6f;
                }
                if (session_ranking[2] == 3)
                {
                    anispeed = 4.2f;
                    speed = 12.9f;
                }
                if (session_ranking[2] == 4)
                {
                    anispeed = 4.1f;
                    speed = 12.4f;
                }
                if (session_ranking[2] == 5)
                {
                    anispeed = 4f;
                    speed = 12.3f;
                }
                if (session_ranking[2] == 6)
                {
                    speed = 12.2f;
                }
                if (session_ranking[2] == 7)
                {
                    speed = 12.1f;
                }
                if (session_ranking[2] == 8)
                {
                    speed = 12f;
                }
                if (session_ranking[2] == 9)
                {
                    speed = 10.9f;
                }
                if (session_ranking[2] == 10)
                {
                    speed = 10.8f;
                }
                if (session_ranking[2] == 11)
                {
                    speed = 10.7f;
                }
                if (session_ranking[2] == 12)
                {
                    speed = 10.6f;
                }
                if (session_ranking[2] == 13)
                {
                    speed = 10.5f;
                }
                if (session_ranking[2] == 14)
                {
                    speed = 10.4f;
                }
            }

            IsSection = false;
        }


        if (part == 1)
        {
            ma_state = 0;
            //print("能力:" +_pointObj[0].transform.position);
            //time = time + 1 * Time.deltaTime;
            //Timer = Random.Range(1, 1);
            //var rspeed = Random.Range(0.01f, 0.03f);
            ////print(rspeed);
            //if ((int)time == Timer)
            //{
            //    //print("时刻到了");
            //    speed += rspeed;
            //}
            
            //print("速度:" + speed);
            ani.Play("Jockey_SpeedRun");
            ani["Jockey_SpeedRun"].speed = anispeed;
            animation2.Play("Horse_SpeedRun");
            animation2["Horse_SpeedRun"].speed = anispeed;

            //开跑点
            if (transform.position !=
                //lineroadmgr.transform.GetChild(part_one_index).position)
                new Vector3(lineroadmgr.transform.GetChild(part_one_index).position.x + move_offset,
                lineroadmgr.transform.GetChild(part_one_index).position.y,
                lineroadmgr.transform.GetChild(part_one_index).position.z + move_offset))
            //lineroadmgr.transform.GetChild(part_one_index).position)
            {
                transform.LookAt(
                    //lineroadmgr.transform.GetChild(part_one_index).position
                    new Vector3(lineroadmgr.transform.GetChild(part_one_index).position.x + move_offset,
                lineroadmgr.transform.GetChild(part_one_index).position.y,
                lineroadmgr.transform.GetChild(part_one_index).position.z + move_offset));

                transform.position = Vector3.MoveTowards(
                    //lineroadmgr.transform.GetChild(part_one_index).position,
                    transform.position, new Vector3(lineroadmgr.transform.GetChild(part_one_index).position.x + move_offset,
                lineroadmgr.transform.GetChild(part_one_index).position.y,
                lineroadmgr.transform.GetChild(part_one_index).position.z + move_offset),
                    speed * Time.deltaTime);
            }
            else
                part_one_index++;

            if (part_one_index > 0)
            {
                //speed = Random.Range(12, 14);
                part = 2; //跑第二段
            }

        }
        if (part == 2) //第一段转弯
        {
            //time = time + 1 * Time.deltaTime;
            //Timer = Random.Range(5, 8);
            
            ani.Play("Jockey_SpeedRun");
            ani["Jockey_SpeedRun"].speed = anispeed;
            animation2.Play("Horse_SpeedRun");
            animation2["Horse_SpeedRun"].speed = anispeed;


            //if (point_index < 45)
            //{
            //    angle += 0.06f;
            //    if (ascent < 8) //身體傾斜
            //    {
            //        ascent += 0.1f;
            //    }

            //    print(angle);
            //    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y - 90 - angle, transform.rotation.z + ascent );
            //}
            //else
            //{
            //    if (ascent > 0)
            //    {
            //        ascent -= 0.1f;
            //    }

            //    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y - 90 - angle, transform.rotation.z + ascent );

            //}

                //接第一个大弯点
                if (transform.position != new Vector3(point[point_index + 1].x - move_offset , point[point_index + 1].y, point[point_index + 1].z + move_offset ))
                {
                    transform.LookAt(new Vector3(point[point_index + 1].x - move_offset , point[point_index + 1].y, point[point_index + 1].z + move_offset));
                    transform.position = Vector3.MoveTowards(transform.position,
                                                                new Vector3(point[point_index + 1].x - move_offset ,
                                                                point[point_index + 1].y,
                                                                point[point_index + 1].z + move_offset),
                                                                speed * Time.deltaTime);
                    //print("时速:" +id +"//"+speed);

                }
                else
                {
                    point_index++;
                }
           

            if (point_index > 69)
            {
                print("有沒有觸發");
                part = 6; //跑第二段
            }


        }
        //else if (part == 3)
        //{
        //    point_index = 0; 
        //    speed = Random.Range(11, 14);
        //    ani.Play("Jockey_SpeedRun");
        //    ani["Jockey_SpeedRun"].speed = 3;
        //    animation2.Play("Horse_SpeedRun");
        //    animation2["Horse_SpeedRun"].speed = 3;

        //    //接第二个直路
        //    if (transform.position != tworoadmgr.transform.GetChild(part_one_index).position)
        //    {
        //        transform.LookAt(tworoadmgr.transform.GetChild(part_one_index));
        //        transform.position = Vector3.MoveTowards(transform.position, tworoadmgr.transform.GetChild(part_one_index).position, speed * Time.deltaTime);

        //    }
        //    else
        //        part_one_index++;

        //    if (part_one_index > 1)
        //    {
        //        part = 4; //跑第四段
        //    }
        //}
        //else if (part == 4)//第二个大弯
        //{
        //    part_one_index = 0;
        //    speed = Random.Range(12, 14);
        //    ani.Play("Jockey_SpeedRun");
        //    ani["Jockey_SpeedRun"].speed = 3;
        //    animation2.Play("Horse_SpeedRun");
        //    animation2["Horse_SpeedRun"].speed = 3;

        //    if (transform.position != new Vector3(point2[point_index + 1].x + move_offset2, point2[point_index + 1].y, point2[point_index + 1].z + move_offset2))
        //    {
        //        transform.LookAt(new Vector3(point2[point_index + 1].x + move_offset2, point2[point_index + 1].y, point2[point_index + 1].z + move_offset2));
        //        transform.position = Vector3.MoveTowards(transform.position, new Vector3(point2[point_index + 1].x + move_offset2, point2[point_index + 1].y, point2[point_index + 1].z + move_offset2), speed * Time.deltaTime);

        //    }
        //    else
        //    {
        //        point_index++;
        //    }

        //    if (point_index > 49)
        //    {
        //        part = 5; //跑第二段
        //    }

        //}
        //else if (part == 5) //最后直路
        //{
        //    point_index = 0;
        //    speed = Random.Range(8, 11);
        //    ani.Play("Jockey_SpeedRun");
        //    ani["Jockey_SpeedRun"].speed = 3;
        //    animation2.Play("Horse_SpeedRun");
        //    animation2["Horse_SpeedRun"].speed = 3;


        //    if (transform.position != threeroadmgr.transform.GetChild(part_one_index).position)
        //    {
        //        transform.LookAt(threeroadmgr.transform.GetChild(part_one_index));
        //        transform.position = Vector3.MoveTowards(transform.position, threeroadmgr.transform.GetChild(part_one_index).position, speed * Time.deltaTime);

        //    }
        //    else
        //        part_one_index++;

        //    if (part_one_index > 2)
        //    {
        //        part = 6; 
        //    }
        //}
        else if (part == 6) //final
        {
            angle = 0;
            ascent = 0;
            part_one_index = 0;
            //DestroyImmediate(this.gameObject);
            ani.Play("Jockey_Idle01");
            ani["Jockey_Idle01"].speed = 1;
            animation2.Play("Horse_Idle01");
            animation2["Horse_Idle02"].speed = 1;
            //transform.position = threeroadmgr.transform.GetChild(part_one_index).position;
            transform.position = new Vector3(point[point_index].x + move_offset, point[point_index].y, point[point_index].z + move_offset);
            //transform.position = new Vector3(transform.position.x,transform.position.y, transform.position.z);
        }
    }
}
