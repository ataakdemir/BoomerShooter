using UnityEngine;

public class KnifeController : MonoBehaviour
{
    public RectTransform knifeLeft;
    public RectTransform knifeRight;

    public float moveSpeed = 1000f;
    public float distanceFromButton = 50f;

    private bool showKnives = false;
    private RectTransform targetButton;

    private Vector2 leftHiddenPos;
    private Vector2 rightHiddenPos;

    void Start()
    {
        // Býçaklarýn baþlangýç pozisyonlarýný kaydet
        leftHiddenPos = knifeLeft.anchoredPosition;
        rightHiddenPos = knifeRight.anchoredPosition;
    }

    void Update()
    {
        if (targetButton == null) return;

        // Býçaklarýn buton kenarýndaki pozisyonlarýný hesapla
        Vector2 leftTarget = targetButton.anchoredPosition + new Vector2(-targetButton.sizeDelta.x / 2 - distanceFromButton, 0f);
        Vector2 rightTarget = targetButton.anchoredPosition + new Vector2(targetButton.sizeDelta.x / 2 + distanceFromButton, 0f);

        // Býçaklar görünürken butonun yanýna gelir, gizlenirken baþlangýç (sahne dýþý) pozisyonuna gider
        Vector2 leftPos = showKnives ? leftTarget : leftHiddenPos;
        Vector2 rightPos = showKnives ? rightTarget : rightHiddenPos;

        knifeLeft.anchoredPosition = Vector2.MoveTowards(knifeLeft.anchoredPosition, leftPos, moveSpeed * Time.deltaTime);
        knifeRight.anchoredPosition = Vector2.MoveTowards(knifeRight.anchoredPosition, rightPos, moveSpeed * Time.deltaTime);
    }

    public void ShowKnives(RectTransform button)
    {
        targetButton = button;
        showKnives = true;
    }

    public void HideKnives()
    {
        showKnives = false;
    }
}
