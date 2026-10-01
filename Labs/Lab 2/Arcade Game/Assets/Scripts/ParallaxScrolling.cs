using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{

    public float scrollAmount;
    Vector3 cameraStartPos;
    Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        cameraStartPos = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraMotion = transform.parent.position - cameraStartPos;
        transform.position = startPos + scrollAmount * cameraMotion;
    }
}
