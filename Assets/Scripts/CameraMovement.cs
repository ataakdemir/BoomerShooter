using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraPosition;

    public Camera Camera;

    [Header("Camera Lean Settings")]
    public float slopeAmount;
    public float slopeSpeed;
    public float returnSpeed;
    private float currentSlope = 0f;
    private float slopeVelocity = 0f; // SmoothDamp için hýz deðiþkeni

    void Update()
    {

        transform.position = cameraPosition.position;

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        float targetSlope = (horizontalInput != 0) ? horizontalInput * -slopeAmount : 0f;

        float smoothTime = (horizontalInput != 0) ? slopeSpeed : returnSpeed; 
        currentSlope = Mathf.SmoothDamp(currentSlope, targetSlope, ref slopeVelocity, smoothTime * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, currentSlope);
    }
}
