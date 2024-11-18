using System;
using System.Collections.Generic;
using System.Linq;
using FGMath;
using NUnit.Framework;
using UnityEngine;


namespace FGMathTests
{

public class GameManagerTests
{

    [SetUp]
    public void Setup()
    {
        GameManager.RefreshShop();
        GameManager.ResetDeck();
    }

    [Test]
    public void RefreshShopHasNoDuplicates()
    {
        GameManager.RefreshShop();
        Assert.AreEqual(Global.Data.ShopCardCount, GameManager.ShopContents.Count);
        Assert.AreEqual(Global.Data.ShopCardCount, GameManager.ShopContents.Distinct().Count());
    }

    [Test]
    public void BuyFromShopThrowsOnInvalidPurchase()
    {
        
        CardDataSO dummy = ScriptableObject.CreateInstance<CardDataSO>();
        dummy.name = "Dummy Card";
        Assert.Throws<KeyNotFoundException>(() => GameManager.BuyFromShop(dummy));
    }

    [Test]
    public void BuyFromShopPutsCardInDeck()
    {
        Global.Data.NewCardsAddToDeck = true;

        int oldDeckSize = GameManager.Deck.Count;
        int oldDiscardSize = GameManager.Discard.Count;
        var buyTarget = GameManager.ShopContents[1];
        GameManager.BuyFromShop(buyTarget);
        Assert.AreEqual(oldDeckSize+1, GameManager.Deck.Count);
        Assert.AreEqual(oldDiscardSize, GameManager.Discard.Count);
        Assert.IsNull(GameManager.ShopContents[1]);
        Assert.IsNotNull(GameManager.Deck.Find((x) => x == buyTarget));
    }

        [Test]
    public void BuyFromShopPutsCardInDiscard()
    {
        Global.Data.NewCardsAddToDiscard = true;

        int oldDeckSize = GameManager.Deck.Count;
        int oldDiscardSize = GameManager.Discard.Count;
        var buyTarget = GameManager.ShopContents[1];
        GameManager.BuyFromShop(buyTarget);
        Assert.AreEqual(oldDeckSize, GameManager.Deck.Count);
        Assert.AreEqual(oldDiscardSize+1, GameManager.Discard.Count);
        Assert.IsNull(GameManager.ShopContents[1]);
        Assert.IsNotNull(GameManager.Discard.Find((x) => x == buyTarget));
    }

    [Test]
    public void DrawNewHandGetsCorrectNumberOfCards()
    {
        Global.Data.HandCardCount = 3;

        GameManager.DrawNewHand();
        Assert.AreEqual(Global.Data.HandCardCount, GameManager.Hand.Count);
    }

    [Test]
    public void DrawNewHandPlacesOldHandIntoDiscard()
    {
        GameManager.DrawNewHand();
        int deckBefore = GameManager.Deck.Count;
        int discardBefore = GameManager.Discard.Count;
        GameManager.DrawNewHand();
        Assert.AreEqual(discardBefore + Global.Data.HandCardCount, GameManager.Discard.Count);
        Assert.AreEqual(deckBefore - Global.Data.HandCardCount, GameManager.Deck.Count);
    }

    [Test]
    public void DrawNewHandCyclesCorrectly()
    {
        int totalCards = GameManager.Deck.Count + GameManager.Discard.Count + GameManager.Hand.Count;
        for (int i = 0; i < 20; i++)
        {
            GameManager.DrawNewHand();
            Assert.AreEqual(Global.Data.HandCardCount, GameManager.Hand.Count);
            Assert.AreEqual(totalCards, GameManager.Deck.Count + GameManager.Discard.Count + GameManager.Hand.Count);
        }
    }
    [Test]
    public void DrawNewHandThrowsIfWeDrawTooMany()
    {
        int totalCards = GameManager.Deck.Count + GameManager.Discard.Count + GameManager.Hand.Count;
        Global.Data.HandCardCount = totalCards + 1;
        Assert.Throws<ArgumentOutOfRangeException>(() => GameManager.DrawNewHand());
    }
}
}
