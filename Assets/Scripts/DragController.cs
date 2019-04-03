using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IDragHandler
{
    public static event Action PointMoved;


    [SerializeField]
    private RectTransform content;

    private Vector2 currentMousePosition;


    public virtual void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(content, eventData.position, eventData.pressEventCamera,
            out currentMousePosition);

        transform.position = currentMousePosition;


        PointMoved();
    }
}