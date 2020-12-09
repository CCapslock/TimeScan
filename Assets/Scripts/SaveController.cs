using UnityEngine;

public class SaveController : MonoBehaviour
{
    private MainGameController _gameController;
    private string _roundForPlaying = "LvlNum";
    private string _allRoundsForPlaying = "LvlBase";
    private bool _isRandom;
    private void Awake()
    {
        _gameController = FindObjectOfType<MainGameController>();
        if(PlayerPrefs.GetString(_allRoundsForPlaying).Length != _gameController.LvlPresets.Length)
        {
            char[] lvlBaseNumbers = new char[_gameController.LvlPresets.Length];
            for (int i = 0; i < lvlBaseNumbers.Length; i++)
            {

            }
        }
    }

    public int GetLvlNum()
    {
        return PlayerPrefs.GetInt(_roundForPlaying);
    }
    public void IncreaseLvlNum()
    {
        PlayerPrefs.SetInt(_roundForPlaying, PlayerPrefs.GetInt(_roundForPlaying) + 1);
    }
    public void DecreaseLvlNum()
    {
        PlayerPrefs.SetInt(_roundForPlaying, PlayerPrefs.GetInt(_roundForPlaying) - 1);
    }
    public void SetLvlNum(int num)
    {
        PlayerPrefs.SetInt(_roundForPlaying, num);
    }
    public void ZeroizeLvlNum()
    {
        PlayerPrefs.SetInt(_roundForPlaying, 0);
    }
}
