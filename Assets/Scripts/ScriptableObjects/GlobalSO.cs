using UnityEditor;
using UnityEngine;

namespace FGMath
{

[System.Serializable]
[CreateAssetMenu(menuName="FG Card Game/Globals")]
public class GlobalSO : ScriptableObject
{
    [Range(1.0f, 30.0f)] public float LerpSmoothDecay = 16;

    public float EndTurnTime = 1.0f;
    public float EndTurnLagFactor = 0.2f;
    public float EndTurnPause = 1.0f;

    [SerializeField] public int HandCardCount = 4;
    [SerializeField] public int ShopCardCount = 4;

    [SerializeField] private bool _BuyToDiscard = true;
    public bool NewCardsAddToDiscard {get {return _BuyToDiscard;} set {_BuyToDiscard = value;}}
    public bool NewCardsAddToDeck {get {return !_BuyToDiscard;} set {_BuyToDiscard = !value;}}

#if UNITY_EDITOR
    void OnValidate()
    {
        EditorApplication.delayCall += () => Global.Set(this);
    }
#endif
}
}