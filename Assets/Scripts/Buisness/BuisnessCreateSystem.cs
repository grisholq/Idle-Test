using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class BuisnessCreateSystem : IEcsInitSystem, IEcsDestroySystem
{
    private BuisnessCoreConfig buisnessCoreConfig;             
    private BuisnessCosmeticConfig buisnessCosmeticConfig;   
    private BuisnessCardListDisplay buisnessCardListDisplay;
    private EcsWorld world;
    private EcsPool<Buisness> buisnessPool;
    private EcsPool<BuisnessData> buisnessData;
    private EcsPool<BuisnessCardComponent> buisnessCardComponentPool;
    
    private const string SaveKey = "BuisnessSaveData";
    
    public BuisnessCreateSystem(BuisnessCoreConfig buisnessCoreConfig, BuisnessCosmeticConfig buisnessCosmeticConfig, BuisnessCardListDisplay buisnessCardListDisplay)
    {
        this.buisnessCoreConfig = buisnessCoreConfig;
        this.buisnessCosmeticConfig = buisnessCosmeticConfig;
        this.buisnessCardListDisplay = buisnessCardListDisplay;
    }
    
    public void Init(IEcsSystems systems)
    {
        world = systems.GetWorld();
        buisnessPool = world.GetPool<Buisness>();
        buisnessData = world.GetPool<BuisnessData>();
        buisnessCardComponentPool = world.GetPool<BuisnessCardComponent>();
        
        var cards = buisnessCardListDisplay.CreateCards(buisnessCoreConfig.BuisnessesCount);
        var saveData = LoadBuisnessSaveData();
        
        for (int i = 0; i < buisnessCoreConfig.BuisnessesCount; i++)
        {
            var coreData = buisnessCoreConfig.BuisnessesCoreData[i];
            var cosmeticData = buisnessCosmeticConfig.BuisnessCosmeticDatas[i];
            CreateBuisness(coreData, cosmeticData, saveData.Buisnesses[i], cards[i]);
        }
    }

    private void CreateBuisness(BuisnessCoreData coreData, BuisnessCosmeticData cosmeticData, Buisness buisness, BuisnessCard card)
    {
        int buisnessEntity = world.NewEntity();
        ref var buisnessCreated = ref buisnessPool.Add(buisnessEntity);
        ref var data = ref buisnessData.Add(buisnessEntity);
        ref var cardComponentInstance = ref buisnessCardComponentPool.Add(buisnessEntity);

        buisnessCreated = buisness;
        data.Name = cosmeticData.Name;
        data.Core = coreData;
        data.Core.FirstUpgrade.Name = cosmeticData.FirstUpgradeName;
        data.Core.SecondUpgrade.Name = cosmeticData.SecondUpgradeName;
        cardComponentInstance.Card = card;
    }

    private BuisnessesSaveData LoadBuisnessSaveData()
    {
        if (PlayerPrefs.HasKey(SaveKey) == false)
        {
            return CreateBuisnessSaveData(buisnessCoreConfig.BuisnessesCount);
        }
        
        return JsonUtility.FromJson<BuisnessesSaveData>(PlayerPrefs.GetString(SaveKey));
    }

    private BuisnessesSaveData CreateBuisnessSaveData(int buisnessCount)
    {
        BuisnessesSaveData saveData = new BuisnessesSaveData();
        
        saveData.Buisnesses.Add(Buisness.Starting);

        for (int i = 0; i < buisnessCount - 1; i++)
        {
            saveData.Buisnesses.Add(Buisness.Default);
        }

        return saveData;
    }

    public void Destroy(IEcsSystems systems)
    {
        var buisnessesFilter = world.Filter<Buisness>().End();
        BuisnessesSaveData saveData = new BuisnessesSaveData();
        
        foreach (var b in buisnessesFilter)
        {
            ref var buisness = ref buisnessPool.Get(b);
            
            saveData.Buisnesses.Add(buisness);
        }
        
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(saveData));
    }
}
