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
        balancePool = world.GetPool<Balance>();
        balanceSpendPool = world.GetPool<BalanceSpendEvent>();
        spendEvents = world.Filter<Balance>().Inc<BalanceSpendEvent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var e in spendEvents)
        {
            ref var balance = ref balancePool.Get(e);
            ref var balanceSpend = ref balanceSpendPool.Get(e);

            balance.Current = Mathf.Max(balance.Current - balanceSpend.Amount, 0);
            
            balanceSpendPool.Del(e);
        }
    }
}