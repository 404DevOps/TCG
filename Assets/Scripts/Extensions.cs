using Mirror;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class Extensions
{
    /// <summary>
    /// Deals first Card from List and Removes it.
    /// </summary>
    /// <param name="deck">List of Cards</param>
    /// <returns>Dealt Card or Null if Deck is Empty.</returns>
    [Server]
    public static string DealCardFromDeck(this SyncList<string> deck)
    {
        if (deck.Count > 0)
        {
            string card = deck.First();
            deck.RemoveAt(0);
            return card;
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// Shuffles the List of Cards to be in Random Order.
    /// </summary>
    /// <param name="deck">List of Cards</param>
    /// <returns>The Shuffled List of Cards.</returns>
    [Server]
    public static SyncList<string> ShuffleDeck(this SyncList<string> deck)
    {
        if (!deck.Any())
        {
            Debug.Log("Can't shuffle an empty Deck.");
            return deck;
        }
            

        var listShuffled = new List<string>();
        var iterations = deck.Count;
        for (int i = 0; i < iterations; i++)
        {
            var randomIndex = Random.Range(0, deck.Count);

            //pick random card and remove it from list
            var pickedCard = deck[randomIndex];
            deck.RemoveAt(randomIndex);

            //add random card to new list
            listShuffled.Add(pickedCard);
        }
        deck.RemoveAll(m => m != null);
        deck.AddRange(listShuffled);

        return deck;

    }

    [Server]
    public static SyncList<string> ShufflePileIntoDeck(this SyncList<string> deck, SyncList<string> pile)
    {
        //cards = discardPile.cards;
        deck.AddRange(pile);
        pile.Clear();

        deck.ShuffleDeck();

        if (deck.Any())
        {
            Debug.Log("Resfhuffled");
            return deck;
        }
        
        GameManager.Instance.ShowMessage("No Cards to reshuffle into Deck.", Color.red);
        return null;
    }
}

