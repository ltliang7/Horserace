
using UnityEngine;

public class CarMono : MonoBehaviour
{
    private int point_index;
    public GameObject RoadMgr;
    private int speed;
    public int speeda, speedb;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speeda, speedb);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != RoadMgr.transform.GetChild(point_index + 1).transform.position)
        {

            transform.LookAt(RoadMgr.transform.GetChild(point_index + 1).transform);
            transform.position = Vector3.MoveTowards(transform.position, RoadMgr.transform.GetChild(point_index + 1)
                .transform.position, speed * Time.deltaTime);
        }
        else
        {
            point_index++;
        }

        if (transform.position == RoadMgr.transform.GetChild(RoadMgr.transform.childCount - 1).transform.position)
        {
            Destroy(this.gameObject);
        }
    }
}
