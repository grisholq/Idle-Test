using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuisnessCoreConfig", menuName = "Configs/BuisnessCoreConfig")]
public class BuisnessCoreConfig : ScriptableObject
{
    public int BuisnessesCount = 5;
    public List<BuisnessCoreData> BuisnessesCoreData;
}