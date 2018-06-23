using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Data;
using Services;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // Character
    private Character _currentCharacter;
    private List<GameObject> _currentDocuments;
    private GameObject _characterGameObject;
    private Animation _characterAnimation;
    private CharactersService _charactersService;
    private DocumentsService _documentsService;
    private CharacterState _characterState;
    
    // Background
    private Animation _backgroundAnimation;
    
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private GameObject _backgroundGameObject;
    [SerializeField] private AnimationClip _entryAnimationClip;
    [SerializeField] private AnimationClip _leaveAnimationClip;
    [SerializeField] private AnimationClip _doorOpenAnimationClip;
    [SerializeField] private AnimationClip _doorCloseAnimationClip;
    [SerializeField] private List<CharacterVisualPreset> _visualPresets;
    [SerializeField] private List<UniquePerson> _uniquePeople;
    [SerializeField] private List<NamePreset> _firstNames;
    [SerializeField] private List<string> _lastNames;
    [SerializeField] private List<DocumentPreset> _documentPresets;
    
    private void Start()
    {
        _currentDocuments = new List<GameObject>();
        _characterGameObject = Instantiate(_characterPrefab);
        _characterAnimation = _characterGameObject.GetComponent<Animation>();
        _charactersService = new CharactersService(_visualPresets, _uniquePeople, _firstNames, _lastNames);
        _documentsService = new DocumentsService(_documentPresets);
        _characterState = CharacterState.Idle;

        _backgroundAnimation = _backgroundGameObject.GetComponent<Animation>();
        
        RandomCharacter();
    }

    public void RandomCharacter()
    {
        var newCharacter = _charactersService.GenerateCharacter();
        StartCoroutine(ChangeCharacter(newCharacter));
    }

    private IEnumerator ChangeCharacter(Character newCharacter)
    {
        if (_characterState == CharacterState.Idle)
        {
            if (_currentDocuments.Count > 0)
            {
                // Wipe the documents
                _currentDocuments.ForEach(d => Destroy(d));
                _currentDocuments.Clear();
            }
            
            if (_currentCharacter != null)
            {
                // Play removal animation
                _characterAnimation.Play("CharacterLeave");
                _characterState = CharacterState.Animation;
            }
        
            // Wait for the character to leave
            yield return WaitForAnimation(_characterAnimation);
            _characterState = CharacterState.Idle;
            
            // Set the new character
            _currentCharacter = newCharacter;
        
            // Change the sprite
            var spriteRenderer = _characterGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = newCharacter.Preset.Sprite;
        
            // Make new character enter
            _characterAnimation.Play("CharacterEntry");
            _backgroundAnimation.Play("DoorsOpen");
            _characterState = CharacterState.Animation;
            yield return WaitForAnimation(_characterAnimation);
            _backgroundAnimation.Play("DoorsClose");
            _characterState = CharacterState.Idle;
            
            // Create new documents
            var documentPrefab = _documentsService.GenerateDocument();
            var document = Instantiate(documentPrefab);
            var documentComponent = document.GetComponent<DocumentController>();
            documentComponent.SetPerson(_currentCharacter.Person);
            _currentDocuments.Add(document);
        }
    }
    
    private IEnumerator WaitForAnimation ( Animation animation )
    {
        do
        {
            yield return null;
        } while ( animation.isPlaying );
    }
}