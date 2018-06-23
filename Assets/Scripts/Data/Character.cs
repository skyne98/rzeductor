using System;
using Data;

[Serializable]
public class Character {
    public Character(CharacterVisualPreset preset, Person person)
    {
        Preset = preset;
        Person = person;
    }

    public CharacterVisualPreset Preset { get; private set; }
    public Person Person { get; private set; }
}