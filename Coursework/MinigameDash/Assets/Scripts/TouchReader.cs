using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchReader : MonoBehaviour
{

    const float minSwipeDp = 20f;
    const float maxSwipeTime = 1f;
    Vector2 startPosition;
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.screenPosition;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                CheckSwipe(touch);
            }
        }
    }

    void OnSwipe(Vector2 vector2)
    {
        switch (vector2.x, vector2.y)
        {
            case (1, 0):
                Debug.Log("Swiped right");
                    break;
            case (0, 1):
                Debug.Log("Swiped up");
                break;
            case (-1, 0):
                Debug.Log("Swiped left");
                break;
            case (0, -1):
                Debug.Log("Swiped down");
                break;
        }
    }

    void CheckSwipe(Touch touch)
    {
        Vector2 delta = touch.screenPosition - touch.startScreenPosition;
        Vector2 direction = Vector2.zero;
        float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;
        float distanceDp = delta.magnitude / (dpi / 160f);
        float time = (float) (touch.time - touch.startTime);

        if (distanceDp < minSwipeDp || time > maxSwipeTime)
        {
            return;
        }

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            OnSwipe(delta.x > 0 ? Vector2.right : Vector2.left);
        }
        else
        {
            OnSwipe(delta.y > 0 ? Vector2.up : Vector2.down);
        }
    }
}
