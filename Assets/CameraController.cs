using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosCamera = RosMessageTypes.NagibDemo.CameraMsg;
using System;
using System.IO;
using System.Text;
public class CameraController : MonoBehaviour
{
    ROSConnection ros;

    // The game object
    public Camera camera;
    // Publish the cube's position and rotation every N seconds

    private string topicName = "image";

    private float publishMessageFrequency = 0.033f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    private RenderTexture renderTexture;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<RosCamera>(topicName);

        if (camera.targetTexture == null) {
            camera.targetTexture = new RenderTexture(1280,720, 24);
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;
    
            camera.Render();

            Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;
            
            // byte[] imageBytes = image.EncodeToJPG();
            String result = System.Convert.ToBase64String(image.EncodeToJPG());
            // Debug.log(result);
            Destroy(image);

            // Finally send the message to server_endpoint.py running in ROS
            // sbyte[] imageSBytes = (sbyte[]) (Array) imageBytes;
            RosCamera RosImage = new RosCamera(result);
            ros.Publish(topicName, RosImage);
            timeElapsed = 0;
        }
    }
}
