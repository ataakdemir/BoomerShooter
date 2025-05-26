using UnityEngine;
using System.Collections;

public class BarsController : MonoBehaviour
{
    public float moveSpeed;

    private RectTransform rectTransform;
    private Vector3 startLocalPosition;
    private Vector3 targetLocalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startLocalPosition = rectTransform.localPosition;

        targetLocalPosition = new Vector3(startLocalPosition.x, -14 , startLocalPosition.z);

        StartCoroutine(MoveBars());
    }

    IEnumerator MoveBars()
    {
        while (rectTransform.localPosition != targetLocalPosition)
        {
            rectTransform.localPosition = Vector3.MoveTowards(
                rectTransform.localPosition,
                targetLocalPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}
