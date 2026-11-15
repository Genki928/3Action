using System.Collections.Generic;

public class ModifierAttribute<T> : Attribute<T>
{
    protected List<Effect> effects = new();

    public void EffectUpdate(float deltaTime)
    {
        effects.RemoveAll(ef => ef.RemoveTime(deltaTime));
    }

    public void AddEffect(Effect effect)
    {
        effects.Add(effect);
    }

    public void RemoveEffect(int id)
    {
        effects.RemoveAll(ef => ef.Id == id);
    }
}