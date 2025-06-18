using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.Serialization;

class EcsStartup : MonoBehaviour 
{ 
    [SerializeField] private BalanceDisplayMono balanceDisplayMono;
    
    EcsWorld world;
    IEcsSystems systems;
    
    private void Init () {        
        world = new EcsWorld ();
        systems = new EcsSystems (world);
        systems
            .Add(new BalanceSpendSystem())
            .Add(new BalanceEarnSystem())
            .Add(new BalanceDisplaySystem(balanceDisplayMono))
            .Init ();
    }


    private void Update () {
        systems?.Run ();
    }
}