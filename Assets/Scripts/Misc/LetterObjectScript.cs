using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class LetterObjectScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    #region Ddependency
    private UIManager _UIManager;
    #endregion
    public delegate void ClickAction();
    public event ClickAction MouseDown;
    public event ClickAction MouseUp;
    public event ClickAction MouseExit;
    public event ClickAction MouseEnter;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameplayController.Instance.LetterClick((int)Position().x, (int)Position().y, true);
        MouseDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameplayController.Instance.LetterClick((int)Position().x, (int)Position().y, false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //GameplayController.Instance.LetterHover((int)Position().x, (int)Position().y);
        //GameplayController.Instance.tempObject.SetActive(false);
        HandleLetterHover(eventData);
        MouseEnter();
    }
    public void OnDrag(PointerEventData eventData)
    {
        HandleLetterHover(eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //GameplayController.Instance.tempObject.SetActive(true);
        MouseExit();
    }
    public void InjectingDependency(UIManager _UiManager)
    {
        _UIManager = _UiManager;
    }
    private Vector2 Position()
    {
        string[] numbers = transform.name.Split('-');
        int x = int.Parse(numbers[0]);
        int y = int.Parse(numbers[1]);
        Vector2 position = new Vector2(x, y);
        return position;
    }
    private void HandleLetterHover(PointerEventData eventData)
    {
        // Convert screen position to local position within the canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_UIManager.GetGridRectTransform(), eventData.position, null, out localPoint);
        localPoint.y = -localPoint.y;
        // Check if the localPoint is within the bounds of the canvasRect
        if (IsWithinCanvasBounds(localPoint))
        {
            // Convert localPoint to grid coordinates
            Vector2 gridPosition = GetGridPositionFromLocalPoint(localPoint);
            GameplayController.Instance.LetterHover((int)gridPosition.x, (int)gridPosition.y);
        }
        else
        {
            Debug.Log("Pointer is outside the bounds of the canvas.");
        }
    }
    private bool IsWithinCanvasBounds(Vector2 localPoint)
    {
        // Check if the localPoint is within the bounds of the canvas's RectTransform
        return localPoint.x >= _UIManager.GetGridRectTransform().rect.xMin && localPoint.x <= _UIManager.GetGridRectTransform().rect.xMax &&
               localPoint.y >= _UIManager.GetGridRectTransform().rect.yMin && localPoint.y <= _UIManager.GetGridRectTransform().rect.yMax;
    }
    private Vector2 GetGridPositionFromLocalPoint(Vector2 localPoint)
    {
        // Assume your UI grid is aligned with the canvas, and localPoint (0, 0) is center of canvas
        // Adjust based on how your grid is laid out relative to the canvas

        // Convert local point to grid coordinates (you may need to adjust based on grid layout)
        float x = (localPoint.x - _UIManager.GetGridRectTransform().rect.xMin) / _UIManager.GetGridRectTransform().rect.width * (GameplayController.Instance.gridSize.x); // gridWidth is the number of grid cells horizontally
        float y = (localPoint.y - _UIManager.GetGridRectTransform().rect.yMin) / _UIManager.GetGridRectTransform().rect.height * (GameplayController.Instance.gridSize.y); // gridHeight is the number of grid cells vertically

        // Rounding or flooring the coordinates to match specific grid cells
        int gridX = Mathf.FloorToInt(x);
        int gridY = Mathf.FloorToInt(y);

        return new Vector2(gridX, gridY);
    }

}
