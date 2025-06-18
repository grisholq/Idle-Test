using Leopotam.EcsLite;

public class BalanceDisplaySystem : IEcsInitSystem, IEcsRunSystem
{
    private BalanceDisplayMono balanceDisplayMono;
    private EcsWorld world;
    private EcsPool<Balance> balancePool;
    private EcsPool<BalanceDisplay> balanceDisplayPool;
    private EcsFilter balanceFilter;

    public BalanceDisplaySystem(BalanceDisplayMono balanceDisplayMono)
    {
       this.balanceDisplayMono = balanceDisplayMono; 
    }
    
    public void Init(IEcsSystems systems)
    { 
        world = systems.GetWorld();
        balancePool = world.GetPool<Balance>();
        balanceDisplayPool = world.GetPool<BalanceDisplay>();
        balanceFilter = world.Filter<Balance>().Inc<BalanceDisplay>().End();
        
        var balanceEntity = world.NewEntity();
        ref var balance = ref balancePool.Add(balanceEntity);
        ref var balanceDisplay = ref balanceDisplayPool.Add(balanceEntity);
        
        balance.Current = 100;
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
}