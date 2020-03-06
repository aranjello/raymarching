using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour
{
    public float MaxLife;
    private float time;
    public float vx, vy, vz;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        MaxLife = Random.Range(5, 5);
        vx = Random.Range(-.1f, .1f);
        vy = Random.Range(.1f, .2f);
        vz = Random.Range(-.1f, .1f);
        this.gameObject.GetComponent<shapeControl>().c = Random.ColorHSV();
        this.gameObject.GetComponent<shapeControl>().c.a = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        vy -= .01f;
        this.transform.position += new Vector3(vx,vy,vz);

        time += .1f;
        if (time > MaxLife)
        {
            Destroy(this.gameObject);
        }
    }
}
