using UnityEngine;

public class ScreenWrapModel : MonoBehaviour
{
	public GameObject SpriteObject;
	public Camera ScreenShotCamera;
	public RectTransform UIHorizontalLine;
	public RectTransform UIVerticalLine;
	public float DelayTimeBeforeUI;
	public float HorizontalSpeed;
	public float VerticalSpeed;
	public int AmountOfDividingIfHorizontal;
	public int AmountOfDividingIfVertical;
	public int CombiningTimes;
	public bool NeedToMakeQuestScreenshot;

	private ScanDirection Direction;

	private UiController _uiController;
	private SpriteRenderer[] Renderers;
	private SpriteRenderer[] UpToDownRenderers;
	private SpriteRenderer[] LeftToRightRenderers;
	private SpriteRenderer[] RightToLeftRenderers;
	private GameObject Object;
	private int ScreenNum = 0;
	private bool ScreenshotIsMade = false;
	private int _amountOfDividing;
	private int _combiningCounter = 1;
	private Rect _rect;
	private Canvas _canvas;
	private Vector3 _movingVector;
	private Vector2 _pivot;
	private float _speed;
	private float HeightOfGameScreen;
	private float WidthOfGameScreen;
	private float _uiSpeed;
	private float _lenghtOfScreenPart;
	private float _startCheckingNum;
	private float _checkingNum;
	private int _combineNum;
	private int _combineCurrentNum;
	private bool _needToScan;
	private bool _needToTakeScreenshot;
	private bool _needToTakeCombiningScreenshot;
	private bool _needToTakeQuestScreenshot;

	private void Awake()
	{
		_uiController = FindObjectOfType<UiController>();
	}

	public void CreateRenderes()
	{
		float num = (ScreenShotCamera.transform.position.z * -1) * Mathf.Tan((ScreenShotCamera.fieldOfView / 2) * Mathf.Deg2Rad);//расчитывает длину от 0 до верхнего края экрана в юнитах
		HeightOfGameScreen = num * 2;//длина игрового экрана в юнитах 
		WidthOfGameScreen = (float)Screen.width / (float)Screen.height * HeightOfGameScreen;//ширина игрового экрана в юнитах
		//Debug.Log("WidthOfGameScreen = " + WidthOfGameScreen);
		//создает на сцене пустые спрайты использую префаб
		UpToDownRenderers = new SpriteRenderer[AmountOfDividingIfHorizontal];
		for (int i = 0; i < AmountOfDividingIfHorizontal; i++)
		{
			Object = Instantiate(SpriteObject, new Vector3(0, HeightOfGameScreen / 2f - (HeightOfGameScreen / AmountOfDividingIfHorizontal * i), 0), Quaternion.identity);
			UpToDownRenderers[i] = Object.GetComponentInChildren<SpriteRenderer>();
		}

		LeftToRightRenderers = new SpriteRenderer[AmountOfDividingIfVertical];
		for (int i = 0; i < AmountOfDividingIfVertical; i++)
		{
			Object = Instantiate(SpriteObject, new Vector3(WidthOfGameScreen * -0.5f + (WidthOfGameScreen / AmountOfDividingIfVertical * i), 0, 0), Quaternion.identity);
			LeftToRightRenderers[i] = Object.GetComponentInChildren<SpriteRenderer>();
		}

		RightToLeftRenderers = new SpriteRenderer[AmountOfDividingIfVertical];
		for (int i = 0; i < AmountOfDividingIfVertical; i++)
		{
			Object = Instantiate(SpriteObject, new Vector3(WidthOfGameScreen * 0.5f - (WidthOfGameScreen / AmountOfDividingIfVertical * i), 0, 0), Quaternion.identity);
			RightToLeftRenderers[i] = Object.GetComponentInChildren<SpriteRenderer>();
		}
	}

