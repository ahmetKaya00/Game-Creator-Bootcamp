using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    public Transform frontLeftTransform, frontRightTransform;
    public Transform rearLeftTransform, rearRightTransform;

    public WheelCollider frontLeftCollider, frontRightCollider;
    public WheelCollider rearLeftCollider, rearRightCollider;

    public float maxSteerAngle = 30f;
    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float decelerationForce = 100f;
    public float reverseForce = 1000f;
    public float stopThreshold = 1.0f;

    public float flipThresholdAngle = 45f;

    private float currentSteerAngle;
    private float currentAcceleration;
    private float currentBrakeForce;
    private bool isReversing = false;
    private bool isCheckingFlip = false;
    private float flipCheckTime = 4f;

    public SteeringWheel steeringWheel;
    private Rigidbody carRigidbody;
    private Renderer carRenderer;

    private void Start()
    {
        
        carRigidbody = GetComponent<Rigidbody>();
        carRenderer = GetComponentInChildren<Renderer>();

        FindSteeringWheel();

        FindButtonsAndAssignEventTrigger();
        carRigidbody.centerOfMass = new Vector3(0, -0.5f, 0); // Aðýrlýk merkezini düþür

    }

    private void Update()
    {
        if(steeringWheel != null)
        {
            currentSteerAngle = maxSteerAngle * steeringWheel.Value;
        }

        ApplyMovement();

        UpdateWheelPoses();

        checkFlip();
    }

    private void checkFlip()
    {
        if(Vector3.Angle(Vector3.up, transform.up) > flipThresholdAngle)
        {
            if (!isCheckingFlip)
            {
                isCheckingFlip = true;
                Invoke("CheckIfStillFlipped", flipCheckTime);
            }
        }
        else
        {
            isCheckingFlip = false;
            CancelInvoke("CheckIfStillFlipped");
        }
    }

    private void CheckIfStillFlipped()
    {
        if (isCheckingFlip)
        {
            StartRespawnProcess();
        }
    }
    private void StartRespawnProcess()
    {
        carRenderer.enabled = false;

        Invoke("Respawn", 2f);
    }
    private void Respawn()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        transform.rotation = Quaternion.identity;
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;

        carRenderer.enabled = true;

        isCheckingFlip = false;
    }

    private void ApplyMovement()
    {
        frontLeftCollider.steerAngle = currentSteerAngle;
        frontRightCollider.steerAngle = currentSteerAngle;


        if (currentAcceleration == 0f && currentBrakeForce == 0f)
        {
            rearLeftCollider.brakeTorque = decelerationForce;
            rearRightCollider.brakeTorque = decelerationForce;
        }
        else {
            rearLeftCollider.brakeTorque = currentBrakeForce;
            rearRightCollider.brakeTorque = currentBrakeForce;
        
        }

        rearLeftCollider.motorTorque = currentAcceleration;
        rearRightCollider.motorTorque = currentAcceleration;
    }

    private void FindSteeringWheel()
    {
        GameObject steeringWhellObj = GameObject.Find("SteeringWheel");

        if(steeringWhellObj != null)
        {
            steeringWheel = steeringWhellObj.GetComponent<SteeringWheel>();
            if(steeringWheel == null)
            {
                Debug.LogError("SteeringWhell bileþeni bulunamadý!");
            }
        }
        else
        {
            Debug.LogError("SteeringWhell objesi bulunamadý!");
        }
    }

    private void FindButtonsAndAssignEventTrigger()
    {
        GameObject gasButton = GameObject.Find("Gas");
        if(gasButton != null)
        {
            AddEventTriggers(gasButton, GasOn, BrekaOn);
        }
        GameObject reverseButton = GameObject.Find("Reverse");
        if (reverseButton != null)
        {
            AddEventTriggers(reverseButton, reverseOn, BrekaOn);
        }
        GameObject brakeButton = GameObject.Find("Brake");
        if (brakeButton != null)
        {
            AddEventTriggerToPointerDown(brakeButton, BrekaOn);
        }
    }
    private void AddEventTriggers(GameObject buttonObj,UnityEngine.Events.UnityAction pointerDownAction, UnityEngine.Events.UnityAction pointerUpAction)
    {
        EventTrigger trigger = buttonObj.GetComponent<EventTrigger>();

        if (trigger == null) { 
            trigger = buttonObj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDownEntry.callback.AddListener((eventData) => { pointerDownAction(); });
        trigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((eventData) => { pointerUpAction(); });
        trigger.triggers.Add(pointerUpEntry);
    }

    private void AddEventTriggerToPointerDown(GameObject buttonObj, UnityEngine.Events.UnityAction pointerDownAction)
    {
        EventTrigger trigger = buttonObj.GetComponent<EventTrigger>();
        if(trigger == null)
        {
            trigger = buttonObj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
        {

            eventID = EventTriggerType.PointerDown
        };

        pointerDownEntry.callback.AddListener((eventData) => { pointerDownAction(); });
        trigger.triggers.Add(pointerDownEntry);
    }

    public void GasOn() {
        Debug.Log("Tetik");
        if (isReversing)
        {
            currentAcceleration = 0f;
            currentBrakeForce = brakeForce;
            if (carRigidbody.velocity.magnitude < stopThreshold)
            {
                isReversing = false;
                currentBrakeForce = 0f;
                currentAcceleration += motorForce;
            }
        }
        else {
            currentBrakeForce = 0f;
            currentAcceleration = motorForce;
        }
    
    }

    public void reverseOn()
    {
        if (!isReversing)
        {
            currentAcceleration = 0f;
            currentBrakeForce = brakeForce;
            if(carRigidbody.velocity.magnitude < stopThreshold)
            {
                isReversing = true;
                currentBrakeForce = 0f;
                currentAcceleration = -reverseForce;
            }
        }
    }

    public void BrekaOn()
    {
        currentAcceleration = 0f;
        currentBrakeForce = brakeForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftCollider, frontLeftTransform);
        UpdateWheelPose(frontRightCollider, frontRightTransform);
        UpdateWheelPose(rearLeftCollider, rearLeftTransform);
        UpdateWheelPose(rearRightCollider, rearRightTransform);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }

}
