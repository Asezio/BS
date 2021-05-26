using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType{BAG,ACTION}
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem() != null)
        {
            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                GameManager.Instance.playerStats.AddHealth(itemUI.GetItem().usableData.healthRecover);
                itemUI.Bag.items[itemUI.Index].amount -= 1;
            }
            UpdateItem();
        }
    }
    
    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
        }
        //Debug.Log(itemUI.Index);
        var item = itemUI.Bag.items[itemUI.Index];
        //Debug.Log("Here");
        itemUI.SetupItemUI(item.ItemData, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.toolTip.SetupToolTip(itemUI.GetItem());
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);

    }
}
