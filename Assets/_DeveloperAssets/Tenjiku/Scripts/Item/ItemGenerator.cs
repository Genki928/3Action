using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    // --- シングルトン --- //
    public static ItemGenerator Instance;

    void Awake()
    {
        Instance = this;
    }

    //
    [SerializeField] ItemObj _prefab;

    public void ItemGenerate(Vector3 ownerPos, ItemStack stack, float cooltime = 1.0f)
    {
        Vector3 pos = new(ownerPos.x, ownerPos.y + 3.0f, ownerPos.z + 1.0f);
        var obj = Instantiate(_prefab, pos, Quaternion.identity);
        obj.Init(stack, cooltime);
    }
}