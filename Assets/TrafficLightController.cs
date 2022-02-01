using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public Material colors_material;

    public Material black_material;

    private GameObject red_light;

    private GameObject yellow_light;

    private GameObject green_light;

    private GameObject[] traffic_lights;

    private string current_color = "red";

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            current_color = "red";
            ChangeLightColor();
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            current_color = "yellow";
            ChangeLightColor();
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            current_color = "green";
            ChangeLightColor();
        }
    }

    void Start()
    {
        traffic_lights = GameObject.FindGameObjectsWithTag("TrafficLight");
    }

    void ChangeLightColor()
    {
        foreach (GameObject light in traffic_lights)
        {
            light
                .transform
                .Find("red_light")
                .gameObject
                .GetComponent<MeshRenderer>()
                .material =
                (current_color == "red") ? colors_material : black_material;
            light
                .transform
                .Find("yellow_light")
                .gameObject
                .GetComponent<MeshRenderer>()
                .material =
                (current_color == "yellow") ? colors_material : black_material;
            light
                .transform
                .Find("green_light")
                .gameObject
                .GetComponent<MeshRenderer>()
                .material =
                (current_color == "green") ? colors_material : black_material;
        }
    }
}
