using UnityEngine;
using UnityEngine.SceneManagement;

public class MakeAScreenshot : MonoBehaviour
{
	public GameObject SpriteObject;
	public Camera ScreenShotCamera;
	public int AmountOfDividing;
	public int AmountOfSimilarPixels;
	public bool NeedToTakeScreenshot;
	public bool NeedToTakeFirstScreenshot;
	public bool NeedToTakeSecondScreenshot;

	private GameObject MainObject;
	private GameObject Object;
	private MovingLine _movingLine;
	private Texture2D _firstScreenshot;
	private Texture2D _secondScreenshot;
	[SerializeField] private int ScreenNum = 0;
	private SpriteRenderer MainRender;
	private SpriteRenderer[] Renderers;
	private Texture2D _mainTexture;
	private byte[] byteArray;
	private bool ScreenshotIsMade = false;


	public float HeightOfGameScreen;
	public float WidthOfGameScreen;

	private void Awake()
	{
		float num = (ScreenShotCamera.transform.position.z * -1) * Mathf.Tan((ScreenShotCamera.fieldOfView / 2) * Mathf.Deg2Rad);
		HeightOfGameScreen = num * 2;


		Renderers = new SpriteRenderer[AmountOfDividing];
		for (int i = 0; i < AmountOfDividing; i++)
		{
			Object = Instantiate(SpriteObject, new Vector3(0, HeightOfGameScreen / 2f - (HeightOfGameScreen / AmountOfDividing * i), 0), Quaternion.identity);
			Renderers[i] = Object.GetComponentInChildren<SpriteRenderer>();
		}

		MainObject = Instantiate(SpriteObject, new Vector3(0, 5.6f, 0), Quaternion.identity);
		MainRender = MainObject.GetComponentInChildren<SpriteRenderer>();

		NeedToTakeFirstScreenshot = true;
		_movingLine = FindObjectOfType<MovingLine>();
	}
	private void Start()
	{
		WidthOfGameScreen = (float)Screen.width / (float)Screen.height * HeightOfGameScreen;
	}
	private void OnPostRender()
	{

		//старый способ
		if (NeedToTakeScreenshot)
		{
			NeedToTakeScreenshot = false;


			RenderTexture renderTexture = ScreenShotCamera.targetTexture;

			Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height / AmountOfDividing, TextureFormat.RGBA32, false);

			Rect rect = new Rect(0, HeightOfGameScreen * 100 / AmountOfDividing * ScreenNum, renderTexture.width, renderTexture.height / AmountOfDividing);
			renderResult.ReadPixels(rect, 0, 0, false);
			renderResult.Apply();

			//создает изображение из массива байтов

			Sprite tempSprite = Sprite.Create(renderResult, new Rect(0, 0, renderResult.width, renderResult.height), new Vector2(0.5f, 0));
			Renderers[ScreenNum].sprite = tempSprite;
			ScreenNum++;

			RenderTexture.ReleaseTemporary(renderTexture);
			ScreenShotCamera.targetTexture = null;
		}

		//новый способ
		/*if (NeedToTakeScreenshot)
		{
			NeedToTakeScreenshot = false;

			RenderTexture renderTexture = ScreenShotCamera.targetTexture;

			Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height / AmountOfDividing , TextureFormat.RGBA32, true);

			//Debug.Log(renderTexture.height / AmountOfDividing * (ScreenNum + 1));

			Rect rect = new Rect(0, 900f / AmountOfDividing * (ScreenNum ), renderTexture.width, renderTexture.height / AmountOfDividing * (ScreenNum + 1));
			renderResult.ReadPixels(rect, 0, 0, true);

			if (ScreenNum == 0)
			{
				byteArray = renderResult.EncodeToPNG();
			}
			else
			{
				byte[] tempByteArray = byteArray;
				byte[] tempByteArray2 = renderResult.EncodeToPNG();
				byteArray = new byte[tempByteArray.Length + tempByteArray2.Length];

				for (int i = 0; i < tempByteArray.Length; i++)
				{
					byteArray[i] = tempByteArray[i];
				}
				for (int i = tempByteArray.Length; i < tempByteArray.Length + tempByteArray2.Length; i++)
				{
					byteArray[i] = tempByteArray2[i - tempByteArray.Length];
				}
				byteArray = tempByteArray2;
			}


			//создает изображение из массива байтов
			Texture2D renderResult2 = new Texture2D(renderTexture.width, renderTexture.height / AmountOfDividing * (ScreenNum + 1), TextureFormat.RGBA32, false);
			Debug.Log("Image Height = " + renderResult2.height);
			renderResult2.LoadImage(byteArray);
			Debug.Log("Image Height after  = " + renderResult2.height);
			Sprite tempSprite = Sprite.Create(renderResult2, new Rect(0, 0, renderResult2.width, renderResult2.height), new Vector2(0.5f, 1f));
			MainRender.sprite = tempSprite;
			ScreenNum++;

			RenderTexture.ReleaseTemporary(renderTexture);
			ScreenShotCamera.targetTexture = null;
		}*/

