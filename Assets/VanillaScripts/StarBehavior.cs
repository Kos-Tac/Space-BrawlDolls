using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehavior : MonoBehaviour
{
    //each star has a mass which is modelised by its volume, because we have only arbitrary units here
    [SerializeField] private float mass;
    //each star has a speed, set at 0 in the beginning
    [SerializeField] private Vector3 speed;
    //each star has an acceleration, set at 0 in the beginning
    [SerializeField] private Vector3 acceleration;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-3000, 3000), Random.Range(-3000, 3000), Random.Range(-3000, 3000));
        float size = Random.Range(1, 100);
        if (size < 45)
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/WhiteStar");
        else if (size < 75)
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/YellowStar");
        else if (size < 89)
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/RedStar");
        else if (size < 96)
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/BLueStar");
        else 
            GetComponent<Renderer>().material = Resources.Load<Material>("Materials/PurpleStar");

        mass = 4 / 3 * Mathf.PI * Mathf.Pow(size, 3);

        transform.localScale *= size;

        speed = new Vector3(0,0,0);

        acceleration = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        speed += acceleration;
        transform.position += speed * Time.deltaTime;
    }

    public float getMass()
    {
        return mass;
    }

    public void setMass(float m)
    {
        mass = m;
    }

    public Vector3 getSpeed()
    {
        return speed;
    }

    public void setSpeed(Vector3 s)
    {
        speed = s;
    }

    public Vector3 getAcceleration()
    {
        return acceleration;
    }

    public void setAcceleration(Vector3 a)
    {
        acceleration = a;
    }

}
