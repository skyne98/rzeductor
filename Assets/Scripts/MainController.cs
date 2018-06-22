using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private Character _currentCharacter;
    private GameObject _characterGameObject;
    private CharactersService _charactersService;
    
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private List<CharacterVisualPreset> _visualPresets;
    
    private void Start()
    {
        _characterGameObject = Instantiate(_characterPrefab);
        _charactersService = new CharactersService(_visualPresets);
        
        RandomCharacter();
    }

    private void Update()
    {
        
    }

    private void RandomCharacter()
    {
        var newCharacter = _charactersService.GenerateCharacter();
        ChangeCharacter(newCharacter);
    }

    private void ChangeCharacter(Character newCharacter)
    {
        // Play removal animation
        
        // Change the sprite
        var spriteRenderer = _characterGameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = newCharacter.Preset.Sprite;

        // Play enter animation
        
    }
}