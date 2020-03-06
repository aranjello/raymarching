using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorChanger : MonoBehaviour
{
    private shapeControl shape;
    Color originalCol;
    public Color newCol;
    int time = 0;
    bool forward = true;
    // Start is called before the first frame update
    void Start()
    {
        shape = GetComponent<shapeControl>();
        originalCol = shape.c;
    }

    // Update is called once per frame
    void Update()
    {
        Color changeCol = Color.Lerp(originalCol, newCol, time / 450f);
        shape.c = changeCol;
        time += (forward) ? 1 : -1;
        if (time > 450) forward = false;
    }
}
