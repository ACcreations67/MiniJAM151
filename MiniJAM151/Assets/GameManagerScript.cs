using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static List<string> cardConversionTable = new() { "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "10S", "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "10C", "2D", "3D", "4D", "5D", "6D", "8D", "9D", "10D", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "AC", "AD", "AH" };
    public static List<string> powerUpConversionTable = new() { "A", "JS", "QS", "KS", "JC", "QC", "KC", "JD", "QD", "KD", "JH", "QH", "KH"};

    public List<int> playerCards;
    public List<int> playerPowerups;
    public int playerActiveCard;
    public int playerPoints = 0;
    public bool playerShreded = false;
    public bool playerAtWar = false;


    public List<int> bot1Cards;
    public List<int> bot1Powerups;
    public int bot1ActiveCard;
    public int bot1Points = 0;
    public bool bot1Shreded = false;
    bool bot1AtWar = false;

    public int bot2Points = 0;
    public List<int> bot2Cards;
    public List<int> bot2Powerups;
    public int bot2ActiveCard;
    public bool bot2Shreded = false;
    bool bot2AtWar = false;

    public int bot3Points = 0;
    public List<int> bot3Cards;
    public List<int> bot3Powerups;
    public int bot3ActiveCard;
    public bool bot3Shreded = false;
    bool bot3AtWar = false;

    public int bot4Points = 0;
    public List<int> bot4Cards;
    public List<int> bot4Powerups;
    public int bot4ActiveCard;
    public bool bot4Shreded = false;
    bool bot4AtWar = false;

    List<int> pot = new List<int>() { };
    private List<int> pickedCards = new() { };
    public List<int> cardsInPlay = new() { };

    private bool waiting = true;

    private List<int> powerUpDeck;

    // Start is called before the first frame update
    void Start()
    {
        shufflePowerupDeck();
        dealCards();
        logCards();
        reset();
        waiting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!waiting)
        {
            aiPlays();
            logCards();
            getWinner();
            reset();
            waiting = true;
        }
    }

    public void dealCards()
    {
        playerCards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
        bot1Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
        bot2Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
        bot3Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
        bot4Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
    }

    private void logCards()
    {
        Debug.Log(cardConversionTable[playerActiveCard]);
    }

    private int pickCard()
    {
        bool cardChecked = false;
        int card = -1;
        while (!cardChecked)
        {
            card = Random.Range(0, 38);
            if (!pickedCards.Contains(card))
            {
                cardChecked = true;
            }
        }
        pickedCards.Add(card);
        return card;
    }

    public void reset()
    {
        if (playerCards.Count > 0)
        {
            playerActiveCard = playerCards[Random.Range(0, playerCards.Count)];
            playerCards.Remove(playerActiveCard);
        }
        else
        {
            playerActiveCard = -1;
        }

        if (bot1Cards.Count > 0)
        {
            bot1ActiveCard = bot1Cards[Random.Range(0, bot1Cards.Count)];
            bot1Cards.Remove(bot1ActiveCard);
        }
        else
        {
            bot1ActiveCard = -1;
        }

        if (bot2Cards.Count > 0)
        {
            bot2ActiveCard = bot2Cards[Random.Range(0, bot2Cards.Count)];
            bot2Cards.Remove(bot2ActiveCard);
        }
        else
        {
            bot2ActiveCard = -1;
        }

        if (bot3Cards.Count > 0)
        {
            bot3ActiveCard = bot3Cards[Random.Range(0, bot3Cards.Count)];
            bot3Cards.Remove(bot3ActiveCard);
        }
        else
        {
            bot3ActiveCard = -1;
        }

        if (bot4Cards.Count > 0)
        {
            bot4ActiveCard = bot4Cards[Random.Range(0, bot4Cards.Count)];
            bot4Cards.Remove(bot4ActiveCard);
        }
        else
        {
            bot4ActiveCard = -1;
        }

        pot.Clear();
    }

    public void shred()
    {
        if (waiting)
        {
            playerShreded = true;
            waiting = false;
        }
    }

    public void play()
    {
        if (waiting)
        {
            playerShreded = false;
            waiting = false;
        }
    }


    public void aiPlays()
    {
        // bot 1
        if (cardvalue(bot1ActiveCard) < Random.Range(3, 7))
        {
            bot1Shreded = true;
        }
        else
        {
            bot1Shreded = false;
        }

        if (cardvalue(bot2ActiveCard) < Random.Range(3, 7))
        {
            bot2Shreded = true;
        }
        else
        {
            bot2Shreded = false;
        }

        if (cardvalue(bot3ActiveCard) < Random.Range(3, 7))
        {
            bot3Shreded = true;
        }
        else
        {
            bot3Shreded = false;
        }

        if (cardvalue(bot4ActiveCard) < Random.Range(3, 7))
        {
            bot4Shreded = true;
        }
        else
        {
            bot4Shreded = false;
        }

    }

    public void getWinner()
    {
        if (playerShreded)
        {
            playerActiveCard = -1;
        }
        if (bot1Shreded)
        {
            bot1ActiveCard = -1;
        }
        if (bot2Shreded)
        {
            bot2ActiveCard = -1;
        }
        if (bot3Shreded)
        {
            bot3ActiveCard = -1;
        }
        if (bot4Shreded)
        {
            bot4ActiveCard = -1;
        }

        List<int> total = new() { cardvalue(playerActiveCard), cardvalue(bot1ActiveCard), cardvalue(bot2ActiveCard), cardvalue(bot3ActiveCard), cardvalue(bot4ActiveCard) };
        List<int> organized = new List<int> { total[0], total[1], total[2], total[3], total[4]};
        organized.Sort();

        if (playerActiveCard > -1)
        {
            pot.Add(playerActiveCard);
        }
        if (bot1ActiveCard > -1)
        {
            pot.Add(bot1ActiveCard);
        }
        if (bot2ActiveCard > -1)
        {
            pot.Add(bot2ActiveCard);
        }
        if (bot3ActiveCard > -1)
        {
            pot.Add(bot3ActiveCard);
        }
        if (bot4ActiveCard > -1)
        {
            pot.Add(bot4ActiveCard);
        }

        if (organized[4] == organized[3])
        {
            int warWager = 3;
            List<int> warPlayPot = new List<int> { };

            if (organized[4] == total[0])
            {
                if (playerCards.Count <= warWager)
                {
                    warWager = playerCards.Count - 1;
                }

                playerAtWar = true;
            }
            if (organized[4] == total[1])
            {
                if (bot1Cards.Count <= warWager)
                {
                    warWager = bot1Cards.Count - 1;
                }

                bot1AtWar = true;
            }
            if (organized[4] == total[2])
            {
                if (bot2Cards.Count <= warWager)
                {
                    warWager = bot2Cards.Count - 1;
                }

                bot2AtWar = true;
            }
            if (organized[4] == total[3])
            {
                if (bot3Cards.Count <= warWager)
                {
                    warWager = bot3Cards.Count - 1;
                }

                bot3AtWar = true;
            }
            if (organized[4] == total[4])
            {
                if (bot4Cards.Count <= warWager)
                {
                    warWager = bot4Cards.Count - 1;
                }

                bot4AtWar = true;
            }

            if (playerAtWar)
            {
                for (int i = warWager; i > 0; i--)
                {
                    pot.Add(pickCardFromHand(0));
                }
                playerActiveCard = pickCardFromHand(0);
                warPlayPot.Add(playerActiveCard);
            }
            else
            {
                playerActiveCard = -1;
            }
            if (bot1AtWar)
            {
                for (int i = warWager; i > 0; i--)
                {
                    pot.Add(pickCardFromHand(1));
                }
                bot1ActiveCard = pickCardFromHand(1);
                warPlayPot.Add(bot1ActiveCard);
            }
            else
            {
                bot1ActiveCard = -1;
            }
            if (bot2AtWar)
            {
                for (int i = warWager; i > 0; i--)
                {
                    pot.Add(pickCardFromHand(2));
                }
                bot2ActiveCard = pickCardFromHand(2);
                warPlayPot.Add(bot2ActiveCard);
            }
            else
            {
                bot2ActiveCard = -1;
            }
            if (bot3AtWar)
            {
                for (int i = warWager; i > 0; i--)
                {
                    pot.Add(pickCardFromHand(3));
                }
                bot3ActiveCard = pickCardFromHand(3);
                warPlayPot.Add(bot3ActiveCard);
            }
            else
            {
                bot3ActiveCard = -1;
            }
            if (bot4AtWar)
            {
                for (int i = warWager; i > 0; i--)
                {
                    pot.Add(pickCardFromHand(4));
                }
                bot4ActiveCard = pickCardFromHand(4);
                warPlayPot.Add(bot4ActiveCard);
            }
            else
            {
                bot4ActiveCard = -1;
            }

            warPlayPot.Sort();

            getWinner();
        }
        else
        {
            if (organized[4] == total[0])
            {
                Debug.Log(cardConversionTable[playerActiveCard]);
                Debug.Log("player wins");
                playerCards.AddRange(pot);
            }
            else if (organized[4] == total[1])
            {
                Debug.Log(cardConversionTable[bot1ActiveCard]);
                Debug.Log("bot1 wins");
                bot1Cards.AddRange(pot);
            }
            else if (organized[4] == total[2])
            {
                Debug.Log(cardConversionTable[bot2ActiveCard]);
                Debug.Log("bot2 wins");
                bot2Cards.AddRange(pot);
            }
            else if (organized[4] == total[3])
            {
                Debug.Log(cardConversionTable[bot3ActiveCard]);
                Debug.Log("bot3 wins");
                bot3Cards.AddRange(pot);
            }
            else if (organized[4] == total[4])
            {
                Debug.Log(bot4ActiveCard);
                Debug.Log(cardConversionTable[bot4ActiveCard]);
                Debug.Log("bot4 wins");
                bot4Cards.AddRange(pot);
            }
        }
    }

    private void war()
    {

    }

    private int pickCardFromHand(int playerID)
    {
        int card = -1;

        if (playerID == 0)
        {
            if (playerCards.Count <= 0)
            {
                return -1;
            }

            card = playerCards[Random.Range(0, playerCards.Count)];
            playerCards.Remove(card);
        }
        else if (playerID == 1)
        {
            if (bot1Cards.Count <= 0)
            {
                return -1;
            }

            card = bot1Cards[Random.Range(0, bot1Cards.Count)];
            bot1Cards.Remove(card);
        }
        else if (playerID == 2)
        {
            if (bot2Cards.Count <= 0)
            {
                return -1;
            }

            card = bot2Cards[Random.Range(0, bot2Cards.Count)];
            bot2Cards.Remove(card);
        }
        else if (playerID == 3)
        {
            if (bot3Cards.Count <= 0)
            {
                return -1;
            }

            card = bot3Cards[Random.Range(0, bot3Cards.Count)];
            bot3Cards.Remove(card);
        }
        else if (playerID == 4)
        {
            if (bot4Cards.Count <= 0)
            {
                return -1;
            }

            card = bot4Cards[Random.Range(0, bot4Cards.Count)];
            bot4Cards.Remove(card);
        }

        return card;
    }

    private int cardvalue(int cardID)
    {
        if (cardID < 0)
        {
            return -1;
        }
        else if (cardID <= 30)
        {
            return cardID % 10;
        }
        else
        {
            return 10;
        }
    }

    private void shufflePowerupDeck()
    {
        for (int x = 13; x > 0; x-- )
        {
            int num = Random.Range(0, x);
            powerUpDeck.Add(num);
        }
    }

    public void gameOver()
    {

    }
}