		//через setPixels
		/*
		if (NeedToTakeScreenshot)
		{
			NeedToTakeScreenshot = false;

			RenderTexture renderTexture = ScreenShotCamera.targetTexture;

			Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height / AmountOfDividing, TextureFormat.RGBA32, true);

			Rect rect = new Rect(0, 900f / AmountOfDividing * ScreenNum, renderTexture.width, renderTexture.height / AmountOfDividing);
			renderResult.ReadPixels(rect, 0, 0, true);


			if (ScreenNum > 0)
			{
				Texture2D TempTexture = _mainTexture;
				_mainTexture = new Texture2D(TempTexture.width, TempTexture.height + renderResult.height);
				_mainTexture.SetPixels(0,0,renderResult.width,renderResult.height,renderResult.GetPixels());
				_mainTexture.SetPixels(0, renderResult.height, TempTexture.width, TempTexture.height, TempTexture.GetPixels());
				_mainTexture.Apply();
			}
			else
			{
				_mainTexture = renderResult;
				_mainTexture.Apply();
			}
			//создает изображение из массива байтов
			Sprite tempSprite = Sprite.Create(_mainTexture, new Rect(0, 0, _mainTexture.width, _mainTexture.height), new Vector2(0.5f, 1f));
			MainRender.sprite = tempSprite;
			ScreenNum++;

			RenderTexture.ReleaseTemporary(renderTexture);
			ScreenShotCamera.targetTexture = null;
		}*/



		/*
		if (NeedToTakeFirstScreenshot)
		{
			NeedToTakeFirstScreenshot = false;

			ScreenShotCamera.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
			RenderTexture renderTexture = ScreenShotCamera.targetTexture;

			_firstScreenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.R8, false);

			Rect rect = new Rect(0, 900f / AmountOfDividing * ScreenNum, renderTexture.width, renderTexture.height);
			_firstScreenshot.ReadPixels(rect, 0, 0, false);


			byte[] byteArray = _firstScreenshot.EncodeToPNG();

			//создает текстуру
			_firstScreenshot.LoadImage(byteArray);

			RenderTexture.ReleaseTemporary(renderTexture);
			ScreenShotCamera.targetTexture = null;
		}
		if (NeedToTakeSecondScreenshot)
		{
			NeedToTakeSecondScreenshot = false;

			ScreenShotCamera.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
			RenderTexture renderTexture = ScreenShotCamera.targetTexture;

			_secondScreenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.R8, false);

			Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
			_secondScreenshot.ReadPixels(rect, 0, 0, false);


			byte[] byteArray = _secondScreenshot.EncodeToPNG();

			//создает текстуру
			_secondScreenshot.LoadImage(byteArray);

			RenderTexture.ReleaseTemporary(renderTexture);
			ScreenShotCamera.targetTexture = null;
			CompareTwoTextures(_firstScreenshot, _secondScreenshot);
		}*/
	}

	private void CompareTwoTextures(Texture2D FirstTexture, Texture2D SecondTexture)
	{
		int Width = (int)FirstTexture.width;
		int Height = (int)FirstTexture.height;
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				if (FirstTexture.GetPixel(i, j) == SecondTexture.GetPixel(i, j))
				{
					AmountOfSimilarPixels++;
				}
			}
		}
		Debug.Log("Amount of Pixels: " + Width * Height + " , Amount of Similar Pixels: " + AmountOfSimilarPixels);
		Debug.Log("% : " + AmountOfSimilarPixels / (Width * Height / 100));
	}

	public void MakeScreenShot(int width, int height)
	{
		if (!ScreenshotIsMade)
		{
			if (ScreenNum < AmountOfDividing)
			{
				ScreenShotCamera.targetTexture = RenderTexture.GetTemporary(width, (int)(HeightOfGameScreen * 100), 32);
				NeedToTakeScreenshot = true;
				//Debug.Log("width: " + width + ", height: " + height);
			}
			else
			{
				ScreenshotIsMade = true;
				//NeedToTakeSecondScreenshot = true;
				_movingLine.StopMoveLine();
				Invoke("RestartGame", 2f);
			}
		}
	}
	private void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
