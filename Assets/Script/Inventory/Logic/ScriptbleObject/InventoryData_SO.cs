using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory",menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(ItemData_SO newItemData, int amount)
    {
        bool found = false;
        //check if already has same items in bag
        if (newItemData.stackable)
        {
            foreach (var item in items)
            {
                if (item.ItemData == newItemData)
                {
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
        }
        //create it in the next block if the item hasn't been found  
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemData == null && !found)
            {
                items[i].ItemData = newItemData;
                items[i].amount = amount;
                break;
            }
            
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    [SerializeField]
    public ItemData_SO ItemData;
    public int amount;
}