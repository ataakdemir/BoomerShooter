using System.Collections;
using UnityEngine;

public class ElevatorDoorController : MonoBehaviour
{
    public Transform firstDoor;
    public Transform secondDoor;

    public Vector3 firstDoorOpenOffset = new Vector3(1f, 0, 0);
    public Vector3 secondDoorOpenOffset = new Vector3(0, 1f, 0);

    public float openSpeed = 10f;
    public float delay = 0.01f; 

    private Vector3 firstDoorClosedPos;
    private Vector3 firstDoorOpenPos;

    private Vector3 secondDoorClosedPos;
    private Vector3 secondDoorOpenPos;

    private Coroutine doorCoroutine;

    void Start()
    {
        firstDoorClosedPos = firstDoor.localPosition;
        firstDoorOpenPos = firstDoorClosedPos + firstDoorOpenOffset;

        secondDoorClosedPos = secondDoor.localPosition;
        secondDoorOpenPos = secondDoorClosedPos + secondDoorOpenOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (doorCoroutine != null) StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(OpenDoorsSequence());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (doorCoroutine != null) StopCoroutine(doorCoroutine);
            doorCoroutine = StartCoroutine(CloseDoorsSequence());
        }
    }

    IEnumerator OpenDoorsSequence()
    {
        while (firstDoor.localPosition != firstDoorOpenPos)
        {
            firstDoor.localPosition = Vector3.MoveTowards(firstDoor.localPosition, firstDoorOpenPos, openSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        while (secondDoor.localPosition != secondDoorOpenPos)
        {
            secondDoor.localPosition = Vector3.MoveTowards(secondDoor.localPosition, secondDoorOpenPos, openSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator CloseDoorsSequence()
    {
        while (secondDoor.localPosition != secondDoorClosedPos)
        {
            secondDoor.localPosition = Vector3.MoveTowards(secondDoor.localPosition, secondDoorClosedPos, openSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(delay);

        while (firstDoor.localPosition != firstDoorClosedPos)
        {
            firstDoor.localPosition = Vector3.MoveTowards(firstDoor.localPosition, firstDoorClosedPos, openSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
