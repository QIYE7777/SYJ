using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HandCardAreaBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static HandCardAreaBehaviour instance;

    CardBehaviour _tpCard;

    public RectTransform cardParent;

    private void Awake()
    {
        instance = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        //Debug.Log(eventData.pointerDrag);
        //Debug.Log(eventData.dragging);
        if (eventData.dragging && eventData.pointerDrag != null)
        {
            var c = eventData.pointerDrag.GetComponent<CardBehaviour>();
            if (c != null)
                _tpCard = c;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        _tpCard = null;
    }

    public void ValidDrop(CardBehaviour c)
    {
        if (c == _tpCard)
            AddCard();
    }

    void AddCard()
    {
        _tpCard.transform.SetParent(cardParent);
        _tpCard = null;
    }
}
