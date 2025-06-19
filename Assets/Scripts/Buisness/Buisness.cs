using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Buisness
{
    public int Level;
    public bool FirstUpgradePurchased;
    public bool SecondUpgradePurchased;
    public float IncomeTime;

    public static Buisness Starting => new Buisness() { Level = 1, FirstUpgradePurchased = false, SecondUpgradePurchased = false, IncomeTime = 0f };
    public static Buisness Default => new Buisness() { Level = 0, FirstUpgradePurchased = false, SecondUpgradePurchased = false, IncomeTime = 0f };
    public static float CalculateIncome(Buisness buisness, BuisnessCoreData coreData)
    {
        float incomeModifier = buisness.FirstUpgradePurchased ? coreData.FisrtUpgradeIncomeModifier : 0;
        incomeModifier += buisness.SecondUpgradePurchased ? coreData.SecondUpgradeIncomeModifier : 0;
        return buisness.Level * coreData.BaseIncome * (1 + incomeModifier);
    }
}
