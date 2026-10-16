using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public SlotToggleUI[] Slots => slots;
    [SerializeField] SlotToggleUI[] slots = new SlotToggleUI[4];

    [Serializable]
    public class SlotToggleUI
    {
        public ItemSlot normal;
        public ItemSlot opened;
    }
}

public enum Direct
{ 
    Top = 0,
    Bottom = 1,
    Right = 2,
    Left = 3,
}