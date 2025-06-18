using TMPro;
using UnityEngine;

public class BalanceDisplayMono : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI balance;
    
    public void SetBalance(float value)
    {
        balance.text = $"Баланс: {value}$";
    }
}