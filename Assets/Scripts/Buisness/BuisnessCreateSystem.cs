using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class BuisnessCreateSystem : IEcsInitSystem
{
    private BuisnessCoreConfig buisnessCoreConfig;             
    private BuisnessCosmeticConfig buisnessCosmeticConfig;   
    private BuisnessCardListDisplay buisnessCardListDisplay;
    private EcsWorld world;
    private EcsPool<Buisness> buisnessPool;
    private EcsPool<BuisnessCoreData> buisnessCoreConfigPool;
    private EcsPool<BuisnessCosmeticData> buisnessCosmeticConfigPool;
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
        buisnessCoreConfigPool = world.GetPool<BuisnessCoreData>();
        buisnessCosmeticConfigPool = world.GetPool<BuisnessCosmeticData>();
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
        ref var coreDataInstance = ref buisnessCoreConfigPool.Add(buisnessEntity);
        ref var cosmeticDataInstance = ref buisnessCosmeticConfigPool.Add(buisnessEntity);
        ref var cardComponentInstance = ref buisnessCardComponentPool.Add(buisnessEntity);

        buisnessCreated = buisness;
        coreDataInstance = coreData;
        cosmeticDataInstance = cosmeticData;
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

        for (int i = 0; i < buisnessCount; i++)
        {
            saveData.Buisnesses.Add(Buisness.Default);
        }

        return saveData;
    }
}
