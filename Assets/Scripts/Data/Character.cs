using System;

[Serializable]
public class Character {
    public Character(CharacterVisualPreset preset)
    {
        Preset = preset;
    }

    public CharacterVisualPreset Preset { get; private set; }
}