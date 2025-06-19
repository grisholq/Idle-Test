using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class SaveSystem : IEcsDestroySystem
{
    public void Destroy(IEcsSystems systems)
    {
        PlayerPrefs.Save();
    }
}
