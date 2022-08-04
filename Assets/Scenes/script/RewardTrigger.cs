using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardTrigger : MonoBehaviour
{

    public GameObject car;

    private void Start()
    {
     
    }

    public void OnTriggerEnter(Collider other)
    {
        car.GetComponent<AgentCar>().hitOnTrigger(this);
    }



}
