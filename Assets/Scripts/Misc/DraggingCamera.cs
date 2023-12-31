using System;
using UnityEngine;

public class DraggingCamera : MonoBehaviour
{
    private float zoom;
    private float zoomMultiplier = 4f;
    private const float minZoom = 2f;
    private const float maxZoom = 20f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;

    [SerializeField] private const float moveSpeed = 8f;
    [SerializeField] private const float xBorder = 50f;
    [SerializeField] private const float yBorder = 50f;

    private Vector3 initialPosition;
    private Vector3 originPosition;
    private Vector3 difference;
    private bool dragging;

    void Start()
    {
        initialPosition = transform.position;
        zoom = Camera.main.orthographicSize;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        zoom -= scroll * zoomMultiplier;

        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, zoom, ref velocity, smoothTime);
    }

    void LateUpdate()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * moveSpeed * Time.deltaTime;

        Vector3 newPosition = transform.position + tempVect;
        if (Math.Abs(newPosition.x) < xBorder && Math.Abs(newPosition.y) < yBorder)
        {
            transform.position = newPosition;
        }
        

        /*if (Input.GetMouseButton(2))
        {
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (!dragging)
            {
                dragging = true;
                originPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            dragging = false;
        }
        
        if (dragging)
        {
            transform.position = originPosition - difference;
        }

        //RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
        if (Input.GetMouseButton(1))
        {
            transform.position = initialPosition;
        }*/
    }
}
