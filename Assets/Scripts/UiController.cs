using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public GameObject WinPanel;

    public Image FirstImage;
    public Image SecondImage;
    public Text WordsText;
    public Text NumbersText;
    public string[] GoodWords;

    private MainGameController _gameController;
    private ScreenWrapModel _screenWrapModel;

    private void Start()
    {
        _gameController = FindObjectOfType<MainGameController>();
        _screenWrapModel = FindObjectOfType<ScreenWrapModel>();
    }

    public void ActivateWinPanel(float DelatTime)
    {
        Invoke("SetPanelActive", DelatTime);
    }
    public void DisActivateWinPanel()
    {
        WinPanel.SetActive(false);
    }
    private void SetPanelActive()
    {
        WinPanel.SetActive(true);
        FirstImage.sprite = _gameController.GetQuestSprite();
        SecondImage.sprite = _screenWrapModel.GetCompletedSceenshot();
        WordsText.text = GoodWords[Random.Range(0, GoodWords.Length)];
    }
}
