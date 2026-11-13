using System;
using UnityEngine;

[Serializable]
public class Effect
{
	public EffectType Type => _effectType;
	[SerializeField] EffectType _effectType;
	[SerializeField] float _value;
	[SerializeField] float _time;

	public Effect(EffectType type, float value, float time)
	{
		;
	}
}

public enum EffectType
{
	MoveSpeed,
	Power,
}