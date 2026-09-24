using UnityEngine;

public class DeviceInfoOverlay : MonoBehaviour
{
    float smoothedDelta = 1f / 30f;
    GUIStyle style;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        smoothedDelta += (Time.unscaledDeltaTime - smoothedDelta) * 0.1f;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.box);
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.white;
        }

        style.fontSize = Mathf.Max(14, Screen.height / 40);

        float fps = 1f / smoothedDelta;
        Resolution res = Screen.currentResolution;

        string info =
            $"FPS: {fps:0}\n" +
            $"Device: {SystemInfo.deviceModel}\n" +
            $"OS: {SystemInfo.operatingSystem}\n" +
            $"Screen: {Screen.width} x {Screen.height} @ {res.refreshRateRatio.value:0} Hz\n" +
            $"DPI: {Screen.dpi:0}\n" +
            $"GPU: {SystemInfo.graphicsDeviceName} ({SystemInfo.graphicsMemorySize / 1000} GB)\n" +
            $"RAM: {SystemInfo.systemMemorySize / 1000} GB\n";


        Vector2 size = style.CalcSize(new GUIContent(info));
        GUI.Label(new Rect(40, 40, size.x + 20, size.y + 20), info, style);
    }


}
