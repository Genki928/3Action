using System;
using UnityEngine;

[Serializable]
public class Effect
{
	public EffectType Type => _effectType;
	public int Value => _value;
	public int Id => _id;
	[SerializeField] EffectType _effectType;
	[SerializeField] protected int _value;
	[SerializeField] protected float _time;
	protected int _id = 0;

	public Effect(int value, float time, int id = 0)
	{
		_value = value;
		_time = time;
		_id = id;
	}

	public bool RemoveTime(float deltaTime)
	{
		_time = Mathf.Max(0.0f, _time - deltaTime);
		return _time == 0.0f;
	}
}

public enum EffectType
{
	MoveSpeed,
	Power,
}