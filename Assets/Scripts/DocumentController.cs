﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableController))]
public class DocumentController : MonoBehaviour {
    private GameObject _characterGameObject;
    private BoxCollider2D _characterCollider2D;
    private Person _person;
    private DraggableController _draggableController;
    private SpriteRenderer _renderer;
    private MainController _mainController;

    [SerializeField] private Text _firstNameText;
    [SerializeField] private Text _lastNameText;
    [SerializeField] private Text _countryText;
    [SerializeField] private Text _dateOfBirthText;
    [SerializeField] private Text _expiryText;
    
    private void Start()
    {
        _characterGameObject = GameObject.FindGameObjectWithTag("Character");
        _characterCollider2D = _characterGameObject.GetComponent<BoxCollider2D>();
        _draggableController = GetComponent<DraggableController>();
        _mainController = GameObject.FindWithTag("MainCamera").GetComponent<MainController>();
        _renderer = GetComponent<SpriteRenderer>();
        
        _draggableController.Dropped += DraggableControllerOnDropped;
    }

    private void Update()
    {
        transform.Find("Canvas").gameObject.active = !_draggableController.OnTray;
    }

    private void DraggableControllerOnDropped(Vector3 mouseWorldPosition)
    {
        if (_characterCollider2D.bounds.Contains(mouseWorldPosition) && _draggableController.OnTray == false)
        {
            _mainController.ReturnDocument(gameObject);
        }
    }

    public void SetPerson(Person person)
    {
        _person = person;
        _firstNameText.text = _person.FirstName;
        _lastNameText.text = _person.Lastname;
        _countryText.text = GetCountryText(_person.Country);
        _dateOfBirthText.text = _person.DateOfBirth;
        _expiryText.text = _person.ExpiryDate;
    }

    private string GetCountryText(Country personCountry)
    {
        switch (personCountry)
        {
            case Country.UnitedVoivodeshipsOfPoland:
                return "United Voivodeships of Poland";
            case Country.UnitedKingdomsOfCanada:
                return "United Kingdoms of Canada";
            case Country.TheGreatEasternComradeship:
                return "The Great Eastern Comradeship";
            case Country.NorthernCommonwealth:
                return "Northern Commonwealth";
            case Country.UnitedCitiesOfAthens:
                return "United Cities of Athens";
        }

        return "Wasteland";
    }
}
