using UnityEngine;
using System;

namespace FGMath
{

/// <summary>
/// Card qualities, their names and their visual effects on the card etc.
/// </summary>
[Serializable]
[CreateAssetMenu(menuName = "FG Card Game/Card Quality")]
public class CardQualitySO : ScriptableObject
{
    [Range(1,3)] public int _effectChoices;

    public MaterialPropertyBlock PropBlock {get; private set;}
    public Color _color;
    [Range(0,1)] public float _smoothness;
    [Range(0,1)] public float _metallic;

    public event Action<CardQualitySO> Update;


    void OnValidate()
    {
        PropBlock ??= new();
        PropBlock.Clear();
        PropBlock.SetColor("_cardQualityColor", _color );
        PropBlock.SetFloat("_cardQualitySmoothness", _smoothness);
        PropBlock.SetFloat("_cardQualityMetallic", _metallic);

        Update?.Invoke(this);
    }
}
}