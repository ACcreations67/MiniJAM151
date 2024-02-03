using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] int BUYINVALUE;
    public List<string> cardConversionTable = new() { "AS", "2S", "3S", "4S", "5S", "6S", "AS", "8S", "9S", "10S", "JS", "QS", "KS", "AC", "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "10C", "JC", "QC", "KC", "AH", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "JH", "QH", "KH", "AD", "2D", "3D", "4D", "5D", "6D", "7D", "8D", "9D", "10D", "JD", "QD", "KD"};

    public List<int> playerCards;
    public int playerChips = 20;
    public int playerBet = 0;
    public bool playerFolded = false;

    public List<int> bot1Cards;
    public int bot1Chips = 20;
    public int bot1Bet;
    public List<int> bot2Cards;
    public int bot2Chips = 20;
    public int bot2Bet;
    public List<int> bot3Cards;
    public int bot3Chips = 20;
    public List<int> bot4Cards;
    public int bot4Chips = 20;

    private List<int> pickedCards = new() { };
    public int currentBet = 1;
    public int pot = 0;

    private bool waiting = true;

    // Start is called before the first frame update
    void Start()
    {
        dealCards();
        logCards();
        waiting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!waiting)
        {
            aiPlays();
            logCards();
            waiting = true;
        }
    }

    public void dealCards()
    {
        playerCards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
        bot1Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
        bot2Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
        bot3Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
        bot4Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard() };
    }

    private void logCards()
    {
        foreach (int x in playerCards)
        {
            Debug.Log(cardConversionTable[x]);
        }
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

    public void buyIn()
    {
        if (playerChips - BUYINVALUE < 0)
        {
            gameOver();
        }
        else
        {
            playerChips -= BUYINVALUE;
        }
    }

    public void call()
    {
        if (playerChips - (currentBet - playerBet) < 0)
        {
            
        }
        else
        {
            playerChips -= currentBet - playerBet;
            playerBet = currentBet;
        }
    }

    public void fold()
    {
        playerFolded = true;
    }

    public void raise(int raiseAmount)
    {
        if (playerChips - (currentBet + raiseAmount) - playerBet >= 0)
        {
            currentBet += raiseAmount;
            playerChips -= currentBet - playerBet;
        }
        else
        {
            Debug.Log("can't do that");
        }
    }

    public void aiPlays()
    {

    }

    public void gameOver()
    {

    }
}
