using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{

    public int starCount;
    private GameObject[] stars;
    [SerializeField] private float g;

    // Start is called before the first frame update
    void Start()
    {
        stars = new GameObject[starCount];
        GameObject star = Resources.Load<GameObject>("Prefabs/Star");
        for(int i = 0; i<starCount; i++)
        {
            GameObject instance = Instantiate(star) as GameObject;
            instance.name = "Star" + i;
            stars[i] = instance;
        }
    }

    public GameObject[] getStars()
    {
        return stars;
    }

    public void Update()
    {
        for (int i = 0; i < starCount; i++)
        {
            Vector3 sumForces = new Vector3(0,0,0);
            Vector3 direction = new Vector3(0,0,0);
            float distance = 0;
            StarBehavior thisStar = stars[i].GetComponent<StarBehavior>();

            for (int j = 0; j < starCount; j++)
            {
                if (i!= j)
                {
                    direction = Vector3.Normalize(stars[j].transform.position - stars[i].transform.position);
                    distance = Vector3.Distance(stars[i].transform.position, stars[j].transform.position);
                    sumForces += ((g * thisStar.getMass() * stars[j].GetComponent<StarBehavior>().getMass()) / (distance * distance)) * direction;
                    print(thisStar.getMass());
                    print("test");
                }
            }
            thisStar.setAcceleration(sumForces);
        }
    }
}
