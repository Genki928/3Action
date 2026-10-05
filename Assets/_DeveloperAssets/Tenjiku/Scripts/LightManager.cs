using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] LegStateManager _legStateManager;
    Light _light;

    void Start()
    {
        _light = GetComponent<Light>();
    }

    public void NightFilter()
    {
        _light.color = Color.black;
    }

    public void DayFilter()
    {
        _light.color = Color.white;
    }

    void OnEnable()
    {
        _legStateManager.OnExploreStarted += DayFilter;
        _legStateManager.OnRaidStarted += NightFilter;
    }

    void OnDisable()
    {
        _legStateManager.OnExploreStarted -= DayFilter;
        _legStateManager.OnRaidStarted -= NightFilter;
    }
}
