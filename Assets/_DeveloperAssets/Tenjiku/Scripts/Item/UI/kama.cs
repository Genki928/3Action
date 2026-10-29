using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    List<Item> _items = new();
    [SerializeField] Item returnItem;

    void Start()
    {
        ;
    }

    public void ReceiveItem(Item item)
    {
        _items.Add(item);
    }

    public ItemStack Craft()
    {
        if (!(_items.Count >= 2)) return null;

        return new(returnItem, 1);
    }
}