using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [Header("▼ アイテムスロット")]
    public ItemSlot Hotbar => _hotbar;
    public ItemSlot[] Slots => slots;
    [SerializeField] ItemSlot _hotbar;  // インベントリを閉じた時、カーソルにホットバーを戻す
    [SerializeField] ItemSlot[] slots = new ItemSlot[4];    // 移動先のインベントリ
}

public enum Direct
{ 
    Top = 0,
    Bottom = 1,
    Right = 2,
    Left = 3,
}