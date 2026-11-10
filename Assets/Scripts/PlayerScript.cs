using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PlayerScript : MonoBehaviour
{
    const int maxLevel = 30;

    [Range(1, maxLevel)]
    public int level = 1;
    public float ballVelocityMult = 0.02f;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject yellowPrefab;
    public GameObject movingYellowPrefab;
    public GameObject ballPrefab;
    static Collider2D[] colliders = new Collider2D[50];
    static ContactFilter2D contactFilter = new ContactFilter2D();

    public GameDataScript gameData;
    static bool gameStarted = false;

    AudioSource musicSource;
    AudioSource sfxSource;
    public AudioClip pointSound;

    int requiredPointsToBall { get { return 400 + (level - 1) * 20; } }

    string OnOff(bool boolVal)
    {
        return boolVal ? "on" : "off";
    }

    public void PlaySound(AudioClip clip, float volumeScale = 1f)
    {
        if (gameData.sound && sfxSource != null)
            sfxSource.PlayOneShot(clip, volumeScale);
    }

    void CreateBlocks(GameObject prefab, float xMax, float yMax,
        int count, int maxCount)
    {
        if (count > maxCount)
            count = maxCount;
        for (int i = 0; i < count; i++)
            for (int k = 0; k < 20; k++)
            {
                var obj = Instantiate(prefab,
                new Vector3((Random.value * 2 - 1) * xMax,
                Random.value * yMax, 0),
                Quaternion.identity);
                if (obj.GetComponent<Collider2D>()
                .OverlapCollider(contactFilter.NoFilter(), colliders) == 0)
            break;
                Destroy(obj);
            }
    }

    public void CreateBalls(int count, bool spawnOnStart)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(0.6f, 1.5f, 0f);
            var obj = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
            var ball = obj.GetComponent<BallScript>();
            ball.ballInitialForce += new Vector2(2f * i, 0);
            ball.ballInitialForce *= 1 + level * ballVelocityMult;

            if (!spawnOnStart)
            {
                ball.LaunchBall();
            }
        }
    }

    void CreateBalls()
    {
        int count = 2;
        if (gameData.balls == 1) count = 1;
        CreateBalls(count, true);
    }

    void StartLevel()
    {
        var yMax = Camera.main.orthographicSize * 0.8f;
        var xMax = Camera.main.orthographicSize * Camera.main.aspect * 0.85f;
        CreateBlocks(bluePrefab, xMax, yMax, level, 8);
        CreateBlocks(redPrefab, xMax, yMax, 1 + level, 10);
        CreateBlocks(greenPrefab, xMax, yMax, 1 + level, 12);
        CreateBlocks(yellowPrefab, xMax, yMax, 2 + level, 15);
        CreateBlocks(movingYellowPrefab, xMax, yMax, 1 + level, 3);
        CreateBalls();
    }

    void SetBackground()
    {
        var bg = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        bg.sprite = Resources.Load(level.ToString("d2"),typeof(Sprite)) as Sprite;
    }

    IEnumerator BallDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
            if (gameData.balls > 0)
                CreateBalls();
            else
            {

                FindObjectOfType<UIManager>().ShowGameOverPanel();
                Time.timeScale = 0f;
            }
    }

    public void BallDestroyed()
    {
        gameData.balls--;
        StartCoroutine(BallDestroyedCoroutine());
    }

    IEnumerator BlockDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            if (level < maxLevel)
                gameData.level++;
            SceneManager.LoadScene("MainScene");
        }
    }
    IEnumerator BlockDestroyedCoroutine2()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            PlaySound(pointSound, 5f);
        }
    }

    public void BlockDestroyed(int points)
    {
        gameData.points += points;
        if (gameData.sound)
            PlaySound(pointSound, 5f);
        gameData.pointsToBall += points;
        if (gameData.pointsToBall >= requiredPointsToBall)
        {
            gameData.balls++;
            gameData.pointsToBall -= requiredPointsToBall;
            if (gameData.sound)
                StartCoroutine(BlockDestroyedCoroutine2());
        }
        StartCoroutine(BlockDestroyedCoroutine());
    }

    void OnGUI()
    {
        if (Time.timeScale == 0f && (FindObjectOfType<UIManager>().startPanel.activeSelf || FindObjectOfType<UIManager>().gameOverPanel.activeSelf))
            return;

        GUI.Label(new Rect(5, 4, Screen.width - 10, 100), string.Format(
            "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>" +
            " Score <b>{2}</b></size></color>",
            gameData.level, 
            gameData.balls, 
            gameData.points)
        );

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperRight;

        GUI.Label(new Rect(5, 14, Screen.width - 10, 100), string.Format(
            "<color=yellow><size=20><color=white>Space</color>-pause {0}" +
            " <color=white>N</color>-new" +
            " <color=white>J</color>-jump" +
            " <color=white>M</color>-music {1}" +
            " <color=white>S</color>-sound {2}" +
            " <color=white>Esc</color>-exit</size></color>",
            OnOff(Time.timeScale > 0), 
            OnOff(!gameData.music),
            OnOff(!gameData.sound)), 
            style
        );
    }

    public void SetMusic()
    {
        if (gameData.music)
        {
            musicSource.volume = gameData.musicVolume;
            if (!musicSource.isPlaying) musicSource.Play();
        }
        else musicSource.Stop();
    }

    public void SetMusicVolume() { musicSource.volume = gameData.musicVolume; }

    public void SetSoundVolume()
    {
        if (sfxSource != null) sfxSource.volume = gameData.soundVolume;
    }

    void OnApplicationQuit()
    {
        gameData.Save();
    }

    public void StartGame()
    {
        Cursor.visible = false;
        level = gameData.level;
        SetBackground();
        StartLevel();
    }

    void Awake()
    {
        musicSource = Camera.main.GetComponents<AudioSource>()[0];
        sfxSource = Camera.main.GetComponents<AudioSource>()[1];
    }

    void Start()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            if (gameData.resetOnStart)
                gameData.Load();
        }
        level = gameData.level;
        SetMusic();
        SetSoundVolume();
        SetBackground();
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos = transform.position;
            pos.x = mousePos.x;
            transform.position = pos;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            gameData.music = !gameData.music;
            SetMusic();
        }

        if (Input.GetKeyDown(KeyCode.S))
            gameData.sound = !gameData.sound;

        if (Input.GetKeyDown(KeyCode.N))
        {
            gameData.Reset();
            SceneManager.LoadScene("MainScene");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public void ApplyBonus(int points)
    {
        gameData.points += points;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Bonus")
        {
            collision.transform.GetComponent<BonusBase>().BonusActivate(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Bonus")
        {
            collision.transform.GetComponent<BonusBase>().BonusActivate(this);
        }
    }
}