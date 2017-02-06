using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour
{
    public WheelCollider[] wheels;
 
    public float enginePower = 2000f;
    public float maxSteer = 50.0f;
    public GameObject wheel;
    public GameObject[] realWheels;

    private float power = 0.0f;
    private float brake = 0.0f;
    private float steer = 0.0f;

    void FixedUpdate()
    {
		// Calcul en fonction de l'appui sur les touches
        power = Input.GetAxis("Vertical") * enginePower * Time.deltaTime * 250.0f;
        steer = Input.GetAxis("Horizontal") * maxSteer;
        brake = Input.GetKey(KeyCode.Space) ? GetComponent<Rigidbody>().mass * 100f : 0.0f;

		// Permet de regarder derrière
        GameObject cam = GameObject.Find("StockcarCamera");
        if (cam != null)
        {
            if (Input.GetKey(KeyCode.C)) cam.transform.localEulerAngles = new Vector3(0, 180, 0);
            else cam.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

		// Pivote les wheels colliders et le volant
        wheels[0].steerAngle = steer;
        wheels[1].steerAngle = steer;
        wheel.transform.localEulerAngles = new Vector3(-steer*2, 0, 0);

		// Pivote les roues Assets
        for(int i = 0; i < realWheels.Length; i++)
        {
            float tmp = realWheels[i].transform.localEulerAngles.x - power;
            if (tmp == 0) tmp = -power;
            if (i == 0 || i == 1)
                realWheels[i].transform.localEulerAngles = new Vector3(tmp, -90 + steer / 2, 90);
            else
                realWheels[i].transform.localEulerAngles = new Vector3(tmp, -90, 90);

        }
        
		// Fait ralentir en cas d'appui sur "espace"
        if (brake > 0.0f)
        {
            wheels[0].brakeTorque = brake;
            wheels[1].brakeTorque = brake;
            wheels[2].brakeTorque = brake;
            wheels[3].brakeTorque = brake;
            wheels[2].motorTorque = 0.0f;
            wheels[3].motorTorque = 0.0f;
        }
		// Sinon fait avancer
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
}