using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class FineStackController : MonoBehaviour
{

    private MainController _mainController;
    
    [SerializeField] private GameObject _finePrefab;

    private void Start()
    {
        _mainController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainController>();
    }

    private void OnMouseDown()
    {
        // Create a new fine
        var newFine = Instantiate(_finePrefab, transform);
        _mainController.AddDocument(newFine);
        
        // Start dragging it
        newFine.GetComponent<DraggableController>().StartDragging();
    }
}
