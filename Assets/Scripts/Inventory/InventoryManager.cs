using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int space = 20;
    public static InventoryManager instance;
    public List<Item> items = new List<Item>();
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Inventory is singleton!");
            return;
        }
        instance = this;
    }

    public bool Add(Item item)
    {
        if (!item.isDefault)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not enought space");
                return false;
            }
            items.Add(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
            
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
