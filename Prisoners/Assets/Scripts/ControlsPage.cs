using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPage : MonoBehaviour {
    private bool show = true;

    void Start () {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (false)//cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }
        
        transform.localScale = scale;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Controls")
            || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            show = !show;
        }
        GetComponent<SpriteRenderer>().enabled = show; 
    }
}
