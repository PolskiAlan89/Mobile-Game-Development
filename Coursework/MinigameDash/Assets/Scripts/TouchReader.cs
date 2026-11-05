using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TouchReader : MonoBehaviour
{

    const float minSwipeDp = 50f;
    const float maxSwipeTime = 0.4f;
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
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
                Debug.Log($"Finger {touch.finger.index} down");
                CheckSwipe(touch);
            }
        }
    }

    void OnSwipe(Vector2 vector2)
    {
        Debug.Log("Swiped");
    }

    void CheckSwipe(Touch touch)
    {
        if (touch.phase != TouchPhase.Ended)
        {
            return;
        }

        Vector2 delta = touch.screenPosition - touch.startScreenPosition;
        float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;
        float distanceDp = delta.magnitude / (dpi / 160f);
        float time = (float) (touch.time - touch.startTime);

        if (distanceDp > minSwipeDp || time > maxSwipeTime)
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
