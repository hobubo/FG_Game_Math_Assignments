using NUnit.Framework;
using FGMath;
using UnityEditor;
using System.Linq;
using System;

namespace FGMathTests
{

public class CardSetTests
{
    CardSetSO cardSet;

    [SetUp]
    public void Setup()
    {
        var guid = AssetDatabase.FindAssets("t:CardSetSO").First();
        var assetPath = AssetDatabase.GUIDToAssetPath(guid);
        cardSet = AssetDatabase.LoadAssetAtPath<CardSetSO>(assetPath);
        cardSet.RecalculateShopWeights();
    }

    [Test]
    public void PickCard()
    {
        var card = cardSet.PickCard();
        Assert.IsNotNull(card);
        Assert.IsTrue(cardSet.Cards.Contains(card));
    }

        [Test]
    public void PickCardUnweighted()
    {
        var card = cardSet.PickCardUnweighted();
        Assert.IsNotNull(card);
        Assert.IsTrue(cardSet.Cards.Contains(card));
    }

    [Test]
    public void PickCardsWithoutDuplicates()
    {
        int max = cardSet.Cards.Count;

        var smallTest = cardSet.PickCardsWithoutDuplicates(max/2);
        Assert.AreEqual(smallTest.Count, smallTest.Distinct().Count());
        Assert.AreEqual(max/2, smallTest.Count);

        var bigTest = cardSet.PickCardsWithoutDuplicates(max);
        Assert.AreEqual(bigTest.Count, bigTest.Distinct().Count());
        Assert.AreEqual(max, bigTest.Count);

        Assert.Throws<ArgumentOutOfRangeException>(() => cardSet.PickCardsWithoutDuplicates(max+1));
    }

        [Test]
    public void PickCardsWithoutDuplicatesUnweighted()
    {
        int max = cardSet.Cards.Count;

        var smallTest = cardSet.PickCardsWithoutDuplicatesUnweighted(max/2);
        Assert.AreEqual(smallTest.Count, smallTest.Distinct().Count());
        Assert.AreEqual(max/2, smallTest.Count);

        var bigTest = cardSet.PickCardsWithoutDuplicatesUnweighted(max);
        Assert.AreEqual(bigTest.Count, bigTest.Distinct().Count());
        Assert.AreEqual(max, bigTest.Count);

        Assert.Throws<ArgumentOutOfRangeException>(() => cardSet.PickCardsWithoutDuplicatesUnweighted(max+1));
    }

    [Test]
    public void PickCardWithQuality()
    {
        for (int i = 0; i < 10; i++)
        {
            var randomCard = cardSet.PickCard();
            var card = cardSet.PickCardWithQuality(randomCard._quality);
            Assert.AreEqual(card._quality, randomCard._quality);
        }
    }
}
}
