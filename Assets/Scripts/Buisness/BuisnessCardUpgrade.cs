using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuisnessCardUpgrade : MonoBehaviour
{
    [SerializeField] private Button firstUpgradeButton;
    [SerializeField] private TextMeshProUGUI firstUpgradeName;
    [SerializeField] private TextMeshProUGUI firstUpgradeIncomeModifier;
    [SerializeField] private TextMeshProUGUI firstUpgradePrice;
    
    public void SetFirstUpgradeButtonInteractable(bool interactable)
    {
        firstUpgradeButton.interactable = interactable;
    }
    
    public void SetName(string name)
    {
        firstUpgradeName.text = name;
    }    
    
    public void SetFirstUpgradeIncomeModifier(float modifier)
    {
        firstUpgradeName.text = "Доход: {modifier:0.0}%";
    }

    public void SetFirstUpgradePrice(float price)
    {
        firstUpgradePrice.text = "Цена: {price:0.0}$}";
    }
}
