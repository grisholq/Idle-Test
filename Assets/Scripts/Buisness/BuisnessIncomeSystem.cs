using Leopotam.EcsLite;
using UnityEngine;

public class BuisnessIncomeSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsPool<Balance> balancePool;
    private EcsPool<Buisness> buisnessPool;
    private EcsPool<BuisnessData> buisnessDataPool;
    private EcsPool<BuisnessCardComponent> buisnessCardComponentPool;
    private EcsPool<BalanceEarnEvent> balanceEarnEventPool;
    private EcsFilter buisnessFilter;
    private EcsFilter balanceFilter;
    
    public void Init(IEcsSystems systems)
    {
        world = systems.GetWorld();
        balancePool = world.GetPool<Balance>();
        buisnessPool = world.GetPool<Buisness>();
        buisnessDataPool = world.GetPool<BuisnessData>();
        buisnessCardComponentPool = world.GetPool<BuisnessCardComponent>();
        balanceEarnEventPool = world.GetPool<BalanceEarnEvent>();
        
        buisnessFilter = world.Filter<Buisness>().Inc<BuisnessData>().Inc<BuisnessCardComponent>().End();
        balanceFilter = world.Filter<Balance>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var b in balanceFilter)
        {
            ref var balance = ref balancePool.Get(b);

            foreach (var e in buisnessFilter)
            {
                ref var buisness = ref buisnessPool.Get(e);
                ref var buisnessData = ref buisnessDataPool.Get(e); 
                var buisnessCard = buisnessCardComponentPool.Get(e).Card;
                var incomeDelay = buisnessData.Core.IncomeDelay;
                
                if (buisness.Purchased == false) continue;
                
                buisness.IncomeTime += Time.deltaTime;
                
                if (buisness.IncomeTime >= buisnessData.Core.IncomeDelay)
                {
                    var income = Buisness.CalculateIncome(buisness, buisnessData.Core);
                    ref var balanceEarnEvent = ref balanceEarnEventPool.Add(b);
                    balanceEarnEvent.Amount += income;
                    
                    buisness.IncomeTime = 0;
                }
                
                buisnessCard.SetProgress(buisness.IncomeTime / incomeDelay);
            }
        }
    }
}
