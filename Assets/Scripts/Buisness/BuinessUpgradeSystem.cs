using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class BuinessUpgradeSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld world;
    private EcsPool<Balance> balancePool;
    private EcsPool<Buisness> buisnessPool;
    private EcsPool<BuisnessData> buisnessDataPool;
    private EcsPool<BuisnessCardComponent> buisnessCardComponentPool;
    private EcsPool<BalanceSpendEvent> balanceSpendEventPool;
    private EcsPool<BuisnessUpgradeEvent> buisnessUpgradeEventPool;

    private EcsFilter buisnessFilter;
    private EcsFilter buisnessUpgradeFilter;
    private EcsFilter balanceFilter;
    
    public void Init(IEcsSystems systems)
    {
        world = systems.GetWorld();
        balancePool = world.GetPool<Balance>();
        buisnessPool = world.GetPool<Buisness>();
        buisnessDataPool = world.GetPool<BuisnessData>();
        buisnessCardComponentPool = world.GetPool<BuisnessCardComponent>();
        balanceSpendEventPool = world.GetPool<BalanceSpendEvent>();
        buisnessUpgradeEventPool = world.GetPool<BuisnessUpgradeEvent>();
        
        buisnessFilter = world.Filter<Buisness>().Inc<BuisnessData>().Inc<BuisnessCardComponent>().End();
        buisnessUpgradeFilter = world.Filter<Buisness>().Inc<BuisnessData>().Inc<BuisnessCardComponent>().Inc<BuisnessUpgradeEvent>().End();
        balanceFilter = world.Filter<Balance>().End();

        foreach (var b in balanceFilter)
        {
            ref var balance = ref balancePool.Get(b);
            
            foreach (var e in buisnessFilter)
            {
                ref var buisness = ref buisnessPool.Get(e);
                ref var buisnessData = ref buisnessDataPool.Get(e);
                var buisnessDataNR = buisnessDataPool.Get(e);
                var buisnessCard = buisnessCardComponentPool.Get(e).Card;

                DisplayUpgradeData(buisness, buisnessData.Core.FirstUpgrade, balance, buisnessCard.FirstUpgrade); 
                DisplayUpgradeData(buisness, buisnessData.Core.SecondUpgrade, balance, buisnessCard.SecondUpgrade); 
                
                buisnessCard.SetFirstUpgradeAction(() =>
                {
                    ref var upgradeEvent = ref buisnessUpgradeEventPool.Add(e);
                    upgradeEvent.Upgrade = buisnessDataNR.Core.FirstUpgrade;
                });
                
                buisnessCard.SetSecondUpgradeAction(() =>
                {
                    ref var upgradeEvent = ref buisnessUpgradeEventPool.Add(e);
                    upgradeEvent.Upgrade = buisnessDataNR.Core.SecondUpgrade;
                });
            }
        }
    }

    private void DisplayUpgradeData(Buisness buisness, BuisnessUpgradeData upgradeData, Balance balance, BuisnessCardUpgrade upgradeUI)
    {
        upgradeUI.UpgradeData = upgradeData;
        upgradeUI.SetName(upgradeData.Name);
        upgradeUI.SetIncomeModifier(upgradeData.IncomeModifier);
        
        if (buisness.ActiveUpgrades.Contains(upgradeData))
        {
            upgradeUI.SetPurchasedCaption();
            upgradeUI.SetButtonInteractable(false);
            return;
        }

        upgradeUI.SetPrice(upgradeData.Price);
        upgradeUI.SetButtonInteractable(upgradeData.Price <= balance.Current);
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
                var card = buisnessCardComponentPool.Get(e).Card;

                foreach (var upgrade in card.BuisnessCardUpgradeList)
                {
                    upgrade.SetButtonInteractable(!buisness.ActiveUpgrades.Contains(upgrade.UpgradeData) 
                                                  && balance.Current >= upgrade.UpgradeData.Price && buisness.Purchased);
                }
            }

            foreach (var e in buisnessUpgradeFilter)
            {
                ref var buisness = ref buisnessPool.Get(e);
                ref var buisnessData = ref buisnessDataPool.Get(e);
                ref var upgradeEvent = ref buisnessUpgradeEventPool.Get(e);
                var card = buisnessCardComponentPool.Get(e).Card;

                foreach (var upgradeUI in card.BuisnessCardUpgradeList)
                {
                    var upgradeData = upgradeUI.UpgradeData;

                    if (!upgradeData.Equals(upgradeEvent.Upgrade)) continue;
                    
                    buisness.ActiveUpgrades.Add(upgradeData);
                    
                    upgradeUI.SetButtonInteractable(false);
                    upgradeUI.SetPurchasedCaption();
                    
                    ref var spendEvent = ref balanceSpendEventPool.Add(b);
                    spendEvent.Amount += upgradeData.Price;
                    buisnessUpgradeEventPool.Del(e);
                    
                    card.SetIncome(Buisness.CalculateIncome(buisness, buisnessData.Core));
                    break;
                }
            }
        }
    }
}