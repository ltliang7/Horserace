using UnityEngine;

public class RoadContrler : MonoBehaviour
{
    //public GameObject loadmgr;
    public bool isopen;
    public int settime;
    public int startime;

    public int speed_a, speed_b;
    private GameObject cars;
    private string[] carname = {"car_002", "car_007", "car_011" , "car_016", "car_019", "car_020"};
    private int rand_index;
    private int random_indexroad;
    private GameObject[] Roadmgr;
    private int timer;
    public float shader_index;
    public float nomal_index;
    
    //public GameObject Bulider;
    public Material new_material;
    public Material old_material;

    public GameObject[] builders;
    // Start is called before the first frame update
    void Start()
    {
        Roadmgr = new GameObject[3];
        Roadmgr[0] = Instantiate(Resources.Load("LoadMgr1") as GameObject);
        Roadmgr[1] = Instantiate(Resources.Load("LoadMgr2") as GameObject);
        Roadmgr[2] = Instantiate(Resources.Load("LoadMgr3") as GameObject);

        //Shader shader = Shader.Find("Shader Graphs/Cut3");
        //Material material = new Material(shader);

        for (int i = 0; i < builders.Length; i++)
        {
            for (int j = 0; j < builders[i].transform.childCount; j++)
            {
                builders[i].transform.GetChild(j).GetComponent<MeshRenderer>().material = new_material;
            }
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        //new_material.SetVector("Vector3_69a23705437b46298c8639dd9f0fc838", new Vector4(0, shader_index, 0, 0));
        //new_material.SetVector("Vector3_5e9098d227e1493f9cb487d767b00e58", new Vector4(0, nomal_index, 0, 0));
        //old_material.SetVector("Vector3_69a23705437b46298c8639dd9f0fc838", new Vector4(0, shader_index, 0, 0));
        //old_material.SetVector("Vector3_5e9098d227e1493f9cb487d767b00e58", new Vector4(0, nomal_index * -1, 0, 0));

        if (timer < startime + settime)
        {
            timer++;
        }
        else
        {
            if (isopen)
            {
                rand_index = Random.Range(0, 6);
                random_indexroad = Random.Range(0, 3);
                cars = Instantiate(Resources.Load(carname[rand_index]) as GameObject);
                cars.transform.localPosition = Roadmgr[random_indexroad].transform.GetChild(0).transform.position;
                cars.AddComponent<CarMono>();
                cars.GetComponent<CarMono>().RoadMgr = Roadmgr[random_indexroad];
                cars.GetComponent<CarMono>().speeda = speed_a;
                cars.GetComponent<CarMono>().speedb = speed_b;
                //carcount++;
                timer = 0;
            }

        }

    }
}
