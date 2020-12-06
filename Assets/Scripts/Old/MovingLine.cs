using UnityEngine;

public class MovingLine : MonoBehaviour
{
	public GameObject Line;
	public RectTransform UILine;
	public float Speed;
	public float UISpeed;
	private Canvas _canvas;
	private MakeAScreenshot _screenshoter;
	private Vector3 _movingVector;
	[SerializeField] private float _numForChecking;
	private float _startCheckingNum;
	[SerializeField] private bool _needToMoveLine;

	private void Start()
	{
		_canvas = FindObjectOfType<Canvas>();
		_screenshoter = FindObjectOfType<MakeAScreenshot>();
		Line.transform.position = new Vector3(0, _screenshoter.HeightOfGameScreen / 2f, 0);
		float yrik = Screen.height / _canvas.scaleFactor / 2;
		UILine.anchoredPosition = new Vector3(0, yrik, 0); 
		_numForChecking = _screenshoter.HeightOfGameScreen  / _screenshoter.AmountOfDividing;
		_startCheckingNum = _screenshoter.HeightOfGameScreen / 2f;
		StartMoveLine();
		UISpeed = Speed * (Screen.height / _canvas.scaleFactor / _screenshoter.HeightOfGameScreen);

	}

	private void FixedUpdate()
	{
		if (_needToMoveLine)
		{
			_movingVector = Line.transform.position;
			_movingVector.y -= Speed;
			Line.transform.position = _movingVector;


			if (_movingVector.y <= _startCheckingNum - _numForChecking)
			{
				_startCheckingNum -= _numForChecking;
				_screenshoter.MakeScreenShot(Screen.width, Screen.height);
			}

			_movingVector = UILine.anchoredPosition;
			_movingVector.y -= UISpeed;
			UILine.anchoredPosition = _movingVector;
		}
	}
	public void StartMoveLine()
	{
		_needToMoveLine = true;
	}
	public void StopMoveLine()
	{
		_needToMoveLine = false;
	}
}
