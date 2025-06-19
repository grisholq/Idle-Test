using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class BuisnessLevelUpSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsPool<Balance> balancePool;
    private EcsPool<Buisness> buisnessPool;
    private EcsPool<BuisnessCardComponent> buisnessCardPool;
    private EcsPool<BuisnessCoreData> buisnessCoreDataPool;
    private EcsPool<BuisnessLevelUpEvent> buisnessLevelUpEventPool;
    private EcsFilter balanceFilter;
    private EcsFilter buisnessFilter;
    private EcsFilter buisnessLevelUpFilter;
    
    public void Init(IEcsSystems systems)
    {
        world = systems.GetWorld();
        
        balancePool = world.GetPool<Balance>();
        buisnessPool = world.GetPool<Buisness>();
        buisnessCardPool = world.GetPool<BuisnessCardComponent>();
        buisnessCoreDataPool = world.GetPool<BuisnessCoreData>();
        buisnessLevelUpEventPool = world.GetPool<BuisnessLevelUpEvent>();
        
        balanceFilter = world.Filter<Balance>().End();
        buisnessFilter = world.Filter<Buisness>().Inc<BuisnessCoreData>().Inc<BuisnessCardComponent>().End();
        buisnessLevelUpFilter = world.Filter<Buisness>().Inc<BuisnessCoreData>().Inc<BuisnessCardComponent>().Inc<BuisnessLevelUpEvent>().End();


        foreach (var b in balanceFilter)
        {
            ref var balance = ref balancePool.Get(b);
            
            foreach (var e in buisnessFilter)
            {
                ref var buisness = ref buisnessPool.Get(e);
                var buisnessCard = buisnessCardPool.Get(e).Card;
                ref var buisnessCore = ref buisnessCoreDataPool.Get(e);
                var levelUpCost = CalculateLevelUpCost(buisness, buisnessCore);
                var income = Buisness.CalculateIncome(buisness, buisnessCore);
                
                buisnessCard.SetLevel(buisness.Level);
                buisnessCard.SetIncome(income);
                buisnessCard.SetLevelUpCost(levelUpCost);
                buisnessCard.SetLevelUpButtonInteractable(balance.Current >= levelUpCost);
                buisnessCard.SetLevelUpAction(() => buisnessLevelUpEventPool.Add(e));
            }
        }
    }
    
    private float CalculateLevelUpCost(Buisness buisness, BuisnessCoreData coreData)
    {
        return (buisness.Level + 1) * coreData.BaseCost;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var b in balanceFilter)
        {
            ref var balance = ref balancePool.Get(b);
            
            HandleLevelUp(ref balance, b);
            HandleLevelUpButtonUpdate(balance);
        }
    }

    private void HandleLevelUp(ref Balance balance, int balanceEntity)
    {
        foreach (var e in buisnessLevelUpFilter)
        {
            ref var buisness = ref buisnessPool.Get(e);
            var buisnessCard = buisnessCardPool.Get(e).Card;
            ref var buisnessCore = ref buisnessCoreDataPool.Get(e);
            var levelUpCost = CalculateLevelUpCost(buisness, buisnessCore);
            
            ref var spendEvent = ref world.GetPool<BalanceSpendEvent>().Add(balanceEntity);
            spendEvent.Amount += levelUpCost;
            buisnessLevelUpEventPool.Del(e);

            buisness.Level++;
            var income = Buisness.CalculateIncome(buisness, buisnessCore);    
            
            buisnessCard.SetLevel(buisness.Level);
            buisnessCard.SetIncome(income);
            buisnessCard.SetLevelUpCost(CalculateLevelUpCost(buisness, buisnessCore));
            buisnessCard.SetLevelUpButtonInteractable(balance.Current >= levelUpCost);
        }
    }

    private void HandleLevelUpButtonUpdate(Balance balance)
    {
        foreach (var e in buisnessFilter)
        {
            ref var buisness = ref buisnessPool.Get(e);
            var buisnessCard = buisnessCardPool.Get(e).Card;
            ref var buisnessCore = ref buisnessCoreDataPool.Get(e);
            var levelUpCost = CalculateLevelUpCost(buisness, buisnessCore);
            
            buisnessCard.SetLevelUpButtonInteractable(balance.Current >= levelUpCost);
        }
    }
}
