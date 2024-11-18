using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEditor;
using System;


namespace FGMath
{

/// <summary>
/// This lil' guy sets doohickies in the card UI like sprites, text and so on.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[SelectionBase]
public class CardUI : MonoBehaviour
        , IPointerEnterHandler
        , IPointerExitHandler
        , ISelectHandler
        , IDeselectHandler
        , IPointerClickHandler
{

    [SerializeField] private CardDataSO _cardData;
    public CardDataSO CardData {
        set
        {
            _cardData = value;
            //@NOTE (Euan): Unity if goofy. You can be destroyed before your OnDisable is called..
            if(this == null) return;
            if(_cardData != null)
            {
                gameObject.SetActive(true);
                UpdateCardImages();
                _glowQuad.gameObject.SetActive(false);

                _nameText.text = _cardData.name;

                if(_cardData._quality != null)
                {
                    UpdateCardTexts();
                    SetQualityEffect();
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
            }
        get
        {
            return _cardData;
        }
    }
    [SerializeField] private SpriteRenderer _resourceCostIcon;
    [SerializeField] private TextMeshPro _resourceCostAmount;
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private GameObject _commonTexts;
    [SerializeField] private GameObject _rareTexts;
    [SerializeField] private GameObject _epicTexts;
    [SerializeField] private SpriteRenderer _cardImage;
    [SerializeField] private Renderer _glowQuad;
    [SerializeField] private Vector2 _cardImageDim;

    private Renderer _renderer;
    private CardGroup _parentGroup;
    private Coroutine _currentAnimation;
    private PseudoTransform _target;
	private static MaterialPropertyBlock _glowHoverPropBlock;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _parentGroup = transform.parent?.GetComponent<CardGroup>();

        if(_glowHoverPropBlock == null)
        {
            _glowHoverPropBlock = new();
            _glowHoverPropBlock.SetColor("_FGColor", new Color(0.4669811f,0.7052721f,1,1));
            _glowHoverPropBlock.SetColor("_BGColor", new Color(0.2971698f,0.5975954f,1,1));
            _glowHoverPropBlock.SetFloat("_Range", 0.39f);
        }
    }

    private void SetCardData(CardDataSO newCardData) => CardData = newCardData;

    internal void MoveToNewGroup(CardGroup newGroup)
    {
        _parentGroup._cards.Remove(this);
        _parentGroup = newGroup;
        newGroup._cards.Add(this);
    }

    private void OnEnable()
    {
        if(_cardData == null) return;

        _cardData.OnCardDataChanged += SetCardData;
        CardData = _cardData;
    }
    private void OnDisable()
    {
        CardDataSO.UnregisterFromAllUpdates(SetCardData);
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        EditorApplication.delayCall += () => {
            if(this == null) return; // Welcome to Unity, fellas..
            Awake();
            CardDataSO.UnregisterFromAllUpdates(SetCardData);
            if(_cardData == null) return;
            _cardData.OnCardDataChanged += SetCardData;
            SetCardData(_cardData);
        };
    }
#endif

    private void UpdateCardTexts()
    {
        _resourceCostAmount.text = _cardData._costAmount.ToString();
        
        //@NOTE (Euan):  My appologies for this spaghetti mess..
        if(_cardData == null) return;

        if(_cardData._quality != null)
        {
            List<TextMeshPro> TMPComponents = new();
            switch (_cardData._quality._effectChoices)
            {
                case 1:
                    _commonTexts.SetActive(true);
                    TMPComponents.AddRange(_commonTexts.GetComponentsInChildren<TextMeshPro>());

                    _rareTexts.SetActive(false);
                    _epicTexts.SetActive(false);
                break;
                case 2:
                    _rareTexts.SetActive(true);
                    TMPComponents.AddRange(_rareTexts.GetComponentsInChildren<TextMeshPro>());
                    
                    _commonTexts.SetActive(false);
                    _epicTexts.SetActive(false);
                break;
                case 3:
                    _epicTexts.SetActive(true);
                    TMPComponents.AddRange(_epicTexts.GetComponentsInChildren<TextMeshPro>());

                    _commonTexts.SetActive(false);
                    _rareTexts.SetActive(false);
                break;
                default:
                    Debug.Log("I did not set up the prefab for cards with more than 3 effects to choose from. Feel free to add your own implementation here!");
                break;
            }

            for (int i = 0; i < TMPComponents.Count; i++)
            {
                TMPComponents[i].text = _cardData._descriptions[i];
            }
        }
    }

    void UpdateCardImages()
    {
        var cd = _cardData;
        if(cd == null) return;

        if(cd._image == null) return;
        {
            if (cd._image.rect.width != 0 &&
               cd._image.rect.height != 0)
            {
                var x = _cardImageDim.x / cd._image.rect.width;
                var y = _cardImageDim.y / cd._image.rect.height;
                var scale = Mathf.Min(x,y);

                _cardImage.size = cd._image.rect.size*scale;
            }
            else
            {
                _cardImage.size = _cardImageDim;
            }

            _cardImage.sprite = cd._image;
        }

        if(cd._costType == null) return;
        _resourceCostIcon.sprite = cd._costType._icon;
    }
    private void SetQualityEffect()
    {
        _renderer.SetPropertyBlock(_cardData._quality.PropBlock);
    }

    private IEnumerator AnimateCard()
    {
        // @NOTE: Unity overloads == and != to check sqMagnitude < epsilon. This is safe.
        while(transform.position != _target.pos ||
              transform.rotation != _target.rot)
        {
            var move = Ease.Approach(transform, _target, Global.Data.LerpSmoothDecay, Time.deltaTime);
            move.ApplyTo(transform);
            yield return null;
        }
        
        // Clean up.
        transform.position = _target.pos;
        transform.rotation = _target.rot;
        _currentAnimation = null;
    }

    public void GoToDiscard(float duration, float delay)
    {
        if(_currentAnimation != null) StopCoroutine(_currentAnimation);
        _currentAnimation = StartCoroutine(LerpTowardsDiscard(duration, delay));
    }

    public void SetTargetPseudoTransform(PseudoTransform target)
    {
        _target = target;
        if(_currentAnimation == null) StartCoroutine(AnimateCard());
    }


    private IEnumerator LerpTowardsDiscard(float duration, float delay)
    {
        float elapsed = 0.0f;
        while(elapsed < delay)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        duration -= elapsed;
        elapsed = 0.0f;

        Assignment2.Input input;
        
        input.discardPosition.pos = GameManager.GetDiscardPosition();
        input.discardPosition.rot = Quaternion.identity;
        input.discardPosition.scale = Vector3.one*0.1f;

        input.startingPosition.pos = transform.position;
        input.startingPosition.rot = transform.rotation;
        input.startingPosition.scale = transform.localScale;

        while(elapsed < duration)
        {
            input.t = elapsed/duration;
            PseudoTransform trans = Assignment2.MoveToDiscard(input);
            trans.ApplyTo(transform);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _currentAnimation = null;
    }

    //
    // Unity Pointer Events
    //

    public void OnPointerEnter(PointerEventData eventData)
    {
		if(GameManager.CurrentState == GameManager.State.Waiting) return;
        eventData.selectedObject = gameObject;
        _glowQuad.SetPropertyBlock(_glowHoverPropBlock);
        _glowQuad.gameObject.SetActive(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if(GameManager.CurrentState == GameManager.State.Waiting) return;

        if(eventData.selectedObject == gameObject)
        {
            eventData.selectedObject = null;
        }
        _glowQuad.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
		if(GameManager.CurrentState == GameManager.State.Waiting) return;

        _target = _parentGroup.GetPseudoTransform(this, true);
        _currentAnimation ??= StartCoroutine(AnimateCard());
    }

    public void OnDeselect(BaseEventData eventData)
    {
		if(GameManager.CurrentState == GameManager.State.Waiting) return;

        _target = _parentGroup.GetPseudoTransform(this, false);
        _currentAnimation ??= StartCoroutine(AnimateCard());
    }
     public void OnPointerClick(PointerEventData eventData)
    {
		if(GameManager.CurrentState == GameManager.State.Waiting) return;
 
        _parentGroup.OnPointerClick(eventData, this);
    }
}
}