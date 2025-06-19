using Leopotam.EcsLite;

public class BuisnessCosmeticsInitSystem : IEcsInitSystem
{
    private EcsWorld world;
    private EcsPool<Buisness> buisnessPool;
    private EcsPool<BuisnessCardComponent> buisnessCardPool;
    private EcsPool<BuisnessCosmeticData> buisnessCosmeticDataPool;
    private EcsFilter buisnessFilter;
    
    public void Init(IEcsSystems systems)
    {
        world = systems.GetWorld();

        buisnessFilter = world.Filter<BuisnessCosmeticData>().Inc<BuisnessCardComponent>().End();
        
        foreach (var e in buisnessFilter)
        {
            var buisnessCard = buisnessCardPool.Get(e).Card;
            ref var buisnessCosmetics = ref buisnessCosmeticDataPool.Get(e);
            
            buisnessCard.SetName(buisnessCosmetics.Name);
            buisnessCard.FirstUpgrade.SetName(buisnessCosmetics.FirstUpgradeName);
            buisnessCard.SecondUpgrade.SetName(buisnessCosmetics.SecondUpgradeName);
        }

    }
}
