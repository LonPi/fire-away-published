using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject[] Enemies;
    public Transform[] SpawnPositions;
    public float spawnFrequency;
    public Player _playerRef { get; private set; }
    public Tree _treeRef { get; private set; }
    public Camera _cameraRef { get; private set; }
    public GameplayCanvas _gameplayCanvas { get; private set; }
    public CameraShake _cameraShake { get; private set; }
    public int currentLevel { get; private set; }
    public float currentExp { get; private set; }
    public float expRequiredToCompleteLevel { get; private set; }

    Vector2 curSpawnPosition;
    float fadeTime = 2.0f;
    public HighScore highScore;
    bool isDisplayingHighScore;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void InitReferences()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _treeRef = GameObject.FindGameObjectWithTag("Tree").GetComponent<Tree>();
        _cameraRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _gameplayCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas").GetComponent<GameplayCanvas>();
        _cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    void ResetProgression()
    {
        currentLevel = 1;
        currentExp = 0;
        expRequiredToCompleteLevel = 200; // for level 1
        isDisplayingHighScore = false;
    }

    void RecordHighScore()
    {
        if (currentLevel > highScore.maxLevel)
            highScore.maxLevel = currentLevel;
        if (_playerRef.killCount > highScore.maxKills)
            highScore.maxKills = _playerRef.killCount;
    }

    public void DisplayHighScore(string name)
    {
        if (isDisplayingHighScore)
            return;
        RecordHighScore();
        StopAllCoroutines();
        _gameplayCanvas.OnDisplayHighScore(name);
        _playerRef.OnDisplayHighScore();
        _treeRef.OnDisplayHighScore();
        isDisplayingHighScore = true;
    }

    public void ReloadLevel()
    {
        StopAllCoroutines();
        StartCoroutine(_ReloadLevel());
    }

    public void IncrementExp(float exp)
    {
        this.currentExp += exp;
        //Debug.Log("gained " + exp + " currrent exp: " + currentExp);
        if (currentExp >= expRequiredToCompleteLevel)
        {
            currentLevel++;
            currentExp = 0;
            expRequiredToCompleteLevel += expRequiredToCompleteLevel * 0.2f;
            expRequiredToCompleteLevel =  Mathf.Floor(expRequiredToCompleteLevel);
            _gameplayCanvas.OnLevelUp();
            _playerRef.OnLevelUp();
            SoundManager.instance.PlayerPlayOneShot(SoundManager.instance.levelUpSFX);
        }
    }

    IEnumerator _ReloadLevel()
    {
        fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    IEnumerator _SpawnEnemy()
    {
        while (true)
        {
            float spawnIndex = Random.Range(0, SpawnPositions.Length-0.01f);
            float enemyIndex = Random.Range(0, Enemies.Length - 0.01f);
            curSpawnPosition = SpawnPositions[(int)Mathf.Floor(spawnIndex)].position;
            if (PoolManager.instance.IsInitialized)
            {
                GameObject selectedEnemy = Enemies[(int)Mathf.Floor(enemyIndex)];
                GameObject enemyObj = PoolManager.instance.GetObjectfromPool(selectedEnemy);
                enemyObj.GetComponent<Enemy>().SetParams(currentLevel, curSpawnPosition);
            }
            yield return new WaitForSeconds(1 / spawnFrequency);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitReferences();
        ResetProgression();
        PoolManager.instance.OnSceneLoaded();
        StartCoroutine(_SpawnEnemy());
    }

    public struct HighScore
    {
        public int maxKills, maxLevel;
        public void Reset()
        {
            maxKills = maxLevel = 0;
        }
    }
}
