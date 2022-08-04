using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentCar : Agent
{
    
    public PrometeoCarController controller;
    public List<RewardTrigger> rewardTriggers;
    public Transform rewardTriggerParent;
    public int nextRewardTriggerIndex;
    private Vector3 originCarPosition;
    private Quaternion originRotation;
    private float lastGap = float.PositiveInfinity;
    private float lastTriggerTime = 0;
    float time = 0;
    string path = "D:\\devTools\\unityWorkspace\\autoDrive\\ml_learning\\Assets\\Scenes\\data\\rewards\\newTrackReward100obstacles.txt";

    float PreSpeed;

    private float episodeReward;
    private int episode;


    // Start is called before the first frame update
    void Start()
    {
        originCarPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        originRotation = new Quaternion(gameObject.transform.localRotation.x, gameObject.transform.localRotation.y, gameObject.transform.localRotation.z, gameObject.transform.localRotation.w);
        episodeReward = 0f;
        episode = 0;
    }

    private void FixedUpdate()
    {
    
    }




    public override void OnEpisodeBegin()
    {
        
        rewardTriggers.Clear();
        RewardTrigger[] rewardTriggerstemp = rewardTriggerParent.GetComponentsInChildren<RewardTrigger>();
        foreach (RewardTrigger rewardTrigger in rewardTriggerstemp)
        {
            rewardTriggers.Add(rewardTrigger);
        }
        nextRewardTriggerIndex = 0;
        controller.Stop();
        this.transform.localPosition = originCarPosition;
        this.transform.localRotation = originRotation;

        if (episode < 140)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(episodeReward);
                }
            }
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(episodeReward);
            }
        }
        else
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        episodeReward = 0f;
        episode++;
    }




    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.GetComponent<DistanceToEnv>().getState());
        sensor.AddObservation(controller.carSpeed);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        controller.setInput(actions.DiscreteActions[0], actions.DiscreteActions[1]);
    }

    public void hitOnTrigger(RewardTrigger rewardTrigger)
    {
        if(rewardTriggers.IndexOf(rewardTrigger) == (nextRewardTriggerIndex%rewardTriggers.Count))
        {
            HitOnRewardTrigger();
            nextRewardTriggerIndex++;
        }
        else
        {
            HitOnWrongTrigger();
        }
        
    }


    public void HitOnRewardTrigger()
    {
        
        float currentTime = Time.fixedTime;
        float gap = currentTime - lastTriggerTime;
        lastTriggerTime = currentTime;
        if(lastGap > gap)
        {
            AddReward(+0.3f);
        }
        lastGap = gap;
        AddReward(+0.6f);
        if(episode < 140)
        {
            episodeReward+=1f;
        }


    }




    public void HitOnWrongTrigger()
    {
        AddReward(-1f);
    }

    public void HitOnWall()
    {
        AddReward(-1f);
        EndEpisode();
    }

}
