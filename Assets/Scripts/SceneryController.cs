using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class SceneryController : MonoBehaviour
{
    private GameObject _firstBackground;
    private GameObject _secondBackground;
    private SpriteRenderer _firstRenderer;
    private SpriteRenderer _secondRenderer;
    private Transform _firstTransform;
    private Transform _secondTransform;
    private Animation _animation;
    
    [SerializeField] private float _speed;

    private void Start()
    {
        _firstBackground = transform.GetChild(0).gameObject;
        _secondBackground = transform.GetChild(1).gameObject;
        _firstRenderer = _firstBackground.GetComponent<SpriteRenderer>();
        _secondRenderer = _secondBackground.GetComponent<SpriteRenderer>();
        _firstTransform = _firstBackground.GetComponent<Transform>();
        _secondTransform = _secondBackground.GetComponent<Transform>();
        _animation = GetComponent<Animation>();
        
        // Fit the backgrounds
        FitIntoCamera(_firstTransform, _firstRenderer);
        FitIntoCamera(_secondTransform, _secondRenderer);
        
        // Set the second background a little further
        _secondTransform.Translate(new Vector3(_firstRenderer.bounds.size.x, 0f, 0f));
    }

    private void Update()
    {
        // Get the real-world camera width
        var topRightCorner = new Vector2(1, 1);
        var edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        var middle = new Vector2(0, 0);
        var middleVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        var cameraWidth = edgeVector.x * 2;
        
        // Move the backgrounds accordingly
        Move(_firstTransform, _firstRenderer, cameraWidth);
        Move(_secondTransform, _secondRenderer, cameraWidth);
    }

    private void FitIntoCamera(Transform backgroundTransform, SpriteRenderer backgroundRenderer)
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = backgroundRenderer.sprite.bounds.size;
        Vector2 scale = backgroundTransform.localScale;
        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }
        
        backgroundTransform.localScale = scale;
    }

    private void Move(Transform backgroundTransform, SpriteRenderer backgroundRenderer, float cameraWidth)
    {
        backgroundTransform.Translate(new Vector3(_speed, 0f, 0f));
        
        if (backgroundTransform.position.x >= backgroundRenderer.bounds.size.x)
            backgroundTransform.position = Vector3.zero - new Vector3(backgroundRenderer.bounds.size.x, 0f, 0f);
    }
}
