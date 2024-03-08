using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static List<string> cardConversionTable = new() { "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "10S", "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "10C", "2D", "3D", "4D", "5D", "6D", "8D", "9D", "10D", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "AC", "AD", "AH" };
    public static List<string> powerUpConversionTable = new() { "A", "JS", "JC", "JD", "JH", "QS", "QC", "QD", "QH", "KS", "KC", "KD", "KH"};

    #region Variables

    public List<int> playerCards;
    public List<int> playerPowerups;
    public int playerActiveCard;
    public int playerPoints = 0;
    public bool playerShreded = false;
    public bool playerAtWar = false;
    public bool playerPlayedJack = false;


    public List<int> bot1Cards;
    public List<int> bot1Powerups;
    public int bot1ActiveCard;
    public int bot1Points = 0;
    public bool bot1Shreded = false;
    public bool bot1AtWar = false;
    public bool bot1PlayedJack = false;


    public int bot2Points = 0;
    public List<int> bot2Cards;
    public List<int> bot2Powerups;
    public int bot2ActiveCard;
    public bool bot2Shreded = false;
    public bool bot2AtWar = false;
    public bool bot2PlayedJack = false;


    public int bot3Points = 0;
    public List<int> bot3Cards;
    public List<int> bot3Powerups;
    public int bot3ActiveCard;
    public bool bot3Shreded = false;
    public bool bot3AtWar = false;
    public bool bot3PlayedJack = false;

    public int bot4Points = 0;
    public List<int> bot4Cards;
    public List<int> bot4Powerups;
    public int bot4ActiveCard;
    public bool bot4Shreded = false;
    public bool bot4AtWar = false;
    public bool bot4PlayedJack = false;

    List<int> pot = new List<int>() { };
    private List<int> pickedCards = new() { };
    public List<int> cardsInPlay = new() { };

    private bool waiting = true;

    private List<int> powerUpDeck = new List<int> { };

    #endregion

    #region Start & Update

    // Start is called before the first frame update
    void Start()
    {
        shufflePowerupDeck();
        dealCards();
        resetCards();
        logCards();
        waiting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!waiting)
        {
            aiPlays();
            getWinner();
            resetCards();
            waiting = true;
            logCards();
        }
    }

    #endregion

    #region Functionality Functions

    public void dealCards()
    {
        playerCards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
        bot1Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
        bot2Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
        bot3Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
        bot4Cards = new List<int> { pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard(), pickCard()};
    }

    private void shufflePowerupDeck()
    {
        List<int> unShuffled = new List<int> { };
        unShuffled.AddRange(Enumerable.Range(0, 13));
        for (int i = unShuffled.Count; i > 0; i--)
        {
            powerUpDeck.Add(unShuffled[Random.Range(0, unShuffled.Count)]);
        }
    }

    public void resetCards()
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

        playerPlayedJack = false;
        bot1PlayedJack = false;
        bot2PlayedJack = false;
        bot3PlayedJack = false;
        bot4PlayedJack = false;

        pot.Clear();
    }

    public void aiPlays()
    {
        bot1BuysCards();
        bot2BuysCards();
        bot3BuysCards();
        bot4BuysCards();

        // bot 1
        if (cardvalue(bot1ActiveCard) < Random.Range(3, 7) && powerUpDeck.Count >= 0)
        {
            bot1LessThan();
        }
        else
        {
            if (botUsePowerUp(2, 1))
            {
                List<int> otherPlayers = new List<int>() { cardvalue(playerActiveCard), cardvalue(bot2ActiveCard), cardvalue(bot3ActiveCard), cardvalue(bot4ActiveCard) };

                if (otherPlayers.Max() >= cardvalue(bot1ActiveCard))
                {
                    bot1LessThan();
                    return;
                }
            }
            bot1Shreded = false;
        }

        //bot2
        if (cardvalue(bot2ActiveCard) < Random.Range(3, 7) && powerUpDeck.Count >= 0)
        {
            bot2LessThan();
        }
        else
        {
            if (botUsePowerUp(2, 2))
            {
                List<int> otherPlayers = new List<int>() { cardvalue(playerActiveCard), cardvalue(bot1ActiveCard), cardvalue(bot3ActiveCard), cardvalue(bot4ActiveCard) };

                if (otherPlayers.Max() >= cardvalue(bot2ActiveCard))
                {
                    bot2LessThan();
                    return;
                }
            }
            bot2Shreded = false;
        }

        //bot3
        if (cardvalue(bot3ActiveCard) < Random.Range(3, 7) && powerUpDeck.Count >= 0)
        {
            bot3LessThan();
        }
        else
        {
            if (botUsePowerUp(2, 3))
            {
                List<int> otherPlayers = new List<int>() { cardvalue(playerActiveCard), cardvalue(bot2ActiveCard), cardvalue(bot1ActiveCard), cardvalue(bot4ActiveCard) };

                if (otherPlayers.Max() >= cardvalue(bot3ActiveCard))
                {
                    bot3LessThan();
                    return;
                }
            }
            bot3Shreded = false;
        }

        //bot4
        if (cardvalue(bot4ActiveCard) < Random.Range(3, 7) && powerUpDeck.Count >= 0)
        {
            bot4LessThan();
        }
        else
        {
            if (botUsePowerUp(2, 4))
            {
                List<int> otherPlayers = new List<int>() { cardvalue(playerActiveCard), cardvalue(bot2ActiveCard), cardvalue(bot3ActiveCard), cardvalue(bot1ActiveCard) };

                if (otherPlayers.Max() >= cardvalue(bot4ActiveCard))
                {
                    bot4LessThan();
                    return;
                }
            }
            bot4Shreded = false;
        }

    }

    void bot1LessThan()
    {
        if (botUsePowerUp(0, 1))
        {
            bot1ActiveCard = -3;
        }
        else if (botUsePowerUp(1, 1))
        {
            bot1PlayedJack = true;

            List<int> otherPlayerCards = new List<int> { };
            List<int> organizedOtherPlayerCards = new List<int>();
            if (playerCards.Count > 0)
            {
                otherPlayerCards.Add(playerCards.Count);
            }
            if (bot2Cards.Count > 0)
            {
                otherPlayerCards.Add(bot2Cards.Count);
            }
            if (bot3Cards.Count > 0)
            {
                otherPlayerCards.Add(bot3Cards.Count);
            }
            if (bot4Cards.Count > 0)
            {
                otherPlayerCards.Add(bot4Cards.Count);
            }

            organizedOtherPlayerCards = otherPlayerCards;

            organizedOtherPlayerCards.Sort();

            int randomNum = Random.Range(0, 4);

            if (randomNum == 0)
            {
                int index = otherPlayerCards.FindLastIndex(a => a == organizedOtherPlayerCards[0]);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
            else if (randomNum == 2)
            {
                int index = Random.Range(0, organizedOtherPlayerCards.Count);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
            else
            {
                int index = otherPlayerCards.FindLastIndex(a => a == organizedOtherPlayerCards[organizedOtherPlayerCards.Count - 1]);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
        }
        bot1Shreded = true;
    }
    void bot2LessThan()
    {
        if (botUsePowerUp(0, 2))
        {
            bot2ActiveCard = -3;
        }
        else if (botUsePowerUp(1, 2))
        {
            bot1PlayedJack = true;

            List<int> otherPlayerCards = new List<int> { };
            List<int> organizedOtherPlayerCards = new List<int>();
            if (playerCards.Count > 0)
            {
                otherPlayerCards.Add(playerCards.Count);
            }
            if (bot1Cards.Count > 0)
            {
                otherPlayerCards.Add(bot2Cards.Count);
            }
            if (bot3Cards.Count > 0)
            {
                otherPlayerCards.Add(bot3Cards.Count);
            }
            if (bot4Cards.Count > 0)
            {
                otherPlayerCards.Add(bot4Cards.Count);
            }

            organizedOtherPlayerCards = otherPlayerCards;

            organizedOtherPlayerCards.Sort();

            int randomNum = Random.Range(0, 4);

            if (randomNum == 0)
            {
                int index = otherPlayerCards.FindLastIndex(a => a == organizedOtherPlayerCards[0]);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot1PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
            else if (randomNum == 2)
            {
                int index = Random.Range(0, organizedOtherPlayerCards.Count);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
            else
            {
                int index = otherPlayerCards.FindLastIndex(a => a == organizedOtherPlayerCards[organizedOtherPlayerCards.Count - 1]);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot1PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
        }
        bot2Shreded = true;
    }
    void bot3LessThan()
    {
        if (botUsePowerUp(0, 3))
        {
            bot3ActiveCard = -3;
        }
        else if (botUsePowerUp(1, 3))
        {
            bot3PlayedJack = true;

            List<int> otherPlayerCards = new List<int> { };
            List<int> organizedOtherPlayerCards = new List<int>();
            if (playerCards.Count > 0)
            {
                otherPlayerCards.Add(playerCards.Count);
            }
            if (bot2Cards.Count > 0)
            {
                otherPlayerCards.Add(bot2Cards.Count);
            }
            if (bot1Cards.Count > 0)
            {
                otherPlayerCards.Add(bot3Cards.Count);
            }
            if (bot4Cards.Count > 0)
            {
                otherPlayerCards.Add(bot4Cards.Count);
            }

            organizedOtherPlayerCards = otherPlayerCards;

            organizedOtherPlayerCards.Sort();

            int randomNum = Random.Range(0, 4);

            if (randomNum == 0)
            {
                int index = otherPlayerCards.FindLastIndex(a => a == organizedOtherPlayerCards[0]);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot1PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
            else if (randomNum == 2)
            {
                int index = Random.Range(0, organizedOtherPlayerCards.Count);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot1PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
            else
            {
                int index = otherPlayerCards.FindLastIndex(a => a == organizedOtherPlayerCards[organizedOtherPlayerCards.Count - 1]);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot1PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot4PlayedJack = true;
                }
            }
        }
        bot3Shreded = true;
    }
    void bot4LessThan()
    {
        if (botUsePowerUp(0, 4))
        {
            bot4ActiveCard = -3;
        }
        else if (botUsePowerUp(1, 4))
        {
            bot4PlayedJack = true;

            List<int> otherPlayerCards = new List<int> { };
            List<int> organizedOtherPlayerCards = new List<int>();
            if (playerCards.Count > 0)
            {
                otherPlayerCards.Add(playerCards.Count);
            }
            if (bot2Cards.Count > 0)
            {
                otherPlayerCards.Add(bot2Cards.Count);
            }
            if (bot3Cards.Count > 0)
            {
                otherPlayerCards.Add(bot3Cards.Count);
            }
            if (bot1Cards.Count > 0)
            {
                otherPlayerCards.Add(bot4Cards.Count);
            }

            organizedOtherPlayerCards = otherPlayerCards;

            organizedOtherPlayerCards.Sort();

            int randomNum = Random.Range(0, 4);

            if (randomNum == 0)
            {
                int index = otherPlayerCards.FindLastIndex(a => a == organizedOtherPlayerCards[0]);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot1PlayedJack = true;
                }
            }
            else if (randomNum == 2)
            {
                int index = Random.Range(0, organizedOtherPlayerCards.Count);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot1PlayedJack = true;
                }
            }
            else
            {
                int index = otherPlayerCards.FindLastIndex(a => a == organizedOtherPlayerCards[organizedOtherPlayerCards.Count - 1]);
                if (index == 0)
                {
                    playerPlayedJack = true;
                }
                else if (index == 1)
                {
                    bot2PlayedJack = true;
                }
                else if (index == 2)
                {
                    bot3PlayedJack = true;
                }
                else if (index == 3)
                {
                    bot1PlayedJack = true;
                }
            }
        }
        bot4Shreded = true;
    }

    public void getWinner()
    {
        if (playerShreded && playerActiveCard != -3)
        {
            if (playerPlayedJack)
            {
                playerActiveCard = -2;
            }
            else
            {
                playerPoints += cardvalue(playerActiveCard);
                playerActiveCard = -1;
            }
        }
        if (bot1Shreded && bot1ActiveCard != -3)
        {
            if (bot1PlayedJack)
            {
                bot1ActiveCard = -2;
            }
            else
            {
                bot1Points += cardvalue(bot1ActiveCard);
                bot1ActiveCard = -1;
            }
        }
        if (bot2Shreded && bot2ActiveCard != -3)
        {
            if (bot2PlayedJack)
            {
                bot2ActiveCard = -2;
            }
            else
            {
                bot2Points += cardvalue(bot2ActiveCard);
                bot2ActiveCard = -1;
            }
        }
        if (bot3Shreded && bot3ActiveCard != -3)
        {
            if (bot3PlayedJack)
            {
                bot3ActiveCard = -2;
            }
            else
            {
                bot3Points += cardvalue(bot3ActiveCard);
                bot3ActiveCard = -1;
            }
        }
        if (bot4Shreded && bot4ActiveCard != -3)
        {
            if (bot4PlayedJack)
            {
                bot4ActiveCard = -2;
            }
            else
            {
                bot4Points += cardvalue(bot4ActiveCard);
                bot4ActiveCard = -1;
            }
        }

        List<int> total = new() { cardvalue(playerActiveCard), cardvalue(bot1ActiveCard), cardvalue(bot2ActiveCard), cardvalue(bot3ActiveCard), cardvalue(bot4ActiveCard) };
        List<int> organized = new List<int> { total[0], total[1], total[2], total[3], total[4] };
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
            Debug.Log("war!");
            int warWager = 3;

            if (organized[4] == total[0])
            {
                if (playerCards.Count <= warWager)
                {
                    warWager = playerCards.Count - 1;
                }

                logCards();
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
            }
            else
            {
                bot4ActiveCard = -1;
            }

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

    void bot1BuysCards()
    {
        while (bot1Points >= 15)
        {
            int card = drawPowerup();
            if (card != -1)
            {
                bot1Powerups.Add(card);
                bot1Points -= 15;
            }
            else
            {
                break;
            }
        }
    }

    void bot2BuysCards()
    {
        while (bot2Points >= 15)
        {
            int card = drawPowerup();
            if (card != -1)
            {
                bot2Powerups.Add(card);
                bot2Points -= 15;
            }
            else
            {
                break;
            }
        }
    }

    void bot3BuysCards()
    {
        while (bot3Points >= 15)
        {
            int card = drawPowerup();
            if (card != -1)
            {
                bot3Powerups.Add(card);
                bot3Points -= 15;
            }
            else
            {
                break;
            }
        }
    }

    void bot4BuysCards()
    {
        while (bot4Points >= 15)
        {
            int card = drawPowerup();
            if (card != -1)
            {
                bot4Powerups.Add(card);
                bot4Points -= 15;
            }
            else
            {
                break;
            }
        }
    }

    #endregion

    #region Code Functions

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

    public int drawPowerup()
    {
        int card = -1;

        if (powerUpDeck.Count > 0)
        {
            card = powerUpDeck[0];
            powerUpDeck.Remove(card);
        }

        return card;
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
            if (cardID == -2)
            {
                return 13;
            }
            else if (cardID == -3)
            {
                return 14;
            }
            return -1;
        }
        else if (cardID <= 33)
        {
            return cardID % 11;
        }
        else
        {
            return 10;
        }
    }

    private bool botUsePowerUp(int powerup, int botID)
    {

        if (botID == 1)
        {
            if (botID == 0)
            {
                if (bot1Powerups.Contains(0))
                {
                    bot1Powerups.Remove(0);
                    return true;
                }
            }
            else 
            {
                List<int> possibleOptions = new List<int> { };
                possibleOptions.AddRange(Enumerable.Range(powerup, 4));
                List<int> powerUpOverlap = bot1Powerups.Intersect(possibleOptions).ToList<int>();
                if (powerUpOverlap.Count > 0)
                {
                    bot1Powerups.Remove(powerUpOverlap[0]);
                    return true;
                }
            }
        }
        else if (botID == 2)
        {
            if (botID == 0)
            {
                if (bot2Powerups.Contains(0))
                {
                    bot2Powerups.Remove(0);
                    return true;
                }
            }
            else
            {
                List<int> possibleOptions = new List<int> { };
                possibleOptions.AddRange(Enumerable.Range(powerup, 4));
                List<int> powerUpOverlap = bot2Powerups.Intersect(possibleOptions).ToList<int>();
                if (powerUpOverlap.Count > 0)
                {
                    bot2Powerups.Remove(powerUpOverlap[0]);
                    return true;
                }
            }
        }
        else if (botID == 3)
        {
            if (botID == 0)
            {
                if (bot3Powerups.Contains(0))
                {
                    bot3Powerups.Remove(0);
                    return true;
                }
            }
            else
            {
                List<int> possibleOptions = new List<int> { };
                possibleOptions.AddRange(Enumerable.Range(powerup, 4));
                List<int> powerUpOverlap = bot3Powerups.Intersect(possibleOptions).ToList<int>();
                if (powerUpOverlap.Count > 0)
                {
                    bot3Powerups.Remove(powerUpOverlap[0]);
                    return true;
                }
            }
        }
        else if (botID == 4)
        {
            if (botID == 0)
            {
                if (bot4Powerups.Contains(0))
                {
                    bot4Powerups.Remove(0);
                    return true;
                }
            }
            else
            {
                List<int> possibleOptions = new List<int> { };
                possibleOptions.AddRange(Enumerable.Range(powerup, 4));
                List<int> powerUpOverlap = bot4Powerups.Intersect(possibleOptions).ToList<int>();
                if (powerUpOverlap.Count > 0)
                {
                    bot4Powerups.Remove(powerUpOverlap[0]);
                    return true;
                }
            }
        }

        return false;
    }

    #endregion

    #region Player Input

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

    public void useJack()
    {
        if (waiting)
        {
            playerShreded = true;
            playerPlayedJack = true;
        }
    }

    public void useKing()
    {
        if (waiting)
        {
            drawPowerup();
            drawPowerup();
        }
    }

    public void useAce()
    {
        if (waiting)
        {
            playerActiveCard = -3;
            waiting = false;
        }
    }

    public void chooseBot1()
    {
        bot1PlayedJack = true;
        waiting = false;
    }
    public void chooseBot2()
    {
        bot2PlayedJack = true;
        waiting = false;
    }
    public void chooseBot3()
    {
        bot3PlayedJack = true;
        waiting = false;
    }
    public void chooseBot4()
    {
        bot1PlayedJack = true;
        waiting = false;
    }

    public void useQueen()
    {
        if (waiting)
        {
            print(bot1ActiveCard + " " + bot2ActiveCard + " " + bot3ActiveCard + " " + bot4ActiveCard);
        }
    }

    #endregion

    public void gameOver()
    {

    }
}
