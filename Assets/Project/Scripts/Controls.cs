/*using UnityEngine;
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
        brake = Input.GetKey("m") ? GetComponent<Rigidbody>().mass * 100f : 0.0f;

        wheels[0].steerAngle = steer;
        wheels[1].steerAngle = steer;
        
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
        }
    }
}*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class Controls : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }
}