using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableController))]
public class RulebookController : MonoBehaviour {
    private GameObject _characterGameObject;
    private BoxCollider2D _characterCollider2D;
    private DraggableController _draggableController;
    private SpriteRenderer _renderer;
    private MainController _mainController;
    
    [SerializeField] private Text _mainTextBox;
    
    private void Update()
    {
        transform.Find("Canvas").gameObject.SetActive(!_draggableController.OnTray);
    }
    
    private void Start()
    {
        _characterGameObject = GameObject.FindGameObjectWithTag("Character");
        _characterCollider2D = _characterGameObject.GetComponent<BoxCollider2D>();
        _draggableController = GetComponent<DraggableController>();
        _mainController = GameObject.FindWithTag("MainCamera").GetComponent<MainController>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void SetRules(Country privilegedCountry, DateTime minimumDateOfBirth)
    {
        _mainTextBox.text = "1. Only people from " + GetCountryText(privilegedCountry) + " are alowed to travel for free" + Environment.NewLine
            + "2. All of them must be born after " + minimumDateOfBirth.ToLongDateString() + "";
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