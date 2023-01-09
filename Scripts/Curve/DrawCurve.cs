using UnityEngine;

public class DrawCurve : MonoBehaviour
{
	public float ymod;  //yÖáÆ«ÒÆÏµÊı
	
	void Start()
	{
	}

	void Update()
	{
        float x1 = 0, y1 = 0,
              x2 = 0, y2 = 0;
        for (int i = 0; i < 200; i++)
        {
            if (i < 100)
            {
                x1 = i;
                y1 = x1 * x1 * ymod;

                x2 = i + 1;
                y2 = x2 * x2 * ymod;
            }
            else
            {

                x1 = -i;
                y1 = x1 * x1 * ymod;

                x2 = (i - 1) * -1;
                y2 = x2 * x2 * ymod;

            }
            Debug.DrawLine(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0), Color.red);
        }

	}
}
