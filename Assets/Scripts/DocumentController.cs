using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DocumentController : MonoBehaviour {
    private Vector3 offset;
    
    private void OnMouseDown()
    {
        offset = gameObject.transform.position -
                 Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
    }

    private void OnMouseDrag()
    {
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        
    }
}
