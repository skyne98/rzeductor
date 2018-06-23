using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Data;
using UnityEngine;
using Random = System.Random;

public class CharactersService
{
    private Random _random;
    private List<CharacterVisualPreset> _presets;
    private List<UniquePerson> _personPresets;
    private List<NamePreset> _firstNamePresets;
    private List<string> _lastNamePresets;
    
    public CharactersService(List<CharacterVisualPreset> presets, List<UniquePerson> personPresets, List<NamePreset> firstNamePresets, List<string> lastNamePresets)
    {
        _presets = presets;
        _personPresets = personPresets;
        _firstNamePresets = firstNamePresets;
        _lastNamePresets = lastNamePresets;
        _random = new Random();
    }

    public Character GenerateCharacter()
    {
        var unique = _random.Next(0, 10);

        if (unique == 0)
        {
            // Choose a unique person
            var uniqueIndex = _random.Next(0, _personPresets.Count);
            var uniquePerson = _personPresets.ElementAt(uniqueIndex);
            
            return new Character(uniquePerson.VisualPreset, uniquePerson.Person);
        }
        else
        {
            // Choose the preset
            var presetIndex = _random.Next(0, _presets.Count);
            var preset = _presets[presetIndex];
        
            // Create a person
            var genderNames = _firstNamePresets.Where(n => n.Female == preset.Female);
            var nameIndex = _random.Next(0, genderNames.Count());
            var firstName = genderNames.ElementAt(nameIndex).Value;

            var lastNameIndex = _random.Next(0, _lastNamePresets.Count);
            var lastName = _lastNamePresets.ElementAt(lastNameIndex);
        
            var values = Enum.GetValues(typeof(Country));
            var country = (Country)values.GetValue(_random.Next(values.Length));

            var dateOfBirthStart = new DateTime(1950, 1, 1);
            var dateOfBirthEnd = new DateTime(2006, 1, 1);
            int range = (dateOfBirthEnd - dateOfBirthStart).Days;           
            var dateOfBirth = dateOfBirthStart.AddDays(_random.Next(range));
        
            var dateOfExpiryStart = new DateTime(2015, 1, 1);
            var dateOfExpiryEnd = new DateTime(2030, 1, 1);
            int expiryRange = (dateOfExpiryEnd - dateOfExpiryStart).Days;           
            var expiryDate = dateOfExpiryStart.AddDays(_random.Next(expiryRange));
        
            var person = new Person()
            {
                FirstName = firstName,
                Lastname = lastName,
                DateOfBirth = dateOfBirth.ToShortDateString(),
                Country = country,
                ExpiryDate = expiryDate.ToShortDateString()
            };
        
            return new Character(preset, person);   
        }
    }
}