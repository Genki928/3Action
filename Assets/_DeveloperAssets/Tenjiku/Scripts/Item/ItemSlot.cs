using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public ItemSlot[] Slots => slots;
    [SerializeField] ItemSlot[] slots = new ItemSlot[4];
}

public enum Direct
{ 
    Top = 0,
    Bottom = 1,
    Right = 2,
    Left = 3,
}