	public void PrepareScan(ScanDirection direction)
	{
		Direction = direction;
		if (Direction == ScanDirection.FromUpToDown)
		{
			_amountOfDividing = AmountOfDividingIfHorizontal;
			_speed = HorizontalSpeed;
		}
		else if (Direction == ScanDirection.FromLeftToRight || Direction == ScanDirection.FromRightToLeft)
		{
			_amountOfDividing = AmountOfDividingIfVertical;
			_speed = VerticalSpeed;
		}

		float num = (ScreenShotCamera.transform.position.z * -1) * Mathf.Tan((ScreenShotCamera.fieldOfView / 2) * Mathf.Deg2Rad);//расчитывает длину от 0 до верхнего края экрана в юнитах
		HeightOfGameScreen = num * 2;//длина игрового экрана в юнитах 
		WidthOfGameScreen = (float)Screen.width / (float)Screen.height * HeightOfGameScreen;//ширина игрового экрана в юнитах
		if (Direction == ScanDirection.FromUpToDown)
		{
			Renderers = UpToDownRenderers;

			_startCheckingNum = HeightOfGameScreen / 2f;//точка от которой начинает движение линия
			_checkingNum = _startCheckingNum;
			_lenghtOfScreenPart = HeightOfGameScreen / _amountOfDividing;

			//находит канвас, ставит спрайт скана куда нужно и высчитывает скорость перемешения скана
			_canvas = FindObjectOfType<Canvas>();
			UIVerticalLine.gameObject.SetActive(false);
			UIHorizontalLine.gameObject.SetActive(true);
			UIHorizontalLine.anchoredPosition = new Vector3(0, Screen.height / _canvas.scaleFactor / 2, 0);
			_uiSpeed = _speed * (Screen.height / _canvas.scaleFactor / HeightOfGameScreen);
		}
		else if (Direction == ScanDirection.FromLeftToRight)
		{
			Renderers = LeftToRightRenderers;

			_startCheckingNum = HeightOfGameScreen * -0.5f;//точка от которой начинает движение линия
			_checkingNum = _startCheckingNum;
			_lenghtOfScreenPart = WidthOfGameScreen / _amountOfDividing;

			//находит канвас, ставит спрайт скана куда нужно и высчитывает скорость перемешения скана
			_canvas = FindObjectOfType<Canvas>();
			UIHorizontalLine.gameObject.SetActive(false);
			UIVerticalLine.gameObject.SetActive(true);
			UIVerticalLine.anchoredPosition = new Vector3(Screen.width / _canvas.scaleFactor * -0.5f, 0, 0);
			_uiSpeed = _speed * (Screen.width / _canvas.scaleFactor / WidthOfGameScreen);
		}
		else if (Direction == ScanDirection.FromRightToLeft)
		{
			Renderers = RightToLeftRenderers;

			_startCheckingNum = HeightOfGameScreen * 0.5f;//точка от которой начинает движение линия
			_checkingNum = _startCheckingNum;
			_lenghtOfScreenPart = WidthOfGameScreen / _amountOfDividing;

			//находит канвас, ставит спрайт скана куда нужно и высчитывает скорость перемешения скана
			_canvas = FindObjectOfType<Canvas>();
			UIHorizontalLine.gameObject.SetActive(false);
			UIVerticalLine.gameObject.SetActive(true);
			UIVerticalLine.anchoredPosition = new Vector3(Screen.width / _canvas.scaleFactor * 0.5f, 0, 0);
			_uiSpeed = _speed * (Screen.width / _canvas.scaleFactor / WidthOfGameScreen);
		}

		_combineNum = _amountOfDividing / CombiningTimes;
		_combineCurrentNum = 0;
		_pivot = new Vector2(0, 0);
		ScreenshotIsMade = false;
		ScreenNum = 0;
		_combiningCounter = 1;
	}
	public void CleanScene()
	{
		for (int i = 0; i < Renderers.Length; i++)
		{
			Renderers[i].sprite = null;
		}
		ScreenShotCamera.targetTexture = null;
	}
	public void ActivateScaner(float AnimationDelay)
	{
		Invoke("StartScanning", AnimationDelay);
	}
	public void StartScanning()
	{
		_needToScan = true;
	}

