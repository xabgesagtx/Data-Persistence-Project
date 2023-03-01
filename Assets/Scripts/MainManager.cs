using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private string m_PlayerName = "";

    
    // Start is called before the first frame update
    void Start()
    {
        m_PlayerName = SaveDataManager.Instance.PlayerName;
        UpdateHighScoreText();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void UpdateHighScoreText()
    {
        var highScore = SaveDataManager.Instance.HighScore;
        if (highScore != null && highScore.PlayerName != "")
        {
            HighScoreText.text = $"Current HighScore: {highScore.PlayerName} - {highScore.Score} points";
            HighScoreText.gameObject.SetActive(true);
        }
        else
        {
            HighScoreText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            return;
        }
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{m_PlayerName}: {m_Points} points";
    }

    public void GameOver()
    {
        var currentHighScore = SaveDataManager.Instance.HighScore;
        if (currentHighScore == null || currentHighScore.Score < m_Points)
        {
            SaveDataManager.Instance.HighScore = new RecordedScore()
            {
                PlayerName = m_PlayerName,
                Score = m_Points
            };
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
