using Leopotam.EcsLite;

public class BuisnessIncomeSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsPool<Balance> balancePool;
    private EcsPool<Buisness> buisnessPool;
    private EcsPool<BuisnessCoreData> buisnessCoreDataPool;
    private EcsPool<BuisnessCardComponent> buisnessCardComponentPool;
    private EcsPool<BalanceEarnEvent> balanceEarnEventPool;
    private EcsFilter buisnessFilter;
    private EcsFilter balanceFilter;
    
    public void Init(IEcsSystems systems)
    {
        world = systems.GetWorld();
        balancePool = world.GetPool<Balance>();
        buisnessPool = world.GetPool<Buisness>();
        buisnessCoreDataPool = world.GetPool<BuisnessCoreData>();
        buisnessCardComponentPool = world.GetPool<BuisnessCardComponent>();
        balanceEarnEventPool = world.GetPool<BalanceEarnEvent>();
        
        buisnessFilter = world.Filter<Buisness>().Inc<BuisnessCoreData>().Inc<BuisnessCardComponent>().End();
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
                ref var buisnessCoreData = ref buisnessCoreDataPool.Get(e); 
                var buisnessCard = buisnessCardComponentPool.Get(e).Card;
                
                if (buisness.IncomeTime >= buisnessCoreData.IncomeDelay)
                {
                    var income = Buisness.CalculateIncome(buisness, buisnessCoreData);
                    ref var balanceEarnEvent = ref balanceEarnEventPool.Add(b);
                    balanceEarnEvent.Amount += income;
                    
                    buisness.IncomeTime = 0;
                }
                
                buisnessCard.SetProgress(buisness.IncomeTime / buisnessCoreData.IncomeDelay);
            }
        }
    }
}
