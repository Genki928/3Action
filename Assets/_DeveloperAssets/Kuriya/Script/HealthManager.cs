using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] List<GameObject> healthobj;

    PlayerController _player;

    void Start()
    {
        _player = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        for (int i = 0; i < healthobj.Count; i++)
        {
            if (i < _player.Health.Value)
            {
                healthobj[i].SetActive(true);
            }
            else
            {
                healthobj[i].SetActive(false);
            }
        }
    }
}