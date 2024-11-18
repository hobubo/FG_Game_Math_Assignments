using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Assertions;
namespace FGMath
{

[CreateAssetMenu(menuName = "FG Card Game/Card Set")]
[Serializable]
public class CardSetSO : ScriptableObject
{
    [field: SerializeField] public List<CardDataSO> Cards { get; private set;} = new();
    [field: SerializeField] public List<CardDataSO> DefaultDeck {get; private set;} = new();

    public List<CardQualitySO> Qualities {get; set;} = new();
    public List<ResourceSO> Resources {get; set;} = new();
    
    public float _shopWeightTotal = 0;

#if UNITY_EDITOR
    void OnValidate()
    {
        EditorApplication.delayCall += OnValidate_;
    }
    void OnValidate_()
    {
        var thisPath = Path.GetDirectoryName(AssetDatabase.GetAssetOrScenePath(this));
        var guids = AssetDatabase.FindAssets("t:CardDataSO", new[]{thisPath});
        Cards.Clear();

        _shopWeightTotal = 0;
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<CardDataSO>(assetPath);
            _shopWeightTotal += asset._shopWeight;
            Cards.Add(asset);
        }

        Qualities = LoadAllOfType<CardQualitySO>("t:CardQualitySO");
        Resources = LoadAllOfType<ResourceSO>("t:ResourceSO");
    }

    List<T> LoadAllOfType<T>(string typeID) where T : ScriptableObject
    {
        var thisPath = Path.GetDirectoryName(AssetDatabase.GetAssetOrScenePath(this));
        var guids = AssetDatabase.FindAssets(typeID, new[]{thisPath});
        List<T> retVal = new();

        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            retVal.Add(asset);
        }

        return retVal;
    }
#endif

    public void RecalculateShopWeights()
    {
        _shopWeightTotal = 0;
        foreach (var card in Cards) _shopWeightTotal += card._shopWeight;
    }

    public List<CardDataSO> PickCardsWithoutDuplicates(int count)
    {
        if(Cards.Count < count)
        {
            throw new ArgumentOutOfRangeException("CardDataSO::PickCardsWithoutDuplicates: Asked for " + 
            "more cards than exist in the cardSet.");
        }

        var runningTotal = _shopWeightTotal;

        List<CardDataSO> retVal = new();
        List<CardDataSO> cardsCopy = new(Cards); // @PERF(Euan): Big piggy copy that I'm not happy about.

        for(int i = 0; i < count; ++i)
        {
            var rand = Random.Range(0.0f, runningTotal);
            for(int j = 0; j < cardsCopy.Count; ++j)
            {
                rand -= cardsCopy[j]._shopWeight;
                if (rand <= 0.0f )
                {
                    runningTotal -= cardsCopy[j]._shopWeight;
                    retVal.Add(cardsCopy[j]);
                    cardsCopy.RemoveAt(j);
                    break;
                } 
            }
        }

        return retVal;
    }


    public List<CardDataSO> PickCardsWithoutDuplicatesUnweighted(int count)
    {
        if(Cards.Count < count)
        {
            throw new ArgumentOutOfRangeException("CardDataSO::PickCardsWithoutDuplicatesUnweighted: Asked for " + 
            "more cards than exist in the cardSet.");
        }

        List<CardDataSO> retVal = new();
        for(int i = 0; i < Cards.Count; ++i)
        {
            int cardsNeeded = count - retVal.Count;
            int cardsLeft = Cards.Count - i;
            if(Random.Range(0.0f, 1.0f) <= cardsNeeded / cardsLeft)
            {

                retVal.Add(Cards[i]);
                if (retVal.Count == count) return retVal;
            }

        }

        //@TIDY: Do we have access to some kind of UneeachableCodeException in Unity C#?
        Assert.IsTrue(false); // Unreachable code.
        return retVal;
    }

    public CardDataSO PickCard()
    {
        var rand = Random.Range(0.0f, _shopWeightTotal);

        for(int i = 0; i < Cards.Count; ++i)
        {
            rand -= Cards[i]._shopWeight;
            if (rand <= 0.0f ) return Cards[i];
        }

        Debug.LogException(new InvalidDataException(
        "CardDataSO::PickRandomCard: Failed to pick a card. _shopWeightTotal is set incorrectly "+
        "or Cards.Count is 0?"));
        return Cards[0];
    }

    public CardDataSO PickCardUnweighted() => Cards[Random.Range(0, Cards.Count)];


    public CardDataSO PickCardWithQuality(CardQualitySO quality)
    {

        CardDataSO current = null;
        int found = 0;

        for (int i = 0; i < Cards.Count; i++)
        {
            if(Cards[i]._quality == quality)
            {
                found++;
                float rand = Random.Range(0.0f, found);
                if(rand < 1.0f)
                {
                    current = Cards[i];
                }
            }
        }

        if(current == null)
        {
            Debug.LogException(new InvalidDataException(
                "CardDataSO::GetRandomOfQuality: Could not find any "+
                "cards with the specified quality in CardSet."));
        }
        
        return current;
    }

}
}