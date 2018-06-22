using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Data;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // Character
    private Character _currentCharacter;
    private GameObject _characterGameObject;
    private Animation _characterAnimation;
    private CharactersService _charactersService;
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
    
    private void Start()
    {
        _characterGameObject = Instantiate(_characterPrefab);
        _characterAnimation = _characterGameObject.GetComponent<Animation>();
        _charactersService = new CharactersService(_visualPresets);
        _characterState = CharacterState.Idle;

        _backgroundAnimation = _backgroundGameObject.GetComponent<Animation>();
        
        RandomCharacter();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            RandomCharacter();
        }
    }

    private void RandomCharacter()
    {
        var newCharacter = _charactersService.GenerateCharacter();
        StartCoroutine(ChangeCharacter(newCharacter));
    }

    private IEnumerator ChangeCharacter(Character newCharacter)
    {
        if (_characterState == CharacterState.Idle)
        {
            // Play removal animation
            if (_currentCharacter != null)
            {
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