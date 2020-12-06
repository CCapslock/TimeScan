using UnityEngine;
using UnityEngine.UI;

public class QuestImageBehavior : MonoBehaviour
{
    public Canvas _canvas;
    public RectTransform ImageTransform;
    public Image QuestImage;
    public float StartScale;
    public float FinishScale;
    public float ScaleSpeed;
    public float MoveSpeed;
    public float DelayTime;

    public float FinishXPosition;
    public float FinishYPosition;

    public bool ImageInFinishPosition;

    private Vector3 _scalingVector;
    private Vector3 _movingVector;

    [SerializeField] private float _moveSpeedX;
    [SerializeField] private float _moveSpeedY;

    private bool _imageInXPosition;
    private bool _imageInYPosition;

    private bool _needToMove;
    private bool _needToScale;
    private bool _imageMoved;
    private bool _imageScaled;

    private void Start()
    {
        _movingVector = new Vector3();
        _scalingVector = new Vector3();
        SetImageIntoDefaultPosition();
        CalculateSpeed();
    }

    private void CalculateSpeed()
    {
        float XDiference = ImageTransform.anchoredPosition.x * -1f - FinishXPosition * -1f;
        float YDiference = ImageTransform.anchoredPosition.y * -1f - FinishYPosition * -1f;
        _moveSpeedY = MoveSpeed;
        _moveSpeedX = MoveSpeed * (XDiference / YDiference);
    }

    public void SetImageIntoFinishPosition()
    {
        Invoke("StartTransformImage", DelayTime);
    }
    public void SetImageIntoDefaultPosition() 
    {
        ImageTransform.anchorMax = new Vector2(1, 1);
        ImageTransform.anchorMin = new Vector2(1, 1);

        _movingVector.x = Screen.width / _canvas.scaleFactor * -0.5f;
        _movingVector.y = Screen.height / _canvas.scaleFactor * -0.5f;
        ImageTransform.anchoredPosition = _movingVector;

        _scalingVector.x = StartScale;
        _scalingVector.y = StartScale;
        ImageTransform.localScale = _scalingVector;

        _needToScale = false;
        _needToMove = false;
        _imageScaled = false;
        _imageMoved = false;

        _imageInXPosition = false;
        _imageInYPosition = false;

        ImageInFinishPosition = false;
    }

    private void StartTransformImage() 
    {
        ImageTransform.anchorMax = new Vector2(1, 1);
        ImageTransform.anchorMin = new Vector2(1, 1);
        _needToMove = true;
        _needToScale = true;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetImageIntoFinishPosition();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetImageIntoDefaultPosition();
        }
        if (_needToMove)
        {
            MoveImage();
        } 
        if (_needToScale)
        {
            ScaleImage();
        }
        if(_imageMoved && _imageScaled && !ImageInFinishPosition)
        {
            ImageInFinishPosition = true;
        }
    }
    private void MoveImage()
    {
        if (ImageTransform.anchoredPosition.x < FinishXPosition)
        {
            _movingVector = ImageTransform.anchoredPosition;
            _movingVector.x += _moveSpeedX;
            ImageTransform.anchoredPosition = _movingVector;
        }
        else
        {
            _imageInXPosition = true;
            _movingVector.x = FinishXPosition;
            ImageTransform.anchoredPosition = _movingVector;
        }
        if(ImageTransform.anchoredPosition.y < FinishYPosition)
        {
            _movingVector = ImageTransform.anchoredPosition;
            _movingVector.y += _moveSpeedY;
            ImageTransform.anchoredPosition = _movingVector;
        }
        else
        {
            _imageInYPosition = true; 
            _movingVector.y = FinishYPosition;
            ImageTransform.anchoredPosition = _movingVector;
        }
        if(_imageInYPosition && _imageInXPosition)
        {
            _needToMove = false;
            _imageMoved = true;
        }
    }
    private void ScaleImage()
    {
        if(ImageTransform.localScale.x > FinishScale)
        {
            _scalingVector = ImageTransform.localScale;
            _scalingVector.x -= ScaleSpeed;
            _scalingVector.y -= ScaleSpeed;
            ImageTransform.localScale = _scalingVector;
        }
        else
        {
            _needToScale = false;
            _imageScaled = true;
            _scalingVector.x = FinishScale;
            _scalingVector.y = FinishScale;
            ImageTransform.localScale = _scalingVector;
        }
    }
    public void SetSprite(Sprite QuestSprite)
    {
        QuestImage.sprite = QuestSprite;
    }
}
