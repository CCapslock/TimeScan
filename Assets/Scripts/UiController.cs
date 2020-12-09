using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public GameObject WinPanel;

    public Image FirstImage;
    public Animator FirstImageAnimator;
    public Image SecondImage;
    public Animator SecondImageAnimator;
    public Text WordsText;
    public Animator GoodWordsAnimator;
    public Text NumbersText;
    public Animator NumbersAnimatoe;
    public string[] GoodWords;

    private MainGameController _gameController;
    private ScreenWrapModel _screenWrapModel;

    private int _percentNum;

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
        StartWinPanelAnimation();
    }

    private void StartWinPanelAnimation()
    {
        NumbersText.text = "";
        Invoke("PopUpFirstImage", 0.5f);
        Invoke("PopUpSecondImage", 1f);
        Invoke("CountPercent", 1.5f);
    }

    private void PopUpFirstImage()
    {
        FirstImageAnimator.SetTrigger("PopUp");
    }
    private void PopUpSecondImage()
    {
        SecondImageAnimator.SetTrigger("PopUp");
    }
    private void CountPercent()
    {
          _percentNum = 0;
        int randomNum = Random.Range(75, 100);
        for (int i = 0; i < randomNum; i++)
        {
            Invoke("AddPercent", i * 0.007f);
        }
        Invoke("PopUpPersentText", randomNum * 0.007f);
        Invoke("PopUpGoodWordsText", randomNum * 0.007f + 0.4f);
    }
    private void PopUpGoodWordsText()
    {
        GoodWordsAnimator.SetTrigger("WordsTextPopUp");
    }
    private void PopUpPersentText()
    {
        NumbersAnimatoe.SetTrigger("TextPopUp");
    }
    private void AddPercent()
    {
        _percentNum++;
        NumbersText.text = (_percentNum).ToString() + "%";
    }
}
