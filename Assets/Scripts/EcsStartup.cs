using System;
using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.Serialization;

class EcsStartup : MonoBehaviour 
{ 
    [SerializeField] private BalanceDisplayMono balanceDisplayMono;
    [SerializeField] private BuisnessCoreConfig buisnessCoreConfig; 
    [SerializeField] private BuisnessCosmeticConfig buisnessCosmeticConfig;
    [SerializeField] private BuisnessCardListDisplay buisnessCardListDisplay;
    
    private EcsWorld world;
    private IEcsSystems systems;

    private void Start()
    {
        Init();
    }

    private void Init () 
    {        
        world = new EcsWorld ();
        systems = new EcsSystems (world);
        systems
            .Add(new BalanceSpendSystem())
            .Add(new BalanceEarnSystem())
            .Add(new BalanceDisplaySystem(balanceDisplayMono))
            .Add(new BuisnessCreateSystem(buisnessCoreConfig, buisnessCosmeticConfig, buisnessCardListDisplay))
            .Init ();
    }

    private void Update () 
    {
        systems?.Run ();
    }
}