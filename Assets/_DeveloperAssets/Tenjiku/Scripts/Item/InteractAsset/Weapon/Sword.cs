using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemAbility/Weapon/Sword")]
public class AbilityWeaponbAssetSword : InteractAbility
{
    [SerializeField] Sword _defaultAttackCollision;
    [SerializeField] float _attackDistance = 1.0f;

    public override void Interact(InteractContext ctx)
    {
        // 剣がまだ存在しているなら攻撃しない
        //if (ctx.character.Direction != Vector3.zero &&
        //    ctx.character.TryGetComponent<PlayerController>(out var player))
        //{

        //    var sword = Instantiate(_defaultAttackCollision, ctx.character.transform.position, Quaternion.identity);
        //    sword.Init(player, ctx.character.Direction);
        //}
    }
}