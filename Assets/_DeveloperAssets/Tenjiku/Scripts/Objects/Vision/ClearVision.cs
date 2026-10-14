using System.Collections.Generic;
using UnityEngine;

public class ClearVision : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] int _searchIndex;

    void OnTriggerStay(Collider col)
    {
        if (col.TryGetComponent<MeshRenderer>(out var mr))
        {
            mr.material.color = new(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.TryGetComponent<MeshRenderer>(out var mr))
        {
            mr.material.color = new(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
