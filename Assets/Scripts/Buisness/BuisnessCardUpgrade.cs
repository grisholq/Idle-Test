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
    
    public BuisnessUpgradeData UpgradeData { get; set; }
    
    public void SetButtonInteractable(bool interactable)
    {
        firstUpgradeButton.interactable = interactable;
    }
    
    public void SetName(string name)
    {
        firstUpgradeName.text = name;
    }    
    
    public void SetIncomeModifier(float modifier)
    {
        modifier *= 100;
        firstUpgradeIncomeModifier.text = $"Доход: {modifier}%";
    }

    public void SetPrice(float price)
    {
        firstUpgradePrice.text = $"Цена: {price:0.0}$";
    }

    public void SetPurchasedCaption()
    {
        firstUpgradePrice.text = "Куплено";
    }
}
