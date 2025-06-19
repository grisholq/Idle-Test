using System;

[Serializable]
public struct BuisnessCoreData
{
    public float IncomeDelay;
    public float BaseCost;
    public float BaseIncome;
    public BuisnessUpgradeData FirstUpgrade;
    public BuisnessUpgradeData SecondUpgrade;
}