	private void FixedUpdate()
	{
		//считает перемещение линии и решает стоит ли делать скрин
		if (_needToScan)
		{
			if (Direction == ScanDirection.FromUpToDown)
			{
				_checkingNum -= _speed;

				_movingVector = UIHorizontalLine.anchoredPosition;
				_movingVector.y -= _uiSpeed;
				UIHorizontalLine.anchoredPosition = _movingVector;//двигает UI спрайт линии скана

				if (_checkingNum <= _startCheckingNum - _lenghtOfScreenPart)
				{
					MakeScreenshot();
					_startCheckingNum -= _lenghtOfScreenPart;
				}

			}
			else if (Direction == ScanDirection.FromLeftToRight)
			{
				_checkingNum += _speed;

				_movingVector = UIVerticalLine.anchoredPosition;
				_movingVector.x += _uiSpeed;
				UIVerticalLine.anchoredPosition = _movingVector;//двигает UI спрайт линии скана
				if (_checkingNum >= _startCheckingNum + _lenghtOfScreenPart)
				{
					MakeScreenshot();
					_startCheckingNum += _lenghtOfScreenPart;
				}
			}
			else if (Direction == ScanDirection.FromRightToLeft)
			{
				_checkingNum -= _speed;

				_movingVector = UIVerticalLine.anchoredPosition;
				_movingVector.x -= _uiSpeed;
				UIVerticalLine.anchoredPosition = _movingVector;//двигает UI спрайт линии скана
				if (_checkingNum <= _startCheckingNum - _lenghtOfScreenPart)
				{
					MakeScreenshot();
					_startCheckingNum -= _lenghtOfScreenPart;
				}
			}

			if (_combineCurrentNum + _combineNum == ScreenNum)
			{
				CombineTextures();
			}
		}
	}
	public void MakeScreenshot()
	{
		if (!ScreenshotIsMade)
		{
			if (ScreenNum < _amountOfDividing - 1)
			{
				ScreenShotCamera.targetTexture = RenderTexture.GetTemporary((int)(WidthOfGameScreen * 100), (int)(HeightOfGameScreen * 100), 24);
				_needToTakeScreenshot = true;
			}
			else
			{
				if (NeedToMakeQuestScreenshot)
				{
					Invoke("MakeQuestScreenshot", 0.5f);
				}
				ScreenshotIsMade = true;
				_needToScan = false;
				_uiController.ActivateWinPanel(DelayTimeBeforeUI);
			}
		}
	}
	public void MakeQuestScreenshot()
	{
		ScreenShotCamera.targetTexture = RenderTexture.GetTemporary((int)(WidthOfGameScreen * 100), (int)(HeightOfGameScreen * 100), 24);
		_needToTakeQuestScreenshot = true;
	}
	private void CombineTextures()
	{
		ScreenShotCamera.targetTexture = RenderTexture.GetTemporary((int)(WidthOfGameScreen * 100), (int)(HeightOfGameScreen * 100), 24);
		_needToTakeCombiningScreenshot = true;
	}

	//TODO убрать копипасту
	private void OnPostRender()
	{
		//вот тут происходит волшебство
		if (_needToTakeScreenshot)
		{
			//создает маленькие скриншоты
			RenderTexture renderTexture = ScreenShotCamera.targetTexture;
			_needToTakeScreenshot = false;
			if (Direction == ScanDirection.FromUpToDown)
			{
				Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height / _amountOfDividing, TextureFormat.RGBA32, false);
				_rect = new Rect(0, HeightOfGameScreen * 100 / _amountOfDividing * ScreenNum, renderTexture.width, renderTexture.height / _amountOfDividing);
				renderResult.ReadPixels(_rect, 0, 0, false);
				renderResult.Apply();
				Sprite tempSprite = Sprite.Create(renderResult, new Rect(0, 0, renderResult.width, renderResult.height), new Vector2(0.5f, 1));
				tempSprite.texture.wrapMode = TextureWrapMode.Clamp;
				Renderers[ScreenNum].sprite = tempSprite;
			}

			else if (Direction == ScanDirection.FromLeftToRight)
			{
				Texture2D renderResult = new Texture2D(renderTexture.width / _amountOfDividing + 1, renderTexture.height, TextureFormat.RGBA32, false);
				_rect = new Rect(WidthOfGameScreen * 100f / _amountOfDividing * ScreenNum, 0, renderTexture.width / _amountOfDividing + 1, renderTexture.height);
				renderResult.ReadPixels(_rect, 0, 0, false);
				renderResult.Apply();
				Sprite tempSprite = Sprite.Create(renderResult, new Rect(0, 0, renderResult.width, renderResult.height), new Vector2(0, 0.5f));
				tempSprite.texture.wrapMode = TextureWrapMode.Clamp;
				Renderers[ScreenNum].sprite = tempSprite;
			}
			else if (Direction == ScanDirection.FromRightToLeft)
			{
				Texture2D renderResult = new Texture2D(renderTexture.width / _amountOfDividing + 1, renderTexture.height, TextureFormat.RGBA32, false);
				if (WidthOfGameScreen * 100f - (WidthOfGameScreen * 100f / _amountOfDividing * ScreenNum) + renderTexture.width / _amountOfDividing + 1 > WidthOfGameScreen * 100f)
				{
					ScreenNum++;
					RenderTexture.ReleaseTemporary(renderTexture);
				}
				else
				{
					_rect = new Rect(WidthOfGameScreen * 100f - (WidthOfGameScreen * 100f / _amountOfDividing * ScreenNum), 0, renderTexture.width / _amountOfDividing + 1, renderTexture.height);
					renderResult.ReadPixels(_rect, 0, 0, false);
					renderResult.Apply();
					Sprite tempSprite = Sprite.Create(renderResult, new Rect(0, 0, renderResult.width, renderResult.height), new Vector2(0, 0.5f));
					tempSprite.texture.wrapMode = TextureWrapMode.Clamp;
					Renderers[ScreenNum].sprite = tempSprite;
				}
			}

			ScreenNum++;
			RenderTexture.ReleaseTemporary(renderTexture);
			if (!_needToTakeCombiningScreenshot)
			{
				ScreenShotCamera.targetTexture = null;
			}
		}

