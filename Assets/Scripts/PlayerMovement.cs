using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float YDelay;

	[SerializeField] private InputController _inputController;
	[SerializeField] private Transform _playerTransform;
	[SerializeField] private Vector3 _delay;
	[SerializeField] private Vector3 _startPosition;
	[SerializeField] private Vector3 _screenWall;
	[SerializeField] private Vector2 _minScreenPosition, _maxScreenPosition;
	[SerializeField] private bool _delayCounted;

	private void Start()
	{
		_playerTransform = GetComponent<Transform>();
		_inputController = FindObjectOfType<InputController>();
		_delay = new Vector3(0, YDelay);
		_minScreenPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)) * 3f;
		_maxScreenPosition = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) * 3f;
		_screenWall = new Vector3(Mathf.Clamp(transform.position.x, _minScreenPosition.x, _maxScreenPosition.x), Mathf.Clamp(transform.position.y, _minScreenPosition.y, _maxScreenPosition.y), transform.position.z);
	}
	//передвигает обьект в зависимости от положения пальца
	private void Update()
	{
		if (_inputController.DragingStarted)
		{
			if (!_delayCounted)
			{
				_delay = _inputController.TouchPosition;
				_startPosition = _playerTransform.position;
				_delayCounted = true;
			}
			_playerTransform.position = _startPosition + _inputController.TouchPosition - _delay;
		}
		else
		{
			_delayCounted = false;
		}
		//ограничение экрана
		_screenWall.x = Mathf.Clamp(transform.position.x, _minScreenPosition.x, _maxScreenPosition.x);
		_screenWall.y = Mathf.Clamp(transform.position.y, _minScreenPosition.y, _maxScreenPosition.y);
		transform.position = _screenWall;
	}
}
