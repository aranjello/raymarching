using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ySinMove : MonoBehaviour
{
    float start;
    public float period = 1;
    // Start is called before the first frame update
    void Start()
    {
        start = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x,start+Mathf.Sin(Time.time/period)*start+5, this.transform.position.z);
    }
}
