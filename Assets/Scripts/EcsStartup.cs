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
        Application.targetFrameRate = 60;
        
        world = new EcsWorld ();
        systems = new EcsSystems (world);
        systems
            .Add(new SaveSystem())
            .Add(new BalanceSpendSystem())
            .Add(new BalanceEarnSystem())
            .Add(new BalanceUpdateSystem(balanceDisplayMono))
            .Add(new BuisnessCreateSystem(buisnessCoreConfig, buisnessCosmeticConfig, buisnessCardListDisplay))
            .Add(new BuisnessCosmeticsInitSystem())
            .Add(new BuisnessLevelUpSystem())
            .Add(new BuisnessIncomeSystem())
            .Add(new BuinessUpgradeSystem())
            .Init ();
    }

    private void Update () 
    {
        systems?.Run ();
    }

    private void OnDestroy()
    {
        systems?.Destroy ();
    }
}