using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class rayMarchControl : MonoBehaviour
{
    public ComputeShader raymarching;

    RenderTexture target;
    Camera cam;
    Light lightSource;
    List<ComputeBuffer> buffersToDispose;
    public Texture Skybox;
    private ComputeBuffer _sphereBuffer;
    private ComputeBuffer _lightBuff;
    private List<shapeControl> s;
    private Light[] lightSources;
    [Range(1, 10)]public int numReflect;
    [Range(100,5000)]public float drawDist;
    [Range(1,4)]public int AA;
    struct Sphere
    {
        public Vector3 position;
        public Vector3 radius;
        public Vector3 rot;
        public int shape;
        public int op;
        public Vector4 albedo;
        public int comboId;
        public int shell;
        public float roundness;
        public int numChildren;
        public int hasParent;
        public float blendtensity;
        public Vector3 scale;
        public Vector3 stretch;
        public int parentlevel;
        public int seeThrough;
        public float refrac;
        public int repete;
        public int xRep;
        public int yRep;
        public int zRep;
        public float repLength;
    };

    struct Lit
    {
        public Vector3 pos;
        public Vector3 forward;
        public Vector3 color;
        public float intensity;
        public float directional;
        public float maxDist;
        public float hardShad;
    };
    void Init()
    {
        
        cam = Camera.current;
        lightSource = FindObjectOfType<Light>();
        lightSources = FindObjectsOfType<Light>();
        List<Lit> lights = new List<Lit>();
        foreach(Light l in lightSources)
        {
            Lit newL = new Lit();
            newL.pos = l.transform.position;
            newL.forward = l.transform.forward;
            newL.color = new Vector3(l.color.r,l.color.g,l.color.b);
            newL.intensity = l.intensity;
            newL.directional = (l.type == LightType.Directional)?1:0;
            newL.maxDist = l.range;
            newL.hardShad = (float)l.shadows;
            lights.Add(newL);
        }
        _lightBuff = new ComputeBuffer(lights.Count, 52);
        _lightBuff.SetData(lights);
        s = new List<shapeControl>(FindObjectsOfType<shapeControl>());
        s.Sort((a, b) => a.operation.CompareTo(b.operation));
        List<Sphere> spheres = new List<Sphere>();
        int x = 1;
        foreach (shapeControl sh in s)
        {
            Sphere sphere = new Sphere();
            sphere.radius = sh.rad;
            sphere.position = sh.transform.position;
            sphere.albedo = sh.c;
            sphere.rot = sh.transform.localRotation.eulerAngles;
            sphere.shape = (int)sh.shapeType;
            sphere.op = (int)sh.operation;
            if (sh.transform.parent)
            {
                sh.id = sh.transform.parent.GetComponent<shapeControl>().id; ;
                sphere.comboId = sh.transform.parent.GetComponent<shapeControl>().id;
                sphere.hasParent = 1;
                sh.parentLevel = sh.transform.parent.GetComponent<shapeControl>().parentLevel + 1;
                sphere.parentlevel = sh.transform.parent.GetComponent<shapeControl>().parentLevel + 1;


            }
            else
            {
                if (sh.transform.childCount > 0)
                {
                    sh.parentLevel = 0;
                    sphere.parentlevel = 0;
                }
                sphere.hasParent = 0;
                sh.id = x;
                sphere.comboId = x;
                x++;
            }
            sphere.numChildren = sh.transform.childCount;
            sphere.shell = sh.shell ? 1 : 0;
            sphere.roundness = sh.roundness;
            sphere.blendtensity = sh.blendensity;
            sphere.scale = sh.transform.localScale;
            sphere.stretch = sh.stretch;
            if (sh.seeThrough)
            {
                sphere.roundness = .01f;
                sphere.seeThrough = 1;
                sphere.shell = 1;
            }
            else
            {
                sphere.roundness = sh.roundness;
                sphere.seeThrough = 0;
                sphere.shell = sh.shell ? 1 : 0;
            }
            sphere.refrac = sh.refractive;
            sphere.repete = sh.repeat ? 1 : 0;
            sphere.xRep = sh.xRep;
            sphere.yRep = sh.yRep;
            sphere.zRep = sh.zRep;
            sphere.repLength = sh.repLength;
            spheres.Add(sphere);
        }



        _sphereBuffer = new ComputeBuffer(spheres.Count, 140);
        _sphereBuffer.SetData(spheres);
    }


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Init();
        buffersToDispose = new List<ComputeBuffer>();

        InitRenderTexture();
        CreateScene();
        SetParameters();

        raymarching.SetTexture(0, "Source", source);
        raymarching.SetTexture(0, "Destination", target);

        int threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 8.0f);
        raymarching.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(target, destination);

        foreach (var buffer in buffersToDispose)
        {
            buffer.Dispose();
        }
    }

    void CreateScene(){
        
    }

    private void Update()
    {
        //SetUpScene();
            
    }

    void SetParameters()
    {
        raymarching.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        raymarching.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);
        raymarching.SetTexture(0,"_SkyboxTexture", Skybox);
        raymarching.SetBuffer(0, "_Shapes", _sphereBuffer);
        raymarching.SetBuffer(0, "_Lights", _lightBuff);
        raymarching.SetInt("Reflections", numReflect);
        raymarching.SetFloat("MAX_DIST", drawDist);
        raymarching.SetInt("AA", AA);
    }

    void InitRenderTexture()
    {
        if (target == null || target.width != cam.pixelWidth || target.height != cam.pixelHeight)
        {
            if (target != null)
            {
                target.Release();
            }
            target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }

    private void OnDisable()
    {
        if (_sphereBuffer != null)
            _sphereBuffer.Release();
    }

}
