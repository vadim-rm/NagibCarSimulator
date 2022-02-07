using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosEncoder = RosMessageTypes.NagibDemo.EncoderMsg;
using System;
using System.IO;
using System.Text;

public class EncoderController : MonoBehaviour
{
    ROSConnection ros;

    public GameObject rightWheel;
    public GameObject leftWheel;
    // Publish the cube's position and rotation every N seconds

    private string topicName = "encoder";

    private float publishMessageFrequency = 0.033f;

    private int leftEncoderValue = 0;
    private int rightEncoderValue = 0;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    private RenderTexture renderTexture;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<RosEncoder>(topicName);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            // Calculate wheels revs per update 
            float rightWheelColliderRPU = rightWheel.GetComponent<WheelCollider>().rpm / (60f * 30f);
            float leftWheelColliderRPU = leftWheel.GetComponent<WheelCollider>().rpm / (60f * 30f);

            // 30 marks on one rotation
            rightEncoderValue += (int)(rightWheelColliderRPU * 30);
            leftEncoderValue += (int)(leftWheelColliderRPU * 30);

            RosEncoder rosEncoder = new RosEncoder(leftEncoderValue, rightEncoderValue);
            ros.Publish(topicName, rosEncoder);
            timeElapsed = 0;
        }
    }
}
