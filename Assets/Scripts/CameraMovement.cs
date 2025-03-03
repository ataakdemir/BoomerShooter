using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
public class CameraMovement : MonoBehaviour
{
    public Transform cameraPosition;

    public Camera Camera;

    public float rotateTime;
    public float returnTime;
    public float slopeAmount;

    private Tween currentTween;

    void Update()
    {

        transform.position = cameraPosition.position;

        float inputX = Input.GetAxisRaw("Horizontal");
        
        if (currentTween != null) currentTween.Kill();

        if (inputX > 0)
        {
            currentTween = transform.DOLocalRotate(new Vector3(0, 0, -slopeAmount), rotateTime);
        }
        else if (inputX < 0)
        {
            currentTween = transform.DOLocalRotate(new Vector3(0, 0, slopeAmount), rotateTime);
        }
        else
        {
            currentTween = transform.DOLocalRotate(Vector3.zero, returnTime);
        }
    }
}