		//комбинирует несколько скринов в одно
		if (_needToTakeCombiningScreenshot)
		{
			_needToTakeCombiningScreenshot = false;
			RenderTexture renderTexture2 = ScreenShotCamera.targetTexture;
			Texture2D renderResult = new Texture2D(0, 0, TextureFormat.RGBA32, true);

			if (Direction == ScanDirection.FromUpToDown)
			{
				renderResult = new Texture2D(renderTexture2.width, renderTexture2.height / _amountOfDividing * _combineNum, TextureFormat.RGBA32, true);
				_rect = new Rect(0, HeightOfGameScreen * 100 / _amountOfDividing * _combineCurrentNum, renderTexture2.width, renderTexture2.height / _amountOfDividing * _combineNum);
				_pivot.x = 0.5f;
				_pivot.y = 1f;
			}
			else if (Direction == ScanDirection.FromLeftToRight)
			{
				renderResult = new Texture2D(renderTexture2.width / CombiningTimes + 1, renderTexture2.height, TextureFormat.RGBA32, true);
				_rect = new Rect(WidthOfGameScreen * 100f / _amountOfDividing * _combineCurrentNum, 0, renderTexture2.width / CombiningTimes + 1, renderTexture2.height);
				_pivot.x = 0f;
				_pivot.y = 0.5f;
			}
			else if (Direction == ScanDirection.FromRightToLeft)
			{
				renderResult = new Texture2D(renderTexture2.width / CombiningTimes + 1, renderTexture2.height, TextureFormat.RGBA32, true);
				_rect = new Rect(WidthOfGameScreen * 100f / CombiningTimes * (CombiningTimes - _combiningCounter) - 1, 0, renderTexture2.width / CombiningTimes + 1, renderTexture2.height);
				if (_combiningCounter == 1)
				{
					renderResult = new Texture2D(renderTexture2.width / CombiningTimes + 1, renderTexture2.height, TextureFormat.RGBA32, true);
					_rect = new Rect(WidthOfGameScreen * 100f / CombiningTimes * (CombiningTimes - _combiningCounter) - 1, 0, renderTexture2.width / CombiningTimes + 1, renderTexture2.height);
				}
				_pivot.x = 1f;
				_pivot.y = 0.5f;
			}

			renderResult.ReadPixels(_rect, 0, 0, true);
			renderResult.Apply();
			Sprite tempSprite = Sprite.Create(renderResult, new Rect(0, 0, renderResult.width, renderResult.height), _pivot);
			tempSprite.texture.wrapMode = TextureWrapMode.Clamp;//убирает последние пиксели конца текстуры из начала текстуры
			Renderers[_combineCurrentNum].sprite = tempSprite;
			RenderTexture.ReleaseTemporary(renderTexture2);
			ScreenShotCamera.targetTexture = null;
			for (int i = 1; i <= _combineNum - 5; i++)
			{
				Renderers[i + _combineCurrentNum].sprite = null;
			}
			_combineCurrentNum += _combineNum;
			_combiningCounter++;
		}

		if (_needToTakeQuestScreenshot)
		{
			//делает скриншот того что получилось и сохранет текстуру(для создания заданий)
			_needToTakeQuestScreenshot = false;
			RenderTexture renderTexture2 = ScreenShotCamera.targetTexture;

			Texture2D renderResult = new Texture2D(renderTexture2.width, renderTexture2.height, TextureFormat.RGBA32, true);
			_rect = new Rect(0, 0, renderTexture2.width, renderTexture2.height);
			renderResult.ReadPixels(_rect, 0, 0, true);

			byte[] byteArray = renderResult.EncodeToPNG();
			System.IO.File.WriteAllBytes(Application.dataPath + "/QuestScreenshot.png", byteArray);

			RenderTexture.ReleaseTemporary(renderTexture2);
			ScreenShotCamera.targetTexture = null;
		}
	}
}
