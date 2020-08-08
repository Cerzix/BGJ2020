using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public Transform visualWheel;
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque = 1000f;
    public float maxSteeringAngle = 35f;

    private bool isPulling = false;
    private float pullStrength = 0f;
    public float PullStrength { get { return pullStrength; } }

    [SerializeField]
    private float pullStrengthMultiplier = 50f;
    [SerializeField]
    private float pullWeakenMultiplier = 10f;
    [SerializeField]
    private float slowDownMultiplier = 2f;
    [SerializeField]
    private float pullToTorqueMultiplier = 14f;

    private float currentTorque = 0f;

    /// <summary>
    /// Just for displaying velocity in inspector
    /// </summary>
    public float currentVelocity;

    private void Start()
    {
        foreach (AxleInfo axle in axleInfos)
        {
            axle.leftWheel.ConfigureVehicleSubsteps(5, 5, 5);
            axle.rightWheel.ConfigureVehicleSubsteps(5, 5, 5);
        }
    }

    private bool justBoosted = false;
    private IEnumerator Cooldown() {
        yield return new WaitForSeconds(1);
        justBoosted = false;
    }

    private void Update() {
        if (!justBoosted) {
            if (Input.GetKeyDown(KeyCode.S))
                isPulling = true;
            else if (Input.GetKeyUp(KeyCode.S)) {
                isPulling = false;

                justBoosted = true;
                StartCoroutine(Cooldown());

                rb.velocity = Vector3.zero;
                if (pullStrength > 90 && pullStrength <= 95)
                    rb.velocity = transform.forward * pullStrength / 10f * 2f;
                else
                    rb.velocity = transform.forward * pullStrength / 10f;
            }
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

            rb.velocity = -transform.forward * 10f;
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