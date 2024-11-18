using System;
using UnityEngine;

namespace FGMath
{

[Serializable]
[CreateAssetMenu(menuName = "FG Card Game/Resource Type")]
public class ResourceSO : ScriptableObject
{
    [SerializeField] public Sprite _icon;

}
}