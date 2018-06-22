using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CharactersService
{
    private Random _random;
    private List<CharacterVisualPreset> _presets;
    
    public CharactersService(List<CharacterVisualPreset> presets)
    {
        _presets = presets;
        _random = new Random();
    }

    public Character GenerateCharacter()
    {
        var presetIndex = _random.Next(0, _presets.Count);
        var preset = _presets[presetIndex];
        
        return new Character(preset);
    }
}