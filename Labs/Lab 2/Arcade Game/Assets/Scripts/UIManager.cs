using System.Data;
using TMPro;
using UnityEngine;

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
    float spawnCooldown = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enterText.text = "Press Enter to Begin";
        isGamePlaying = false;
        player = PlayerManager.instance.gameObject;
        tempCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (score >= highscore) // Setting highscore
        {
            highscore = score;
        }

        if (!isGamePlaying && Input.GetButtonDown("Submit")) // Game starts once enter is pressed
        {
            isGamePlaying = true;
            tempCamera.SetActive(false);
            Instantiate(player, playerSpawn, Quaternion.identity);
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
            enterText.text = "Game Over! Press Enter to Begin";
            score = 0;
        }

        scoreText.text = "Score: " + score;
        healthText.text = "Health: " + PlayerManager.instance.playerHealth;
        highScoreText.text = "Highscore: " + highscore;
    }

    public void EnemyKill()
    {
        score += 10;
    }
}
