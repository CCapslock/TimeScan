using UnityEngine;

public class MainGameController : MonoBehaviour
{
	public GameObject[] LvlPresets;

	private GameObject CurrentLVL;
	private LvlSettings[] _lvlSettings;

	private QuestImageBehavior _image;
	private ScreenWrapModel _screenWrapModel;
	private UiController _uiController;
	private SaveController _saveController;
	private int _lvlNum;
	private bool _needToCheckForImage;

	private void Awake()
	{
		_screenWrapModel = FindObjectOfType<ScreenWrapModel>();
		_uiController = FindObjectOfType<UiController>(); 
		_saveController = FindObjectOfType<SaveController>();
		 _image = FindObjectOfType<QuestImageBehavior>();

		_lvlSettings = new LvlSettings[LvlPresets.Length];
		for (int i = 0; i < _lvlSettings.Length; i++)
		{
			_lvlSettings[i] = LvlPresets[i].GetComponent<LvlSettings>();
		}
		_screenWrapModel.CreateRenderes();
	}
	private void Start()
	{
		_lvlNum = _saveController.GetLvlNum();
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
		_screenWrapModel.PrepareScan(_lvlSettings[_lvlNum].Direction);
		CurrentLVL = Instantiate(LvlPresets[_lvlNum], new Vector3(0, 0, 0), Quaternion.identity);
		LvlSettings Settings = CurrentLVL.GetComponent<LvlSettings>();
		_image.SetSprite(Settings.QuestImage);
		_image.SetBackgroundColor(Settings.BackgroundColorForIntro);
		_image.SetImageIntoDefaultPosition();
		_image.SetImageIntoFinishPosition();
		_needToCheckForImage = true;
	}
	private void ActivateScan()
	{
		LvlSettings Settings = CurrentLVL.GetComponent<LvlSettings>();
		if (Settings.LvlAnimator != null)
		{
			Settings.LvlAnimator.SetTrigger("StartAnimation");
		}
		_screenWrapModel.ActivateScaner(_lvlSettings[_lvlNum].LenghtOfAnimation);
	}
	public void CalculateLvl()
	{
		if (_lvlNum + 1 <= LvlPresets.Length - 1)
		{
			_saveController.IncreaseLvlNum();
			_lvlNum = _saveController.GetLvlNum();
		}
		else
		{
			_saveController.ZeroizeLvlNum();
			_lvlNum = _saveController.GetLvlNum();
		}
	}
	public void StartNextLVL()
	{
		_screenWrapModel.CleanScene();
		CleanScene();
		_uiController.DisActivateWinPanel();
		SpawnLvl();
	}
	public void RestartLVL()
	{
		_screenWrapModel.CleanScene();
		CleanScene();
		_uiController.DisActivateWinPanel();
		if (_lvlNum != 0)
		{
			_saveController.DecreaseLvlNum();
			_lvlNum = _saveController.GetLvlNum();
		}
		else
		{
			_saveController.SetLvlNum(LvlPresets.Length - 1);
			_lvlNum = _saveController.GetLvlNum();
		}
		SpawnLvl();
		//CurrentLVL = Instantiate(LvlPresets[_lvlNum], new Vector3(0, 0, 0), Quaternion.identity);
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
}
