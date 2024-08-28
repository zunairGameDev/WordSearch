using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region editor references
    [SerializeField] private LevelCompleteScreen levelCompleteScreen;
    [SerializeField] private MainMenuScreen mainMenuScreen;
    [SerializeField] private GameplayScreen gameplayScreen;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    #endregion

    private void Start()
    {
        HideLevelCompleteScreen();
        ShowMainMenuScreen();
        AssignOnClickListeners();
        ObserveEvents();
    }
    public GridLayoutGroup GetGridLayoutGroup()
    {
        return gridLayoutGroup;
    }
    public Transform GetGridTransform()
    {
        return gridLayoutGroup.GetComponent<Transform>();
    }
    public RectTransform GetGridRectTransform()
    {
        return gridLayoutGroup.GetComponent<RectTransform>();
    }
    public void SetStartButtonText(string startButtonText)
    {
        mainMenuScreen.SetStartButtonText(startButtonText);
    }
    private void ObserveEvents()
    {
        GameManager.Instance.onLevelCompleted += ShowLevelCompleteScreen;
    }
    private void AssignOnClickListeners()
    {
        mainMenuScreen.GetStartButton().onClick.RemoveAllListeners();
        mainMenuScreen.GetStartButton().onClick.AddListener(
            () =>
            {
                GameManager.Instance.StartGame();
                mainMenuScreen.Hide();
                gameplayScreen.Show();
            }
            );

        levelCompleteScreen.GetNextLevelButton().onClick.RemoveAllListeners();
        levelCompleteScreen.GetNextLevelButton().onClick.AddListener(
            () =>
            {
                GameManager.Instance.ReturnToMainMenu();
            }
            );
    }
    private void ShowMainMenuScreen()
    {
        mainMenuScreen.Show();
    }
    private void HideMainMenuScreen()
    {
        mainMenuScreen.Hide();
    }
    private void ShowLevelCompleteScreen(int levelNumber)
    {
        levelCompleteScreen.Show();
    }
    private void HideLevelCompleteScreen()
    {
        levelCompleteScreen.Hide();
    }
}
