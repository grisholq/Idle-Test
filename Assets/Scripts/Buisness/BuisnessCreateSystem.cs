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
        var cards = buisnessCardListDisplay.CreateCards(buisnessCoreConfig.BuisnessesCount);
        var saveData = LoadBuisnessSaveData();
        
        for (int i = 0; i < buisnessCoreConfig.BuisnessesCount; i++)
        {
            var coreData = buisnessCoreConfig.BuisnessesCoreData[i];
            var cosmeticData = buisnessCosmeticConfig.BuisnessCosmeticDatas[i];
            CreateBuisness(coreData, cosmeticData, saveData.Levels[i], cards[i]);
        }
    }

    private void CreateBuisness(BuisnessCoreData coreData, BuisnessCosmeticData cosmeticData, int level, BuisnessCard card)
    {
        int buisnessEntity = world.NewEntity();
        ref var buisness = ref buisnessPool.Add(buisnessEntity);
        ref var coreDataInstance = ref buisnessCoreConfigPool.Add(buisnessEntity);
        ref var cosmeticDataInstance = ref buisnessCosmeticConfigPool.Add(buisnessEntity);

        coreDataInstance = coreData;
        cosmeticDataInstance = cosmeticData;
        
        buisness.Level = level;
    }

    private BuisnessSaveData LoadBuisnessSaveData()
    {
        if (PlayerPrefs.HasKey(SaveKey) == false)
        {
            return CreateBuisnessSaveData(buisnessCoreConfig.BuisnessesCount);
        }
        
        return JsonUtility.FromJson<BuisnessSaveData>(PlayerPrefs.GetString(SaveKey));
    }

    private BuisnessSaveData CreateBuisnessSaveData(int buisnessCount)
    {
        BuisnessSaveData saveData = new BuisnessSaveData { Levels = new List<int> { 1 } };
        
        for (int i = 0; i < buisnessCount - 1; i++)
        {
            saveData.Levels.Add(0);
        }
        
        return saveData;
    }
}
