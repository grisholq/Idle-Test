using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Buisness
{
    public int Level;
    public List<BuisnessUpgradeData> ActiveUpgrades;
    public float IncomeTime;
    public bool Purchased => Level > 0;

    public static Buisness Starting => new Buisness() { Level = 1, ActiveUpgrades = new List<BuisnessUpgradeData>(), IncomeTime = 0f };
    public static Buisness Default => new Buisness() { Level = 0, ActiveUpgrades = new List<BuisnessUpgradeData>(), IncomeTime = 0f };
    
    public static float CalculateIncome(Buisness buisness, BuisnessCoreData coreData)
    {
        float incomeModifier = 0;

        foreach (var upgrade in buisness.ActiveUpgrades)
        {
            incomeModifier += upgrade.IncomeModifier;
        }

        return buisness.Level * coreData.BaseIncome * (1 + incomeModifier);
    }
}
