using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour
{
    public WheelCollider[] wheels;
 
    public float enginePower = 2000f;
    public float maxSteer = 50.0f;

    private float power = 0.0f;
    private float brake = 0.0f;
    private float steer = 0.0f;


    private Rigidbody rb;

    void Start()
    {

    }

    void FixedUpdate()
    {
        power = Input.GetAxis("Vertical") * enginePower * Time.deltaTime * 250.0f;
        steer = Input.GetAxis("Horizontal") * maxSteer;
        brake = Input.GetKey("m") ? GetComponent<Rigidbody>().mass * 0.1f : 0.0f;

        wheels[0].steerAngle = steer;
        wheels[1].steerAngle = steer;
        //rb.AddTorque(Vector3.up * steer);
        
        if (brake > 0.0f)
        {
            wheels[0].brakeTorque = brake;
            wheels[1].brakeTorque = brake;
            wheels[2].brakeTorque = brake;
            wheels[3].brakeTorque = brake;
            wheels[2].motorTorque = 0.0f;
            wheels[3].motorTorque = 0.0f;
            //rb.AddForce(Vector3.back * power);
        }
        else
        {
            wheels[0].brakeTorque = 0f;
            wheels[1].brakeTorque = 0f;
            wheels[2].brakeTorque = 0f;
            wheels[3].brakeTorque = 0f;
            wheels[2].motorTorque = power;
            wheels[3].motorTorque = power;
            //rb.AddForce(Vector3.forward * power);
        }
    }

    /*private WheelCollider GetCollider(int n){
        return wheels[n].gameObject.GetComponent<WheelCollider>();
    }*/
}