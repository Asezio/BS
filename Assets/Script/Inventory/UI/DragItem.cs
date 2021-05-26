using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private ItemUI currentItemUI;
    private SlotHolder currentSlotholder;
    private SlotHolder targetSlotholder;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentSlotholder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录原始数据
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform) transform.parent;
        
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform,true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标位置移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品，交换数据
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInInventoryUI(eventData.position) || InventoryManager.Instance.CheckInActionUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetSlotholder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else
                    targetSlotholder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                switch (targetSlotholder.slotType)
                {
                    case SlotType.BAG:
                        SwapItem();
                        break;
                    case SlotType.ACTION:
                        if(currentItemUI.Bag.items[currentItemUI.Index].ItemData.itemType == ItemType.Useable)
                            SwapItem();
                        break;
                }
                
                currentSlotholder.UpdateItem();
                targetSlotholder.UpdateItem();
            }
        }
        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetSlotholder.itemUI.Bag.items[targetSlotholder.itemUI.Index];
        var tempItem = currentSlotholder.itemUI.Bag.items[currentSlotholder.itemUI.Index];

        bool isSameItem = tempItem.ItemData == targetItem.ItemData;

        if (isSameItem && targetItem.ItemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.ItemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentSlotholder.itemUI.Bag.items[currentSlotholder.itemUI.Index] = targetItem;
            targetSlotholder.itemUI.Bag.items[targetSlotholder.itemUI.Index] = tempItem;
        }
    }
}
