using Leopotam.EcsLite;

public class BuisnessCosmeticsInitSystem : IEcsInitSystem
{
    private EcsWorld world;
    private EcsPool<Buisness> buisnessPool;
    private EcsPool<BuisnessCardComponent> buisnessCardPool;
    private EcsPool<BuisnessData> buisnessDataPool;
    private EcsFilter buisnessFilter;
    
    public void Init(IEcsSystems systems)
    {
        world = systems.GetWorld();
        buisnessPool = world.GetPool<Buisness>();
        buisnessCardPool = world.GetPool<BuisnessCardComponent>();
        buisnessDataPool = world.GetPool<BuisnessData>();

        buisnessFilter = world.Filter<BuisnessData>().Inc<BuisnessCardComponent>().End();
        
        foreach (var e in buisnessFilter)
        {
            var buisnessCard = buisnessCardPool.Get(e).Card;
            ref var buisnessData = ref buisnessDataPool.Get(e);
            
            buisnessCard.SetName(buisnessData.Name);
            buisnessCard.FirstUpgrade.SetName(buisnessData.Core.FirstUpgrade.Name);
            buisnessCard.SecondUpgrade.SetName(buisnessData.Core.SecondUpgrade.Name);
        }

    }
}
