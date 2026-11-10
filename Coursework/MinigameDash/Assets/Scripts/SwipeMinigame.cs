using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class SwipeMinigame : MonoBehaviour
{

    const float minSwipeDp = 50f;
    const float maxSwipeTime = 1f;
    Vector2 startPosition;
    int randomPosition;
    int score;
    public TextMeshProUGUI swipeDirection;
    public TextMeshProUGUI scoreText;

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
        ShowDirection();
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

        scoreText.text = "Score: " + score;
    }

    void ShowDirection()
    {
        randomPosition = Random.Range(0, 4);
        if (randomPosition == 0)
        {
            swipeDirection.text = "Swipe Right";
        }
        else if (randomPosition == 1)
        {
            swipeDirection.text = "Swipe Up";
        }
        else if (randomPosition == 2)
        {
            swipeDirection.text = "Swipe Left";
        }
        else if (randomPosition == 3)
        {
            swipeDirection.text = "Swipe Down";
        }
    }

    void OnSwipe(Vector2 vector2)
    {
        switch (vector2.x, vector2.y)
        {
            case (1, 0):
                if (randomPosition == 0)
                {
                    ShowDirection();
                    score++;
                }
                Debug.Log("Swiped right");
                break;
            case (0, 1):
                if (randomPosition == 1)
                {
                    ShowDirection();
                    score++;
                }
                Debug.Log("Swiped up");
                break;
            case (-1, 0):
                if (randomPosition == 2)
                {
                    ShowDirection();
                    score++;
                }
                Debug.Log("Swiped left");
                break;
            case (0, -1):
                if (randomPosition == 3)
                {
                    ShowDirection();
                    score++;
                }
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
