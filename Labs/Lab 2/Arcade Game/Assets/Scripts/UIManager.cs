using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class UIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject tempCamera;

    public long score = 0;
    public long highscore = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI enterText;

    bool isGamePlaying;
    Vector2 playerSpawn = new Vector2(0, 0);
    Vector2 enemySpawn1 = new Vector2(-16, 4);
    Vector2 enemySpawn2 = new Vector2(16, 4);
    Vector2 enemySpawn3 = new Vector2(0, 7);
    float spawnTimer;
    float spawnCooldown = 1.0f;

    float smoothedDelta = 1f / 30f;
    GUIStyle style;

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

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enterText.text = "Tap to Begin";
        isGamePlaying = false;
        player = PlayerManager.instance.gameObject;
        tempCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        smoothedDelta += (Time.unscaledDeltaTime - smoothedDelta) * 0.1f;

        spawnTimer += Time.deltaTime;

        if (score >= highscore) // Setting highscore
        {
            highscore = score;
        }

        foreach (var touch in Touch.activeTouches)
        {
            if (!isGamePlaying && touch.phase == TouchPhase.Ended) // Game starts once player taps on the screen
            {
                isGamePlaying = true;
                tempCamera.SetActive(false);
                Instantiate(player, playerSpawn, Quaternion.identity);
            }
        }

        if (isGamePlaying)
        {
            enterText.text = "";
        }

        if (isGamePlaying && spawnTimer >= spawnCooldown) // Spawn enemies depending on spawn timer
        {
            spawnTimer = 0;
            Instantiate(enemy, enemySpawn1, Quaternion.identity);
            Instantiate(enemy, enemySpawn2, Quaternion.identity);
            Instantiate(enemy, enemySpawn3, Quaternion.identity);
        }

        if (PlayerManager.instance.playerHealth <= 0 && isGamePlaying) // Game over once player dies
        {
            isGamePlaying = false;
            tempCamera.SetActive(true);
            enterText.text = "Game Over! Tap to Begin";
            score = 0;
        }

        scoreText.text = "Score: " + score;
        healthText.text = "Health: " + PlayerManager.instance.playerHealth;
        highScoreText.text = "Highscore: " + highscore;
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

    public void EnemyKill()
    {
        score += 10;
    }
}
