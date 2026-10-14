using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] LegStateManager _legStateManager;
    Light _light;
    bool _doDayCycle = false;

    void Start()
    {
        _light = GetComponent<Light>();
    }

    void Update()
    {
        if (!_doDayCycle) return;

        var ratio = 1.0f - _legStateManager.Ratio;
        _light.color = new(ratio + 0.5f, ratio, ratio);
    }

    public void NightFilter()
    {
        _light.color = Color.black;
        _doDayCycle = false;
    }

    public void EnableDayCycle()
    {
        _doDayCycle = true;
    }

    void OnEnable()
    {
        _legStateManager.OnExploreStarted += EnableDayCycle;
        _legStateManager.OnRaidStarted += NightFilter;
    }

    void OnDisable()
    {
        _legStateManager.OnExploreStarted -= EnableDayCycle;
        _legStateManager.OnRaidStarted -= NightFilter;
    }
}
