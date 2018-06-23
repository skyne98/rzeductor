using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class DocumentController : MonoBehaviour {
    private Vector3 _offset;
    private SpriteRenderer _renderer;
    private BoxCollider2D _boxCollider2D;
    private GameObject _trayGameObject;
    private GameObject _characterGameObject;
    private BoxCollider2D _trayCollider2D;
    private BoxCollider2D _characterCollider2D;
    private Person _person;
    private Vector2 _offsetRatio;
    private bool _offsetWithSmall;
    private MainController _mainController;

    [SerializeField] private Sprite _bigSprite;
    [SerializeField] private Sprite _smallSprite;

    [SerializeField] private Text _firstNameText;
    [SerializeField] private Text _lastNameText;
    [SerializeField] private Text _countryText;
    [SerializeField] private Text _dateOfBirthText;
    [SerializeField] private Text _expieryText;
    
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _trayGameObject = GameObject.FindGameObjectWithTag("Tray");
        _trayCollider2D = _trayGameObject.GetComponent<BoxCollider2D>();
        _characterGameObject = GameObject.FindGameObjectWithTag("Character");
        _characterCollider2D = _characterGameObject.GetComponent<BoxCollider2D>();
        _offsetRatio = new Vector2(_smallSprite.bounds.size.x / _bigSprite.bounds.size.x, _smallSprite.bounds.size.y / _bigSprite.bounds.size.y);
        _mainController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainController>();
    }

    private void OnMouseDown()
    {
        _offset = gameObject.transform.position -
                 Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        _offsetWithSmall = _renderer.sprite == _smallSprite;
    }

    private void OnMouseUp()
    {
        var mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        if (_characterCollider2D.bounds.Contains(mouseWorld) && _renderer.sprite == _bigSprite)
        {
            _mainController.RandomCharacter();
        }
    }

    private void OnMouseDrag()
    {
        var mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        var currentOffset = _offset;
        if (_trayCollider2D.bounds.Contains(mouseWorld))
        {
            _renderer.sprite = _smallSprite;
            if (_offsetWithSmall == false)
            {
                currentOffset = currentOffset * _offsetRatio;
            }
            transform.Find("Canvas").gameObject.active = false;
        }
        else
        {
            _renderer.sprite = _bigSprite;
            if (_offsetWithSmall)
            {
                currentOffset = currentOffset / _offsetRatio;
            }
            transform.Find("Canvas").gameObject.active = true;
        }
        
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + currentOffset;
    }

    public void SetPerson(Person person)
    {
        _person = person;
        _firstNameText.text = _person.FirstName;
        _lastNameText.text = _person.Lastname;
        _countryText.text = GetCountryText(_person.Country);
        _dateOfBirthText.text = DateTime.Parse(_person.DateOfBirth).ToShortDateString();
        _expieryText.text = DateTime.Parse(_person.ExpieryDate).ToShortDateString();
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
