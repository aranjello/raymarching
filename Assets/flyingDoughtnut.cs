using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingDoughtnut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(Mathf.Sin(Time.time)*10,Mathf.Cos(Time.time)*10,0);
    }
}
