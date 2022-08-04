using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{

    public GameObject car;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        car.GetComponent<AgentCar>().HitOnWall();
    }
}
