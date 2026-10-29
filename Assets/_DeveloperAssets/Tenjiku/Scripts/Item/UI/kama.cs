using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    public bool IsMax => _items.Count >= 3;
    public bool IsLack => _items.Count < 2;
    List<Item> _items = new();
    [SerializeField] Item returnItem;
    [SerializeField] List<Image> _image = new();

    void Start()
    {
        for (int i = 0; i < _image.Count; i++)
        {
            _image[i].sprite = null;
            _image[i].color = new(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    void Update()
    {
        
    }

    public void ReceiveItem(Item item)
    {
        _items.Add(item);
        _image[_items.Count - 1].sprite = item.Sprite;
        _image[_items.Count - 1].color = new(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public ItemStack Craft()
    {
        for (int i = 0; i < _image.Count; i++)
        {
            _image[i].sprite = null;
            _image[i].color = new(1.0f, 1.0f, 1.0f, 0.0f);
        }

        _items = new();
        return new(returnItem, 1);
    }
}