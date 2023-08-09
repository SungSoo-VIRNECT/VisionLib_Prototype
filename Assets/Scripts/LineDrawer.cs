using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float lineWidth = 0.1f;
    public Color lineColor = Color.white;
    public Slider slider;
    private Camera mainCamera;
    private bool isDrawing = false;
    private bool isFading = false;

    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        slider.onValueChanged.AddListener(SlideClickCheck);
    }


    private void SlideClickCheck(float value)
    {
        isFading = true;
    }

    void Update()
    {
        if (Input.touchCount > 0 && isFading != true)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDrawing = true;
                lineRenderer.positionCount = 1;
                lineRenderer.SetPosition(0, GetTouchWorldPosition(touch));
            } 
            else if (touch.phase == TouchPhase.Moved && isDrawing)
            {
                int newPositionCount = lineRenderer.positionCount + 1;
                lineRenderer.positionCount = newPositionCount;
                lineRenderer.SetPosition(newPositionCount - 1, GetTouchWorldPosition(touch));
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDrawing = false;
            }
        }
        isFading = false;

    }

    private Vector3 GetTouchWorldPosition(Touch touch)
    {
        Vector3 touchPosition = touch.position; 
        touchPosition.z = 5; // Set an arbitrary distance from the camera
        return mainCamera.ScreenToWorldPoint(touchPosition);
    }
}
