using UnityEngine;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour
{
	public GameObject[] LvlPresets;
	public GameObject[] BonusLvlPresets;

	private GameObject CurrentLVL;
	private LvlSettings[] _lvlSettingsForBasicLvls;
	private LvlSettings[] _lvlSettingsForBonusLvls;

	private QuestImageBehavior _image;
	private ScreenWrapModel _screenWrapModel;
	private UiController _uiController;
	private SaveController _saveController;
	private int _lvlNum;
	private bool _needToCheckForImage;
	private bool _needToSaveLvl = true;
	private bool _isBonusLvl;

	private void Awake()
	{
		_screenWrapModel = FindObjectOfType<ScreenWrapModel>();
		_uiController = FindObjectOfType<UiController>();
		_saveController = FindObjectOfType<SaveController>();
		_image = FindObjectOfType<QuestImageBehavior>();

		_lvlSettingsForBasicLvls = new LvlSettings[LvlPresets.Length];
		for (int i = 0; i < _lvlSettingsForBasicLvls.Length; i++)
		{
			_lvlSettingsForBasicLvls[i] = LvlPresets[i].GetComponent<LvlSettings>();
		}
		_lvlSettingsForBonusLvls = new LvlSettings[BonusLvlPresets.Length];
		for (int i = 0; i < _lvlSettingsForBonusLvls.Length; i++)
		{
			_lvlSettingsForBonusLvls[i] = BonusLvlPresets[i].GetComponent<LvlSettings>();
		}
		_screenWrapModel.CreateRenderes();
	}
	private void Start()
	{
		_isBonusLvl = _saveController.IsNextLvlBonus();
		if (_isBonusLvl)
		{
			_lvlNum = _saveController.GetBonusLvlNum();
		}
		else
		{
			_lvlNum = _saveController.GetNextLvlNum();
		}
		SpawnLvl();
	}
	private void FixedUpdate()
	{
		if (_needToCheckForImage)
		{
			if (_image.ImageInFinishPosition)
			{
				_needToCheckForImage = false;
				ActivateScan();
			}
		}
	}
	public void SpawnLvl()
	{
		if (_isBonusLvl)
		{
			_screenWrapModel.PrepareScan(_lvlSettingsForBonusLvls[_lvlNum].Direction);
			CurrentLVL = Instantiate(BonusLvlPresets[_lvlNum], new Vector3(0, 0, 0), Quaternion.identity);
			_image.TurnOffImage();
			_uiController.ActivateBonusLvlUi();
			Invoke("ActivateScan", _uiController.BonusDelayTime);
		}
		else
		{
			_screenWrapModel.PrepareScan(_lvlSettingsForBasicLvls[_lvlNum].Direction);
			CurrentLVL = Instantiate(LvlPresets[_lvlNum], new Vector3(0, 0, 0), Quaternion.identity);
			LvlSettings Settings = CurrentLVL.GetComponent<LvlSettings>();
			_image.SetSprite(Settings.QuestImage);
			_image.SetBackgroundColor(Settings.BackgroundColorForIntro);
			_image.SetImageIntoDefaultPosition();
			_image.SetImageIntoFinishPosition();
			_needToCheckForImage = true;
		}
	}
	private void ActivateScan()
	{
		LvlSettings Settings = CurrentLVL.GetComponent<LvlSettings>();
		if (Settings.LvlAnimator != null)
		{
			Settings.LvlAnimator.SetTrigger("StartAnimation");
		}
		if (_isBonusLvl)
		{
			_screenWrapModel.ActivateScaner(_lvlSettingsForBonusLvls[_lvlNum].LenghtOfAnimation);
		}
		else
		{
			_screenWrapModel.ActivateScaner(_lvlSettingsForBasicLvls[_lvlNum].LenghtOfAnimation);
		}
	}
	public void SaveLvl()
	{
		if (_needToSaveLvl)
		{
			if (_isBonusLvl)
			{
				_saveController.SaveCurrentBonusLvl();
			}
			else
			{
				_saveController.SaveCurrentLvl();
			}
			_needToSaveLvl = false;
		}
	}
	public void StartNextLVL()
	{
		_screenWrapModel.CleanScene();
		CleanScene();
		_uiController.DisActivateWinPanel();
		_isBonusLvl = _saveController.IsNextLvlBonus();
		if (_isBonusLvl)
		{
			_lvlNum = _saveController.GetBonusLvlNum();
		}
		else
		{
			_lvlNum = _saveController.GetNextLvlNum();
		}
		_needToSaveLvl = true;
		SpawnLvl();
	}
	public void RestartLVL()
	{
		_screenWrapModel.CleanScene();
		CleanScene();
		_uiController.DisActivateWinPanel();
		_lvlNum = _saveController.GetCurrentLvlNum();
		SpawnLvl();
	}
	private void CleanScene()
	{
		Destroy(CurrentLVL);
	}
	public Sprite GetQuestSprite()
	{
		LvlSettings Settings = CurrentLVL.GetComponent<LvlSettings>();
		return Settings.QuestImage;
	}
	public void ActivateWinUi(float Delay)
	{
		if (_isBonusLvl)
		{
			_uiController.ActivateBonusWinPanel(Delay);
		}
		else
		{
			_uiController.ActivateWinPanel(Delay);
		}
	}
}
