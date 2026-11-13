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

        // アイテム作成
        ItemStack craft = new(returnItem, 1);
        foreach (var item in _items)
        {
            if (item.InteractAsset == null) continue;

            // 素材が持つ効能をポーションに入れていく
            foreach (var cEvent in item.InteractAsset)
            {
                craft.AddClickEvent(cEvent);
            }
        }

        _items = new();

        return craft;
    }
}