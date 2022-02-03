using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;

using RosDisplay = RosMessageTypes.NagibDemo.DisplayMsg;

public class DisplayController : MonoBehaviour
{
    public RenderTexture displayTexture;

    void Start()
    {
        ROSConnection
            .GetOrCreateInstance()
            .Subscribe<RosDisplay>("display", ShowDisplayImage);
    }

    void ShowDisplayImage(RosDisplay displayMessage)
    {
        byte[] Bytes = System.Convert.FromBase64String(displayMessage.image);
        Texture2D tex = new Texture2D(1280, 720);
        tex.LoadImage (Bytes);
        Graphics.Blit (tex, displayTexture);
        Destroy (tex);
    }
}
