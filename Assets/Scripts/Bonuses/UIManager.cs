using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameDataScript gameData;
    public PlayerScript playerScript;

    public GameObject startPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public Toggle startMusicToggle;
    public Slider startMusicSlider;
    public Toggle startSoundToggle;
    public Slider startSoundSlider;
    public Button playButton;

    public Toggle pauseMusicToggle;
    public Slider pauseMusicSlider;
    public Toggle pauseSoundToggle;
    public Slider pauseSoundSlider;
    public Button resumeButton;
    public Button newGameButton;
    public Button pauseExitButton;

    public Toggle gameOverMusicToggle;
    public Slider gameOverMusicSlider;
    public Toggle gameOverSoundToggle;
    public Slider gameOverSoundSlider;
    public Button gameOverPlayButton;
    public Button gameOverExitButton;

    void Start()
    {
        SyncUIWithData();
        ShowStartPanel();
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && !startPanel.activeSelf && !gameOverPanel.activeSelf)
        {
            if (Time.timeScale > 0) PauseGame();
            else ResumeGame();
        }
    }

    void SyncUIWithData()
    {
        if (gameData == null) return;

        if (startMusicToggle != null)
        {
            startMusicToggle.SetIsOnWithoutNotify(gameData.music);
            startMusicSlider.SetValueWithoutNotify(gameData.musicVolume);
            startMusicSlider.interactable = gameData.music;
        }
        if (startSoundToggle != null)
        {
            startSoundToggle.SetIsOnWithoutNotify(gameData.sound);
            startSoundSlider.SetValueWithoutNotify(gameData.soundVolume);
            startSoundSlider.interactable = gameData.sound;
        }
        if (pauseMusicToggle != null)
        {
            pauseMusicToggle.SetIsOnWithoutNotify(gameData.music);
            pauseMusicSlider.SetValueWithoutNotify(gameData.musicVolume);
            pauseMusicSlider.interactable = gameData.music;
        }
        if (pauseSoundToggle != null)
        {
            pauseSoundToggle.SetIsOnWithoutNotify(gameData.sound);
            pauseSoundSlider.SetValueWithoutNotify(gameData.soundVolume);
            pauseSoundSlider.interactable = gameData.sound;
        }
        if (gameOverMusicToggle != null)
        {
            gameOverMusicToggle.SetIsOnWithoutNotify(gameData.music);
            gameOverMusicSlider.SetValueWithoutNotify(gameData.musicVolume);
            gameOverMusicSlider.interactable = gameData.music;
        }
        if (gameOverSoundToggle != null)
        {
            gameOverSoundToggle.SetIsOnWithoutNotify(gameData.sound);
            gameOverSoundSlider.SetValueWithoutNotify(gameData.soundVolume);
            gameOverSoundSlider.interactable = gameData.sound;
        }
    }

    public void OnMusicToggleChanged(bool isOn)
    {
        gameData.music = isOn;
        if (startMusicSlider != null) startMusicSlider.interactable = isOn;
        if (pauseMusicSlider != null) pauseMusicSlider.interactable = isOn;
        if (gameOverMusicSlider != null) gameOverMusicSlider.interactable = isOn;
        if (playerScript != null) playerScript.SetMusic();
    }

    public void OnMusicVolumeChanged(float val)
    {
        gameData.musicVolume = val;
        if (playerScript != null) playerScript.SetMusicVolume();
        gameData.Save();
    }

    public void OnSoundToggleChanged(bool isOn)
    {
        gameData.sound = isOn;
        if (startSoundSlider != null) startSoundSlider.interactable = isOn;
        if (pauseSoundSlider != null) pauseSoundSlider.interactable = isOn;
        if (gameOverSoundSlider != null) gameOverSoundSlider.interactable = isOn;
    }

    public void OnSoundVolumeChanged(float val)
    {
        gameData.soundVolume = val;
        if (playerScript != null) playerScript.SetSoundVolume();
        gameData.Save();
    }

    public void ShowStartPanel()
    {
        startPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverPanel()
    {
        Cursor.visible = true;
        startPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        SyncUIWithData();
    }

    public void StartGame()
    {
        Cursor.visible = false;
        startPanel.SetActive(false);
        Time.timeScale = 1f;
        SyncUIWithData();
        if (playerScript != null) playerScript.StartGame();
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        SyncUIWithData();
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
        gameData.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}