using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameInputField;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private TextMeshProUGUI highscoreHeadline;
    [SerializeField]
    private TextMeshProUGUI highscoreText;

    // Start is called before the first frame update
    void Start()
    {
        RecordedScore highScore = SaveDataManager.Instance.HighScore;
        if (highScore != null && highScore.PlayerName != "")
        {
            highscoreText.text = $"{highScore.PlayerName}: {highScore.Score} points";
            highscoreHeadline.gameObject.SetActive(true);
            highscoreText.gameObject.SetActive(true);
        }
        else
        {
            highscoreHeadline.gameObject.SetActive(false);
            highscoreText.gameObject.SetActive(false);
        }

        nameInputField.onValueChanged.AddListener(HandleNameChanged);

        string playerName = SaveDataManager.Instance.PlayerName;
        if (playerName != null && playerName.Trim() != "")
        {
            nameInputField.text = playerName;
            HandleNameChanged(playerName);
        }
    }

    void HandleNameChanged(string newName)
    {
        if (newName.Trim() == "")
        {
            Debug.Log("Button disabled");
            startButton.interactable = false;
        }
        else
        {
            Debug.Log("Button enabled");
            startButton.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void StartGame()
    {
        SaveDataManager.Instance.PlayerName = nameInputField.text;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        SaveDataManager.Instance.PlayerName = nameInputField.text;
        SaveDataManager.Instance.SaveSaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
