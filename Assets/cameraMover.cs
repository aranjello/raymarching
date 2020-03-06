using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMover : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 rad;
    public Vector3 lookPoint;
    public Vector3 speed;
    public Animator stopper;
    float timer = 0;
    float time;
    public bool linear;
    public Vector3 endPoint;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopper)
        {
            AnimatorStateInfo anim = stopper.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] animClip = stopper.GetCurrentAnimatorClipInfo(0);
            time = animClip[0].clip.length * anim.normalizedTime;

            timer = Mathf.Max(timer, time);
        }
        else
        {
            timer = Time.frameCount;
        }
            float anglex = 0, angley = 0, anglez = 0;
            if (speed.x != 0)
            {
                anglex = timer/900f*speed.x * Mathf.PI * 2;
            }
            if (speed.y != 0)
            {
                angley = timer/900f*speed.y * Mathf.PI * 2;
            }
            if (speed.z != 0)
            {
                anglez = timer/900f*speed.z * Mathf.PI * 2;
            }

        if (linear)
        {
            float vx = timer / 900f * speed.x * (startPoint.x - endPoint.x);
            float vy = timer / 900f * speed.y *(startPoint.y - endPoint.y);
            float vz = timer / 900f * speed.z * (startPoint.z - endPoint.z);
            transform.position = startPoint - new Vector3(vx, vy, vz);
        }
        else
        {
            transform.localPosition = new Vector3(startPoint.x + Mathf.Sin(anglex) * rad.x, startPoint.y + Mathf.Sin(angley) * rad.y, startPoint.z + Mathf.Cos(anglez) * rad.z);
            transform.LookAt(lookPoint);
        }
            if (time >= 30)
            {
                if (UnityEditor.EditorApplication.isPlaying)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
        }
    
}
