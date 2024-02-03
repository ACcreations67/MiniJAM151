using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public List<string> cardConversionTable = new List<string> {"AS", "2S", "3S", "4S", "5S", "6S", "AS", "8S", "9S", "10S", "JS", "QS", "KS", "AC", "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "10C", "JC", "QC", "KC", "AH", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "JH", "QH", "KH", "AD", "2D", "3D", "4D", "5D", "6D", "7D", "8D", "9D", "10D", "JD", "QD", "KD"};

    public List<int> playerCards;
    public List<int> bot1Cards;
    public List<int> bot2Cards;
    public List<int> bot3Cards;
    public List<int> bot4Cards;

    private List<int> pickedCards = new List<int> { };

    // Start is called before the first frame update
    void Start()
    {
        playerCards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
        bot1Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
        bot2Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
        bot3Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
        bot4Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int pickCard()
    {
        bool cardChecked = false;
        int card = -1;
        while (!cardChecked)
        {
            card = Random.Range(0, 53);
            if (!pickedCards.Contains(card))
            {
                cardChecked = true;
            }
        }
        pickedCards.Add(card);
        return card;
    }
}
