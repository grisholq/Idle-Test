using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuisnessCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buisnessName;
    [SerializeField] private Slider incomeProgress;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI income;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private TextMeshProUGUI levelUpPrice;
    
    [SerializeField] private BuisnessCardUpgrade firstUpgrade;
    [SerializeField] private BuisnessCardUpgrade secondUpgrade;
    
    public BuisnessCardUpgrade FirstUpgrade => firstUpgrade;
    public BuisnessCardUpgrade SecondUpgrade => secondUpgrade;

    private Action onLevelUp, onFirstUpgrade, onSecondUpgrade;
    
    public void SetActions(Action onLevelUp, Action onFirstUpgrade, Action onSecondUpgrade)
    {
        this.onLevelUp = onLevelUp;
        this.onFirstUpgrade = onFirstUpgrade;
        this.onSecondUpgrade = onSecondUpgrade;
    }

    public void OnLevelUp()
    {
        onLevelUp?.Invoke();
    }

    public void OnFirstUpgrade()
    {
        onFirstUpgrade?.Invoke();
    }

    public void OnSecondUpgrade()
    {
        onSecondUpgrade?.Invoke();
    }
    
    public void SetName(string name)
    {
        buisnessName.text = name;
    }

    public void SetProgress(float progress)
    {
        incomeProgress.value = progress;
    }

    public void SetLevel(int level)
    {
        this.level.text = $"Level: {level}";
    }

    public void SetIncome(float income)
    {
        this.income.text = $"Доход: {income:0.0}$";
    }

    public void SetLevelUpButtonInteractable(bool interactable)
    {
        levelUpButton.interactable = interactable;
    }
    
    public void SetLevelUpPrice(float price)
    {
        levelUpPrice.text = $"Цена: {price:0.0}$";
    }
}
