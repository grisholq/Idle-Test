using Leopotam.EcsLite;
using UnityEngine;

public class BalanceEarnSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter earnEvents;
    private EcsPool<Balance> balancePool;
    private EcsPool<BalanceEarnEvent> balanceEarnPool;
    
    public void Init(IEcsSystems systems)
    { 
        world = systems.GetWorld();
        balancePool = world.GetPool<Balance>();
        balanceEarnPool = world.GetPool<BalanceEarnEvent>();
        earnEvents = world.Filter<Balance>().Inc<BalanceEarnEvent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in earnEvents)
        {
            ref var balance = ref balancePool.Get(entity);
            ref var balanceEarn = ref balanceEarnPool.Get(entity);

            balance.Current = Mathf.Min(balance.Current + balanceEarn.Amount, balance.Max);
            
            balanceEarnPool.Del(entity);
        }
    }
}