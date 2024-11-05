using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float sensivity = 100f;
    [SerializeField] Transform Player;
    [SerializeField] Joystick LookJoystick;

    float xRotation = 0f;

    private void Update()
    {
        float X = LookJoystick.Horizontal * sensivity * Time.deltaTime;
        float Y = LookJoystick.Vertical * sensivity * Time.deltaTime;

        xRotation -= Y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        Player.Rotate(Vector3.up * X);
    }

}
