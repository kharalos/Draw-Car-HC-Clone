using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private Transform frontWheels, backWheels;

    private float currentbreakForce;
    [SerializeField]
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;


    private bool updateCollider = false;
    private void Update()
    {
        if (updateCollider)
        {
            Invoke("UpdateCollider", 0.1f);
            UpdateCollider();
            GetComponent<Rigidbody>().isKinematic = false;
            updateCollider = false;
        }
    }
    void UpdateCollider()
    {
        if (GetComponent<MeshCollider>())
        {
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<MeshCollider>().enabled = true;
        }

        MeshCollider[] colliders = GetComponentsInChildren<MeshCollider>();

        foreach (MeshCollider meshCollider in colliders)
        {
            meshCollider.enabled = false;
            meshCollider.enabled = true;
            meshCollider.convex = true;
        }
    }
    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.rotation = new Quaternion(0, 0, transform.rotation.z, transform.rotation.w);
    }

    public void GetData(Vector3 rearWheelLoc, Vector3 frontWheelLoc)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody.isKinematic) rigidbody.isKinematic = false;

        rigidbody.velocity = Vector3.zero;

        frontWheels.localPosition = frontWheelLoc;
        backWheels.localPosition = rearWheelLoc;

        transform.rotation = Quaternion.identity;
        transform.position += Vector3.up;

        if (GetComponent<MeshCollider>()) 
        {
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<MeshCollider>().enabled = true;
        }

        rigidbody.isKinematic = true;

        updateCollider = true;
    }
    int accelerating = 1;
    public void StopTheCar()
    {
        isBreaking = true;
        accelerating = 0;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = accelerating * motorForce;
        frontRightWheelCollider.motorTorque = accelerating * motorForce;
        rearLeftWheelCollider.motorTorque = accelerating * motorForce;
        rearRightWheelCollider.motorTorque = accelerating * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        frontLeftWheelCollider.steerAngle = 90;
        frontRightWheelCollider.steerAngle = 90;
        rearLeftWheelCollider.steerAngle = 90;
        rearRightWheelCollider.steerAngle = 90;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
