
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace FGMath
{

public static class GameManager
{
    static GameManager()
    {
        CardSet = Addressables.LoadAssetsAsync<CardSetSO>("ActiveCardSet", null).WaitForCompletion()[0];
        ResetDeck();
    }
    static public DiscardUI discardUI;
    static public CardSetSO CardSet;
    static public List<CardDataSO> ShopContents {get; private set;} = new();
    static public List<CardDataSO> Deck {get; private set;} = new();
    static public List<CardDataSO> Hand {get; private set;} = new();
    static public List<CardDataSO> Discard {get; private set;}  = new();

    static public event Action OnRefreshShop;
    static public event Action OnDrawNewHand;
    static public event Action OnShuffleDiscardIntoDeck;
    static public event Action OnCardBought;


    static public event Action<float> OnEndTurn;

	public enum State
	{
		Playing, Waiting
	}
	static public State CurrentState {get; private set;} = State.Waiting;

    public static void ResetDeck()
    {
        Deck = new(CardSet.DefaultDeck);
        Discard.Clear();
    }

    public static void RefreshShop()
    {
        ShopContents = CardSet.PickCardsWithoutDuplicates(Global.Data.ShopCardCount);
        OnRefreshShop?.Invoke();
    }

    public static void BuyFromShop(CardDataSO card)
    {
        var idx = ShopContents.IndexOf(card);
        if(idx < 0)
        {
            throw new KeyNotFoundException("GameManager::BuyFromShop(CardDataSO): Tried to buy a card that isn't in the shop..");
        }
        BuyFromShop(idx);
    }

    public static void BuyFromShop(int idx)
    {
        AddCard(ShopContents[idx]);
        ShopContents[idx] = null;
        OnCardBought?.Invoke();
    }

    public static void AddCard(CardDataSO newCard)
    {
        if(Global.Data.NewCardsAddToDiscard)
        {
            AddCardToDiscard(newCard);
        }
        else
        {
            ADdCardToDeck(newCard);
        }
    }

    private static void AddCardToDiscard(CardDataSO newCard)
    {
        Discard.Add(newCard);
    }

    private static void ADdCardToDeck(CardDataSO newCard)
    {
        int idx = Random.Range(0, Deck.Count);
        Deck.Add(newCard);
        (Deck[idx], Deck[Deck.Count-1]) = (Deck[Deck.Count-1], Deck[idx]);
        
    }

    public static void DrawNewHand()
    {
		if(CurrentState == State.Playing) return;
        if(Global.Data.HandCardCount > Discard.Count + Hand.Count + Deck.Count)
        {
            // @TODO: @BUG: Currently just throwing when this happens, but might be more
            //  legitimate to return a smaller hand than was requested..?
            throw new ArgumentOutOfRangeException(
                "GameManager::DrawNewHand: Asked for more cards than we currently have!");
        }

        Discard.AddRange(Hand);
        Hand.Clear();

        if(Global.Data.HandCardCount > Deck.Count)
        {
            Hand.AddRange(Deck);
            Deck.Clear();
            ShuffleDiscardIntoDeck();
            int remainingDraw = Global.Data.HandCardCount - Hand.Count;
            Hand.AddRange(Deck.TakeLast(remainingDraw));
            Deck.RemoveRange(Deck.Count - remainingDraw, remainingDraw);
        }
        else
        {
            Hand = Deck.TakeLast(Global.Data.HandCardCount).ToList();
            Deck.RemoveRange(Deck.Count - Global.Data.HandCardCount, Global.Data.HandCardCount);
        }

        OnDrawNewHand?.Invoke();
		CurrentState = State.Playing;
    }

    public static void ShuffleDiscardIntoDeck()
    {
        Deck.AddRange(Discard);
        Discard.Clear();

        for (int i = 0; i < Deck.Count-1; ++i)
        {
            int shuffle = Random.Range(i, Deck.Count);
            (Deck[shuffle], Deck[i]) = (Deck[i], Deck[shuffle]);
        }

        OnShuffleDiscardIntoDeck?.Invoke();
    }

    public static void EndTurn()
    {
        OnEndTurn.Invoke(Global.Data.EndTurnTime);
		CurrentState = State.Waiting;
    }

    public static Vector3 GetDiscardPosition()
    {
        return discardUI.transform.position;
    }
    }
}