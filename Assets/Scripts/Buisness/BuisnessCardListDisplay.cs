using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuisnessCardListDisplay : MonoBehaviour
{
    [SerializeField] private BuisnessCard buisnessCardPrefab;
    [SerializeField] private Transform cardsParent;

    public List<BuisnessCard> CreateCards(int cardsAmount)
    {
        List<BuisnessCard> cards = new List<BuisnessCard>();
        
        for (int i = 0; i < cardsAmount; i++)
        {
            var cardInstance = Instantiate(buisnessCardPrefab, cardsParent);
            cards.Add(cardInstance);
        }
        
        return cards;
    }
}