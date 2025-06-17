using UnityEngine;
using Leopotam.EcsLite;

class EcsStartup : MonoBehaviour 
{
    EcsWorld _world;
    IEcsSystems _systems;
    
    private void Init () {        
        _world = new EcsWorld ();
        _systems = new EcsSystems (_world);
        _systems.Init ();
    }


    private void Update () {
        _systems?.Run ();
    }
}