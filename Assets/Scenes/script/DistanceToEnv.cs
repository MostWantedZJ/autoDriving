using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class DistanceToEnv : MonoBehaviour
{
    
    [Range(3, 41)]
    public int numberOfRays;
    Ray[] rays;
    RaycastHit[] hits;
    public PrometeoCarController controller;
    

    // Start is called before the first frame update
    void Start()
    {
        rays = new Ray[numberOfRays];
        hits = new RaycastHit[numberOfRays];

    }
    private void FixedUpdate()
    {
        getState();
        //int fab = 4;
        //int lar = 5;
        //if (Input.GetKey(KeyCode.W))
        //{
        //    fab = 0;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    fab = 1;
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    lar = 0;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    lar = 2;
        //}
        //controller.setInput(fab, lar);
        //Debug.Log("car speed:" + controller.carSpeed);
    }

    public float[] getState()
    {
        float[] dis = new float[numberOfRays];
        rays[0] = new Ray(transform.position + Vector3.up * 0.5f, transform.TransformDirection(new Vector3(0, 0, 1)));
        //Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.TransformDirection(new Vector3(0, 0, 1)) * 20f, Color.yellow);
        float angle = 0;
        for (int i = 0; i < (numberOfRays - 1) / 2; i++)
        {
            float mid = 1;
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += Mathf.PI / numberOfRays;
            Vector3 dir = new Vector3( x,0, z);
            Vector3 dir2 = new Vector3(-x, 0, z);
            rays[i + 1] = new Ray(transform.position + Vector3.up * 0.5f, transform.TransformDirection(dir));
            rays[numberOfRays / 2 + 1 + i] = new Ray(transform.position + Vector3.up * 0.5f, transform.TransformDirection(dir2));
            //Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.TransformDirection(dir)*20f, Color.yellow);
            //Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.TransformDirection(dir2)*20f, Color.yellow);
        }
        
        for (int i = 0; i < numberOfRays; i++)
        {
            if (Physics.Raycast(rays[i], out hits[i]))
            {
                dis[i] = hits[i].distance;
            }
        }
        return dis;


    }



}
