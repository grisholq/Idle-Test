using Leopotam.EcsLite;
using UnityEngine;

public class BalanceSpendSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsFilter spendEvents;
    private EcsPool<Balance> balancePool;
    private EcsPool<BalanceSpendEvent> balanceSpendPool;
    
    public void Init(IEcsSystems systems)
    { 
        world = systems.GetWorld();
        spendEvents = world.Filter<Balance>().Inc<BalanceSpendEvent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in spendEvents)
        {
            ref var balance = ref balancePool.Get(entity);
            ref var balanceSpend = ref balanceSpendPool.Get(entity);

            balance.Current = Mathf.Max(balance.Current - balanceSpend.Amount, 0);
            
            balanceSpendPool.Del(entity);
        }
    }
}