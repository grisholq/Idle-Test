using System;

[Serializable]
public struct BuisnessUpgradeData : IEquatable<BuisnessUpgradeData>
{
    public string Name { get; set; }
    public float Price;
    public float IncomeModifier;

    public bool Equals(BuisnessUpgradeData other)
    {
        return IncomeModifier == other.IncomeModifier && Price == other.Price;
    }
}
