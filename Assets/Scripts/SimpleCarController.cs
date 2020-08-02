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

public class SimpleCarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque = 1000f;
    public float maxSteeringAngle = 35f;

    public bool isPulling = false;
    public float pullStrength = 0f;
    public float pullStrengthMultiplier = 50f;
    public float pullWeakenMultiplier = 10f;
    public float slowDownMultiplier = 10f;
    public float pullToTorqueMultiplier = 10f;

    public float currentTorque = 0f;

    /// <summary>
    /// Just for displaying velocity in inspector
    /// </summary>
    public float currentVelocity;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            isPulling = true;
        } else if (Input.GetKeyUp(KeyCode.S)) {
            isPulling = false;

            // set velocity to forward at current pullStrength
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, pullStrength / 10f);
        }

        if (isPulling)
            // Motor "aufziehen" (Feder wird stärker)
            pullStrength += Time.deltaTime * pullStrengthMultiplier;
        else
            // Motor "auslaufen lassen" (Feder wird schwächer)
            pullStrength -= Time.deltaTime * pullWeakenMultiplier;

        // clamp from 0-100%
        pullStrength = Mathf.Clamp(pullStrength, 0f, 100f);
    }

    public void FixedUpdate() {
        if (!isPulling) {
            // set motor torque to 
            currentTorque = pullStrength * pullToTorqueMultiplier;
        } else {
            currentTorque -= Time.deltaTime * slowDownMultiplier;

            // this is no good yet
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -10f);
        }
        // clamp torque
        currentTorque = Mathf.Clamp(currentTorque, 0f, maxMotorTorque);

        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = currentTorque;
                axleInfo.rightWheel.motorTorque = currentTorque;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        // display velocity in inspector
        currentVelocity = rb.velocity.z;
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider) {
        if (collider.transform.childCount == 0) {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}