using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedometerController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textMeshPro;
    [SerializeField] public GameObject car;
    [SerializeField] public int fakeMaxSpeed;

    // Update is called once per frame
    void Update()
    {
        float speed = car.GetComponent<Rigidbody>().velocity.magnitude;
        float maxSpeed = car.GetComponent<CarController>().maxSpeed;
        
        textMeshPro.text = string.Format("{0:00} km/h", Mathf.Clamp(speed/maxSpeed, 0f, 1f) * fakeMaxSpeed);
    }
}
