using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Data;
using Services;
using UnityEngine;
using Random = System.Random;

public class MainController : MonoBehaviour
{
    // Character
    private Character _currentCharacter;
    private bool _currentWithTicket;
    private List<GameObject> _currentDocuments;
    private GameObject _characterGameObject;
    private Animation _characterAnimation;
    private CharactersService _charactersService;
    private DocumentsService _documentsService;
    private Random _random;
    
    // Background
    private Animation _backgroundAnimation;
    
    // Scenert
    private Animation _sceneryAnimation;
    
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private GameObject _ticketPrefab;
    [SerializeField] private GameObject _backgroundGameObject;
    [SerializeField] private GameObject _sceneryGameObject;
    [SerializeField] private RulebookController _rulebookController;
    [SerializeField] private AnimationClip _entryAnimationClip;
    [SerializeField] private AnimationClip _leaveAnimationClip;
    [SerializeField] private AnimationClip _doorOpenAnimationClip;
    [SerializeField] private AnimationClip _doorCloseAnimationClip;
    [SerializeField] private List<CharacterVisualPreset> _visualPresets;
    [SerializeField] private List<UniquePerson> _uniquePeople;
    [SerializeField] private List<NamePreset> _firstNames;
    [SerializeField] private List<string> _lastNames;
    [SerializeField] private List<DocumentPreset> _documentPresets;
    
    // States
    public CharacterState CharacterState { get; set; }
    public MouseState MouseState { get; set; }
    
    // Rules
    public Country _privilegedCountry;
    public DateTime _minimumDateOfBirth;
    
    private void Start()
    {
        _currentDocuments = new List<GameObject>();
        _characterGameObject = Instantiate(_characterPrefab);
        _characterAnimation = _characterGameObject.GetComponent<Animation>();
        _charactersService = new CharactersService(_visualPresets, _uniquePeople, _firstNames, _lastNames);
        _documentsService = new DocumentsService(_documentPresets);
        _random = new Random();
        CharacterState = CharacterState.Idle;
        MouseState = MouseState.Idle;
        
        _backgroundAnimation = _backgroundGameObject.GetComponent<Animation>();

        _sceneryAnimation = _sceneryGameObject.GetComponent<Animation>();
        
        // Setup the rules
        var values = Enum.GetValues(typeof(Country));
        var country = (Country)values.GetValue(_random.Next(values.Length));
        _privilegedCountry = country;
        _minimumDateOfBirth = new DateTime(1993, 1, 1);
        
        // Write to the book
        _rulebookController.SetRules(_privilegedCountry, _minimumDateOfBirth);
        
        RandomCharacter();
    }

    public void RandomCharacter()
    {
        var newCharacter = _charactersService.GenerateCharacter();
        StartCoroutine(ChangeCharacter(newCharacter));
    }

    public void FineCharacter()
    {
        if (RequiresTicket() && _currentWithTicket)
            Debug.Log("You fined a wrong bro!");
        if (RequiresTicket() == false)
            Debug.Log("You fined a wrong bro!");
        
        RandomCharacter();
    }

    public void ReturnDocument(GameObject document)
    {
        _currentDocuments.Remove(document);
        Destroy(document);

        if (_currentDocuments.All(d => d.GetComponent<FineController>() != null))
        {
            LetCharacter();
        }
    }

    public void AddDocument(GameObject document)
    {
        _currentDocuments.Add(document);
    }

    private void LetCharacter()
    {
        if (RequiresTicket() && !_currentWithTicket)
            Debug.Log("You had to fine him!");
        
        RandomCharacter();
    }

    private bool RequiresTicket()
    {
        if (DateTime.Parse(_currentCharacter.Person.DateOfBirth) < _minimumDateOfBirth)
            return true;
        if (_currentCharacter.Person.Country != _privilegedCountry)
            return true;

        return false;
    }

    private IEnumerator ChangeCharacter(Character newCharacter)
    {
        if (CharacterState == CharacterState.Idle)
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
                CharacterState = CharacterState.Animation;
            }
        
            // Wait for the character to leave
            yield return WaitForAnimation(_characterAnimation);
            CharacterState = CharacterState.Idle;
            
            // Set the new character
            _currentCharacter = newCharacter;
        
            // Change the sprite
            var spriteRenderer = _characterGameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = newCharacter.Preset.Sprite;
        
            // Make new character enter
            _sceneryAnimation.Play("SceneryStop");
            yield return WaitForAnimation(_sceneryAnimation);
            _backgroundAnimation.Play("DoorsOpen");
            yield return WaitForAnimation(_backgroundAnimation);
            _characterAnimation.Play("CharacterEntry");
            CharacterState = CharacterState.Animation;
            yield return WaitForAnimation(_characterAnimation);
            _backgroundAnimation.Play("DoorsClose");
            _sceneryAnimation.Play("SceneryStart");
            yield return WaitForAnimation(_sceneryAnimation);
            CharacterState = CharacterState.Idle;
            
            // Create new documents
            var documentPrefab = _documentsService.GenerateDocument();
            var document = Instantiate(documentPrefab);
            var documentComponent = document.GetComponent<DocumentController>();
            documentComponent.SetPerson(_currentCharacter.Person);
            _currentDocuments.Add(document);
            
            // Create a ticket
            var ticketIndex = _random.Next(0, 10);
            if (ticketIndex <= 7)
            {
                var ticket = Instantiate(_ticketPrefab);
                var ticketComponent = ticket.GetComponent<TicketController>();
                _currentDocuments.Add(ticket);
                _currentWithTicket = true;
            }
            else
            {
                _currentWithTicket = false;
            }
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