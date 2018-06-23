using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableController))]
public class TicketController : MonoBehaviour {
    private GameObject _characterGameObject;
    private BoxCollider2D _characterCollider2D;
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
            _mainController.ReturnDocument(gameObject);
        }
    }
}
