using UnityEngine;
using UnityEngine.EventSystems;

public class LetterGrid : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, /*IPointerEnterHandler, IPointerExitHandler,*/ IDragHandler
{

    private Vector2 originPoint; // Stores the origin point when pointer is down
    private Vector2 endPoint;    // Stores the current point during dragging

    [SerializeField] private RectTransform draggableArea; // Reference to the target RectTransform that bounds the dragging area

    public delegate void ClickAction();
    public event ClickAction MouseDown;
    public event ClickAction MouseUp;
    public event ClickAction MouseExit;
    public event ClickAction MouseEnter;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Capture the origin point as a Vector2 when the pointer goes down
        originPoint = GetLocalPoint(eventData);

        if (!IsWithinDraggableArea(originPoint))
        {
            Debug.Log("Pointer is outside the draggable area. Dragging will not start.");
            return;
        }

        //Debug.Log("Origin Point: " + originPoint);
        GameplayController.Instance.LetterClick((int)originPoint.x, (int)originPoint.y, true);
        //MouseDown?.Invoke(); // Safe event invocation
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the end point as a Vector2 while dragging
        endPoint = GetLocalPoint(eventData);

        if (!IsWithinDraggableArea(endPoint))
        {
            Debug.Log("Pointer moved outside the draggable area. Dragging stopped.");
            return;
        }

        //Debug.Log("Current Dragging Point: " + endPoint);
        GameplayController.Instance.LetterHover((int)endPoint.x, (int)endPoint.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Log the final end point when the pointer is released
        //Debug.Log("Final End Point: " + endPoint);
        GameplayController.Instance.LetterClick((int)endPoint.x, (int)endPoint.y, false);
        //MouseUp?.Invoke(); // Safe event invocation
    }

    private Vector2 GetLocalPoint(PointerEventData eventData)
    {
        // Convert screen position to local position within the parent RectTransform
        RectTransform rectTransform = transform as RectTransform;
        if (rectTransform == null)
        {
            Debug.LogWarning("The transform is not a RectTransform.");
            return Vector2.zero;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );
        return localPoint;
    }

    private bool IsWithinDraggableArea(Vector2 point)
    {
        if (draggableArea == null)
        {
            Debug.LogWarning("Draggable area is not set.");
            return false;
        }

        // Check if the point is within the bounds of the draggable area's RectTransform
        Rect rect = draggableArea.rect;
        return rect.Contains(point);
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    GameplayController.Instance.LetterClick((int)Position().x, (int)Position().y, true);
    //    MouseDown();
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    GameplayController.Instance.LetterClick((int)Position().x, (int)Position().y, false);

    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    //GameplayController.Instance.LetterHover((int)Position().x, (int)Position().y);
    //    //GameplayController.Instance.tempObject.SetActive(false);
    //    HandleLetterHover(eventData);
    //    //MouseEnter();
    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    HandleLetterHover(eventData);
    //}
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    //GameplayController.Instance.tempObject.SetActive(true);
    //    //MouseExit();
    //}
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    // Capture the origin point as a Vector2 when the pointer goes down
    //    originPoint = GetLocalPoint(eventData);
    //    if (IsWithinDraggableArea(originPoint))
    //    {
    //        Debug.Log("Origin Point: " + originPoint);
    //        GameplayController.Instance.LetterClick((int)originPoint.x, (int)originPoint.y, true);
    //    }
    //    else
    //    {
    //        Debug.Log("Pointer is outside the draggable area. Dragging will not start.");
    //    }

    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    // Update the end point as a Vector2 while dragging
    //    endPoint = GetLocalPoint(eventData);
    //    if (IsWithinDraggableArea(endPoint))
    //    {
    //        Debug.Log("Current Dragging Point: " + endPoint);
    //        GameplayController.Instance.LetterHover((int)endPoint.x, (int)endPoint.y);
    //    }
    //    else
    //    {
    //        Debug.Log("Pointer moved outside the draggable area. Dragging stopped.");
    //        // Optionally, you can add additional logic here to stop further processing.
    //    }

    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    // Log the final end point when the pointer is released
    //    Debug.Log("Final End Point: " + endPoint);
    //}


    //private Vector2 GetLocalPoint(PointerEventData eventData)
    //{
    //    // Convert screen position to local position within the parent RectTransform
    //    Vector2 localPoint;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        transform as RectTransform,
    //        eventData.position,
    //        eventData.pressEventCamera,
    //        out localPoint
    //    );
    //    return localPoint;
    //}
    //private bool IsWithinDraggableArea(Vector2 point)
    //{
    //    // Check if the point is within the bounds of the draggable area RectTransform
    //    if (draggableArea == null)
    //    {
    //        Debug.LogWarning("Draggable area is not set.");
    //        return false;
    //    }
    //    // Check if the point is within the bounds of the draggable area's RectTransform
    //    Rect rect = draggableArea.rect;
    //    return rect.Contains(point);
    //}

}
