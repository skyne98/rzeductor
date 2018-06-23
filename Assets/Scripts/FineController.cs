using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

[RequireComponent(typeof(DraggableController))]
public class FineController : MonoBehaviour {
    private GameObject _characterGameObject;
    private BoxCollider2D _characterCollider2D;
    private Person _person;
    private DraggableController _draggableController;
    private SpriteRenderer _renderer;
    private MainController _mainController;
    
    private void Start()
    {
        _characterGameObject = GameObject.FindGameObjectWithTag("Character");
        _characterCollider2D = _characterGameObject.GetComponent<BoxCollider2D>();
        _draggableController = GetComponent<DraggableController>();
        _mainController = GameObject.FindWithTag("MainCamera").GetComponent<MainController>();
        _renderer = GetComponent<SpriteRenderer>();
        
        _draggableController.Dropped += DraggableControllerOnDropped;
    }

    private void DraggableControllerOnDropped(Vector3 mouseWorldPosition)
    {
        if (_characterCollider2D.bounds.Contains(mouseWorldPosition) && _draggableController.OnTray == false)
        {
            _mainController.FineCharacter();
        }
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
