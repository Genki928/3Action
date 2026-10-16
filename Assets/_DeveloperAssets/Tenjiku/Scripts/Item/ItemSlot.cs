using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] ItemSlot TopSlot;
    [SerializeField] ItemSlot BottomSlot;
    [SerializeField] ItemSlot RightSlot;
    [SerializeField] ItemSlot LeftSlot;
}

public enum Direct
{ 
    Top = 0,
    Bottom = 1,
    Right = 2,
    Left = 3,
}