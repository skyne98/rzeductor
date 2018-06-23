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
    private BoxCollider2D _trayCollider2D;
    private Person _person;
    
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
    }

    private void OnMouseDown()
    {
        _offset = gameObject.transform.position -
                 Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
    }

    private void OnMouseUp()
    {
        Debug.Log("TODO: Implement giving back the documents");
    }

    private void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + _offset;
        var mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        if (_trayCollider2D.bounds.Contains(mouseWorld))
        {
            _renderer.sprite = _smallSprite;
            transform.Find("Canvas").gameObject.active = false;
        }
        else
        {
            _renderer.sprite = _bigSprite;
            transform.Find("Canvas").gameObject.active = true;
        }
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
