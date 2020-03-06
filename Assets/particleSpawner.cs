using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSpawner : MonoBehaviour
{
    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 1 == 0)
        {
            particle g = Instantiate(particles).GetComponent<particle>();
        }
    }
}
