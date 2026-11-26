using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Cauldron : MonoBehaviour
{
    public bool IsMax => _items.Count >= 3;
    public bool IsLack => _items.Count < 2;
    List<Item> _items = new();
    [SerializeField] Item returnItem;
    [SerializeField] List<CraftSlot> _slot = new();

    [SerializeField] private Vector2 center = Vector2.zero; // 楕円の中心
    [SerializeField] private float radiusX = 3f;            // 横方向の半径
    [SerializeField] private float radiusY = 1.5f;          // 縦方向の半径
    [SerializeField] private float speed = 2f;              // 移動速度（rad/秒）

    void Start()
    {
        foreach (var slot in _slot)
            slot.SetImage(null, new(1.0f, 1.0f, 1.0f, 0.0f));
    }

    void Update()
    {
        for (int i = 0; i < _slot.Count; i++)
            _slot[i].Move(speed, center, radiusX, radiusY, Mathf.PI * 2 / _slot.Count * i);
    }

    public void ReceiveItem(Item item)
    {
        _items.Add(item);
        _slot[_items.Count - 1].SetImage(item.Sprite, new(1.0f, 1.0f, 1.0f, 1.0f));
    }

    public PotionItem Craft()
    {
        foreach (var slot in _slot)

            slot.SetImage(null, new(1.0f, 1.0f, 1.0f, 1.0f));

        // アイテム作成
        PotionItem craft = new(returnItem, 1);
        foreach (var item in _items)
        {
            if (item.ItemAbility == null) continue;

            // 素材が持つ効能をポーションに入れていく
            foreach (var ability in item.ItemAbility)
            {
                craft.AddAbility(ability);
            }
        }

        _items = new();
        foreach (var slot in _slot)
            slot.SetImage(null, new(1.0f, 1.0f, 1.0f, 0.0f));

        return craft;
    }
}