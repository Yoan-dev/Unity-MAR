using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour
{
    public WheelCollider[] wheels;
 
    private float enginePower = 2000f;

    private float power = 0.0f;
    private float brake = 0.0f;
    private float steer = 0.0f;

    private float maxSteer = 50.0f;

    void Start()
    {
       // GetComponent<Rigidbody>().centerOfMass = new Vector3(0,0,0);
    }

    void Update()
    {
        power = Input.GetAxis("Vertical") * enginePower * Time.deltaTime * 250.0f;
        steer = Input.GetAxis("Horizontal") * maxSteer;
        brake = Input.GetKey("m") ? GetComponent<Rigidbody>().mass * 0.1f : 0.0f;

        wheels[0].steerAngle = steer;
        wheels[1].steerAngle = steer;
        Debug.Log("power " + wheels[2].motorTorque);
        Debug.Log("steer " + wheels[0].steerAngle);
        Debug.Log("brake " + wheels[0].brakeTorque);

        /*if(wheels[0].isGrounded)
            Debug.Log(wheels[0].isGrounded);
        if (wheels[1].isGrounded)
            Debug.Log(wheels[1].isGrounded);
        if (wheels[2].isGrounded)
            Debug.Log(wheels[2].isGrounded);
        if (wheels[3].isGrounded)
            Debug.Log(wheels[3].isGrounded);*/
        if (brake > 0.0f)
        {
            wheels[0].brakeTorque = brake;
            wheels[1].brakeTorque = brake;
            wheels[2].brakeTorque = brake;
            wheels[3].brakeTorque = brake;
            wheels[2].motorTorque = 0.0f;
            wheels[3].motorTorque = 0.0f;
        }
        else
        {
            wheels[0].brakeTorque = 0f;
            wheels[1].brakeTorque = 0f;
            wheels[2].brakeTorque = 0f;
            wheels[3].brakeTorque = 0f;
            wheels[2].motorTorque = power;
            wheels[3].motorTorque = power;
            //GetComponent<Rigidbody>().AddTorque(Vector3.forward * power);
        }
    }

    /*private WheelCollider GetCollider(int n){
        return wheels[n].gameObject.GetComponent<WheelCollider>();
    }*/
}