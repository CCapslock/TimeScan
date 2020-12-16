using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
	public GameObject WinPanel;

	public Animator[] StarsImages;
	public Animator BonusPanel;

	public Image FirstImage;
	public Animator FirstImageAnimator;
	public Image SecondImage;
	public Animator SecondImageAnimator;
	public Image ThirdImage;
	public Animator ThirdImageAnimator;
	public Text WordsText;
	public Animator GoodWordsAnimator;
	public Text NumbersText;
	public Animator NumbersAnimatoe;
	public string[] GoodWords;
	public float BonusDelayTime;

	private MainGameController _gameController;
	private ScreenWrapModel _screenWrapModel;

	private int _percentNum;

	private void Start()
	{
		_gameController = FindObjectOfType<MainGameController>();
		_screenWrapModel = FindObjectOfType<ScreenWrapModel>();
	}
	public void ActivateBonusLvlUi()
	{
		BonusPanel.SetTrigger("PopUp");
	}
	public void ActivateBonusWinPanel(float DelayTime)
	{
		Invoke("SetBonusPanelActive", DelayTime);
	}
	public void ActivateWinPanel(float DelayTime)
	{
		Invoke("SetPanelActive", DelayTime);
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
	private void SetBonusPanelActive()
	{
		WinPanel.SetActive(true);
		ThirdImage.sprite = _screenWrapModel.GetCompletedSceenshot();
		WordsText.text = GoodWords[Random.Range(0, GoodWords.Length)];
		StartBonusWinPanelAnimation();
	}

	private void StartBonusWinPanelAnimation()
	{
		NumbersText.text = "";
		Invoke("PopUpThirdImage", 0.5f);
		Invoke("PopUpGoodWordsText", 0.7f);
		Invoke("PopUpAllStars", 1f);
	}
	private void StartWinPanelAnimation()
	{
		NumbersText.text = "";
		Invoke("PopUpFirstImage", 0.5f);
		Invoke("PopUpSecondImage", 1f);
		Invoke("CountPercent", 1.5f);
	}
	private void PopUpAllStars()
	{
		for (int i = 0; i < StarsImages.Length; i++)
		{
			StarsImages[i].SetTrigger("PopUp");
		}
	}
	private void PopUpFirstImage()
	{
		FirstImageAnimator.SetTrigger("PopUp");
	}
	private void PopUpSecondImage()
	{
		SecondImageAnimator.SetTrigger("PopUp");
	}
	private void PopUpThirdImage()
	{
		ThirdImageAnimator.SetTrigger("PopUp");
	}
	private void CountPercent()
	{
		_percentNum = 0;
		int randomNum = 100;//Random.Range(75, 100);
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
		NumbersText.text = (_percentNum).ToString() + "% MATCH";
		switch (_percentNum)
		{
			case 25:
				StarsImages[0].SetTrigger("PopUp");
				return;
			case 50:
				StarsImages[1].SetTrigger("PopUp");
				return;
			case 75:
				StarsImages[2].SetTrigger("PopUp");
				return;
		}
	}
}
