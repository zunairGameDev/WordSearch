using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager Instance;
    #endregion

    #region dependencies
    private GameplayController _gameplayController;
    private LevelManager _levelManager;
    private UIManager _uiManager;
    #endregion

    #region events
    public Action onLevelStarted;
    public Action<int> onLevelCompleted;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadAndInstantiateControllers();
        _uiManager.SetStartButtonText(_levelManager.GetCurrentLevelName().ToUpper());
    }
    public void StartGame()
    {
        int currentLevel = SaveLoadManager.LoadLevel();
        int rows = _levelManager.GetRowSize(currentLevel);
        int columns = _levelManager.GetColumnSize(currentLevel);

        _gameplayController.SetGridSize(rows, columns);
        _gameplayController.SetWordSource(_levelManager.GetLevelDataBasedOnDifficulty(currentLevel));

        _gameplayController.Setup(currentLevel);
    }
    public void LevelCompleted()
    {
        int levelNumber;
        if (_levelManager.HasMoreLevels())
        {
            levelNumber = SaveLoadManager.LoadLevel();
            levelNumber++;
            SaveLoadManager.SaveLevel(levelNumber);
        }
        else
        {
            levelNumber = 0;
            SaveLoadManager.SaveLevel(levelNumber);
        }
        onLevelCompleted?.Invoke(levelNumber);
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    private void LoadAndInstantiateControllers()
    {
        // loading
        GameplayController gameplayConrollerRef = Resources.Load<GameplayController>(Paths.GAMEPLAY_CONTROLLER);
        LevelManager levelManagerRef = Resources.Load<LevelManager>(Paths.LEVEL_MANAGER);
        UIManager uiManagerRef = Resources.Load<UIManager>(Paths.UI_CANVAS);

        //instantiating
        _gameplayController = Instantiate(gameplayConrollerRef);
        _levelManager = Instantiate(levelManagerRef);
        _uiManager = Instantiate(uiManagerRef);

        //inject dependencies
        _gameplayController.InjectDependencies(_uiManager);
    }
    private void QuitApplication()
    {
        Application.Quit();
    }
}
