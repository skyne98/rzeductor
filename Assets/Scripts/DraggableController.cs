using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DraggableController : MonoBehaviour {
    private Vector3 _offset;
    private SpriteRenderer _renderer;
    private BoxCollider2D _boxCollider2D;
    private GameObject _trayGameObject;
    private BoxCollider2D _trayCollider2D;
    private Vector2 _offsetRatio;
    private bool _offsetWithSmall;
    private MainController _mainController;
    private bool _virtualDragging;

    [SerializeField] private Sprite _bigSprite;
    [SerializeField] private Sprite _smallSprite;

    public delegate void DroppedHandler(Vector3 mouseWorldPosition);
    public event DroppedHandler Dropped;

    public bool OnTray;
    public bool Dragged;
    
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _trayGameObject = GameObject.FindGameObjectWithTag("Tray");
        _trayCollider2D = _trayGameObject.GetComponent<BoxCollider2D>();
        _offsetRatio = new Vector2(_smallSprite.bounds.size.x / _bigSprite.bounds.size.x, _smallSprite.bounds.size.y / _bigSprite.bounds.size.y);
        _mainController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainController>();
    }

    public void StartDragging()
    {
        if (_mainController.MouseState == MouseState.Idle)
        {
            _virtualDragging = true;
            OnPress();
        }
    }

    private void Update()
    {
        if (_virtualDragging)
        {
            OnDrag();
            if (Input.GetMouseButtonUp(0))
                OnMouseUp();
        }
    }

    private void OnMouseDown()
    {
        OnPress();
    }

    private void OnPress()
    {
        if (_mainController.MouseState == MouseState.Idle)
        {
            _offset = gameObject.transform.position -
                      Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            _mainController.MouseState = MouseState.Dragging;
            Dragged = true;
        }
    }

    private void OnMouseUp()
    {
        if (Dragged)
        {
            if (Dropped != null)
                Dropped(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)));
            Dragged = false;
            _virtualDragging = false;
            _mainController.MouseState = MouseState.Idle;
        }
    }

    private void OnMouseDrag()
    {
        OnDrag();
    }

    private void OnDrag()
    {
        if (Dragged)
        {
            var mouseWorld =
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            var currentOffset = _offset;
            if (_trayCollider2D.bounds.Contains(mouseWorld))
            {
                _renderer.sprite = _smallSprite;
                OnTray = true;
                _boxCollider2D.offset = new Vector2(0, 0);
                _boxCollider2D.size = new Vector3(_renderer.sprite.bounds.size.x / transform.lossyScale.x,
                    _renderer.sprite.bounds.size.y / transform.lossyScale.y,
                    _renderer.sprite.bounds.size.z / transform.lossyScale.z);
                if (_offsetWithSmall == false)
                {
                    currentOffset = currentOffset * _offsetRatio;
                }
            }
            else
            {
                _renderer.sprite = _bigSprite;
                OnTray = false;
                _boxCollider2D.offset = new Vector2(0, 0);
                _boxCollider2D.size = new Vector3(_renderer.sprite.bounds.size.x / transform.lossyScale.x,
                    _renderer.sprite.bounds.size.y / transform.lossyScale.y,
                    _renderer.sprite.bounds.size.z / transform.lossyScale.z);
                if (_offsetWithSmall)
                {
                    currentOffset = currentOffset / _offsetRatio;
                }
            }

            Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            transform.position = Camera.main.ScreenToWorldPoint(newPosition) + currentOffset;
        }
    }
}
