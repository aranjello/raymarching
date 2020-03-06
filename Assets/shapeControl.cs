using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class shapeControl : MonoBehaviour
{
    public enum shapes { cube, sphere , torus, hexPrism, roundCone, triPrism, cappedCylinder, cone, octahedron, pyramid, plane};
    public enum op {none, cut, blend, shapeMask, colorMask};
    public Color c = Color.red;
    public Vector3 rad = new Vector3(0, 0, 0);
    public shapes shapeType = shapes.cube;
    public op operation;
    public int id;
    public bool shell;
    public float roundness;
    private Vector3 currTransform;
    private Vector3 currRotation;
    public bool locked;
    public float blendensity;
    public Vector3 stretch;
    public int parentLevel;
    public bool seeThrough;
    public float refractive;
    public bool repeat;
    public int xRep;
    public int yRep;
    public int zRep;
    public float repLength;

    private void Start()
    {
        currTransform = transform.position;
        currRotation = transform.rotation.eulerAngles;
    }

    public void Update()
    {

        if (transform.hasChanged)
        {
            int i = this.transform.childCount;
            if (!locked)
            {
                Vector3 transChange = currTransform - transform.position;
                currTransform = transform.position;
                for (int j = 0; j < i; j++)
                {
                    transform.GetChild(j).transform.localPosition += transChange;
                }
                transform.hasChanged = false;
            }
            else
            {
                Vector3 RotChange = currRotation - transform.rotation.eulerAngles;
                currRotation = transform.rotation.eulerAngles;
                for (int j = 0; j < i; j++)
                {
                    
                    transform.GetChild(j).transform.localEulerAngles -= RotChange;
                }
                transform.hasChanged = false;
            }
        }
        gameObject.name = shapeType.ToString();
        
    }


}