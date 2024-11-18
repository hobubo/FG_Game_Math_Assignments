using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
namespace FGMath
{

    /// <summary>
    /// Represents an individual card.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "FG Card Game/Card Data")]
    public class CardDataSO : ScriptableObject
    {
        static public List<CardDataSO> AllCardData = new();

        [SerializeField] public ResourceSO _costType;
        [SerializeField] public int _costAmount;
        [SerializeField] public Sprite _image;
        
        [SerializeField, Range(0.1f, 10.0f)] public float _shopWeight;
        [SerializeField] public CardQualitySO _quality;
        [SerializeField] public string[] _descriptions = new string[3];

        public event Action<CardDataSO> OnCardDataChanged;

        void OnEnable()
        {
            if (!AllCardData.Contains(this))
            {
                AllCardData.Add(this);
            }
            if (_quality != null)
            {
                _quality.Update += OnQualityUpdated;
            }
#if UNITY_EDITOR
            else
            {
                EditorApplication.delayCall += OnEnable;
            }
#endif
        }

#if UNITY_EDITOR
        
        public void OnValidate()
        {
            EditorApplication.delayCall += OnValidate_;
        }
        public void OnValidate_()
        {
            foreach (var qual in GameManager.CardSet.Qualities)
            {
                qual.Update -= OnQualityUpdated;
            }
            if (_quality != null) _quality.Update += OnQualityUpdated;
            OnCardDataChanged?.Invoke(this);
        }
#endif

        void OnDisable()
        {
            if (AllCardData.Contains(this))
            {
                AllCardData.Remove(this);
            }
            if (_quality != null)
            {
                _quality.Update -= OnQualityUpdated;
            }
        }

        void OnQualityUpdated(CardQualitySO updatedQuality)
        {
            OnCardDataChanged?.Invoke(this);
        }

        public static void UnregisterFromAllUpdates(Action<CardDataSO> obj)
        {
            foreach (var cd in AllCardData)
            {
                cd.OnCardDataChanged -= obj;
            }
        }
    }
}