using UnityEngine;
using UnityEngine.EventSystems;

public class RotateUIGear : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float rotationSpeed = 150f;
    private static string currentHoveredGroup = "";
    private string groupID;

    void Start()
    {
        // Grup adý, parent'ýn adý (örn: "Start Button", "Quit Button")
        groupID = transform.parent.name;
    }

    void Update()
    {
        if (currentHoveredGroup == groupID)
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Mouse bu çarka geldiðinde grup döner
        currentHoveredGroup = groupID;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentHoveredGroup == groupID)
        {
            currentHoveredGroup = "";
        }
    }
}
