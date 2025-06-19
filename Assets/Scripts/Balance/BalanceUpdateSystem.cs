using Leopotam.EcsLite;
using UnityEngine;

public class BalanceUpdateSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    private BalanceDisplayMono balanceDisplayMono;
    private EcsWorld world;
    private EcsPool<Balance> balancePool;
    private EcsPool<BalanceDisplay> balanceDisplayPool;
    private EcsFilter balanceFilter;
    private int balanceEntity;

    private const string SaveKey = "Balance";    
    
    public BalanceUpdateSystem(BalanceDisplayMono balanceDisplayMono)
    {
       this.balanceDisplayMono = balanceDisplayMono; 
    }
    
    public void Init(IEcsSystems systems)
    { 
        world = systems.GetWorld();
        balancePool = world.GetPool<Balance>();
        balanceDisplayPool = world.GetPool<BalanceDisplay>();
        balanceFilter = world.Filter<Balance>().Inc<BalanceDisplay>().End();
        
        balanceEntity = world.NewEntity();
        ref var balance = ref balancePool.Add(balanceEntity);
        ref var balanceDisplay = ref balanceDisplayPool.Add(balanceEntity);
        
        balance.Current = LoadBalance();
        balance.Max = 9999999;
        balanceDisplay.BalanceDisplayMono = balanceDisplayMono;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in balanceFilter)
        {
            ref var balance = ref balancePool.Get(entity);
            ref var balanceDisplay = ref balanceDisplayPool.Get(entity);
            
            balanceDisplay.BalanceDisplayMono.SetBalance(balance.Current);
        }
    }

    private float LoadBalance()
    {
        return PlayerPrefs.GetFloat(SaveKey, 100);
    }

    private void SaveBalance()
    {
        var balance = balancePool.Get(balanceEntity);
        
        PlayerPrefs.SetFloat(SaveKey, balance.Current);
    }

    public void Destroy(IEcsSystems systems)
    {
        SaveBalance();
    }
}