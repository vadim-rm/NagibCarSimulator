using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosControl = RosMessageTypes.NagibDemo.ControlMsg;

// _____   __              ___________                
// ___  | / /_____ _______ ___(_)__  /_               
// __   |/ /_  __ `/_  __ `/_  /__  __ \              
// _  /|  / / /_/ /_  /_/ /_  / _  /_/ /              
// /_/ |_/  \__,_/ _\__, / /_/  /_.___/               
//                 /____/                             
// ________      ______       __________              
// ___  __ \________  /_________  /___(_)_____________
// __  /_/ /  __ \_  __ \  __ \  __/_  /_  ___/_  ___/
// _  _, _// /_/ /  /_/ / /_/ / /_ _  / / /__ _(__  ) 
// /_/ |_| \____//_.___/\____/\__/ /_/  \___/ /____/  
                                                                                                                                                                                                                                                      
public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    // ROS Fields
    private float rosHorizontalInput;
    private float rosVerticalInput;

    [SerializeField] private GameObject car;
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;
    

    [SerializeField] private Vector3 spawnRotation;
    [SerializeField] private Vector3 spawnPosition;

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<RosControl>("control", SetSpeedAndRotate);
    }


    float map(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void SetSpeedAndRotate(RosControl controlMessage)
    {
        rosVerticalInput = controlMessage.speed / 100f;
        rosHorizontalInput = map(controlMessage.rotation, -90f, 90f, -1f, 1f);
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void GetInput()
    {
        float keyboardHorizontalInput = Input.GetAxis(HORIZONTAL);
        float keyboardVerticalInput = Input.GetAxis(VERTICAL);
        
        if (!Input.GetKey(KeyCode.LeftShift)) {
            verticalInput = rosVerticalInput;
            horizontalInput = rosHorizontalInput;
        } else {
            rosVerticalInput = 0f;
            rosHorizontalInput = 90f;

            verticalInput = keyboardVerticalInput;
            horizontalInput = keyboardHorizontalInput;
        }
        if (Input.GetKey(KeyCode.R)) {
            car.transform.eulerAngles = spawnRotation;
            car.transform.position = spawnPosition;
        }
        isBreaking = (verticalInput == 0f) || (Input.GetKey(KeyCode.Space));
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();       
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}