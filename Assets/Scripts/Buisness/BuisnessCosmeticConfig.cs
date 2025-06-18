using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuisnessCosmeticConfig", menuName = "Configs/BuisnessCosmeticConfig")]
public class BuisnessCosmeticConfig : ScriptableObject
{
    public List<BuisnessCosmeticData> BuisnessCosmeticDatas;
}