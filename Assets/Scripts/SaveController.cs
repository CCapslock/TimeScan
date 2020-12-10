using System;
using UnityEngine;

public class SaveController : MonoBehaviour
{
	public bool IsRandom;

	private MainGameController _gameController;
	private string _roundForPlaying = "LvlNum";
	private string _allRoundsForPlaying = "LvlBase";
	private string _notBonusLvlsCounter = "BasicLvls";
	private string _bonusLvlsCounter = "BonusLvls";
	private char[] _currentLvlBase;
	private int _currentLvlNum;
	private void Awake()
	{
		_gameController = FindObjectOfType<MainGameController>();
		if (PlayerPrefs.GetString(_allRoundsForPlaying).Length != _gameController.LvlPresets.Length)
		{
			char[] lvlBaseNumbers = new char[_gameController.LvlPresets.Length];
			for (int i = 0; i < lvlBaseNumbers.Length; i++)
			{
				lvlBaseNumbers[i] = Convert.ToChar("0");
			}
			string LVLBase = new string(lvlBaseNumbers);
			PlayerPrefs.SetString(_allRoundsForPlaying, LVLBase);
			PlayerPrefs.SetInt(_roundForPlaying, 0);
		}
		if (PlayerPrefs.GetInt(_roundForPlaying) != _gameController.LvlPresets.Length)
		{
			IsRandom = false;
		}
		else
		{
			IsRandom = true;
		}
	}

	public int GetNextLvlNum()
	{
		if (!IsRandom)
		{
			if (PlayerPrefs.GetInt(_roundForPlaying) != _gameController.LvlPresets.Length)
			{
				return PlayerPrefs.GetInt(_roundForPlaying);
			}
			else
			{
				IsRandom = true;
				return GetRandomLvlNum();
			}
		}
		else
		{
			return GetRandomLvlNum();
		}
	}
	private int GetRandomLvlNum()
	{
		_currentLvlBase = PlayerPrefs.GetString(_allRoundsForPlaying).ToCharArray();
		bool AllLvlsPlayed = true;
		for (int i = 0; i < _currentLvlBase.Length; i++)
		{
			if (_currentLvlBase[i] == Convert.ToChar("0"))
			{
				AllLvlsPlayed = false;
			}
		}
		if (AllLvlsPlayed)
		{
			char[] lvlBaseNumbers = new char[_gameController.LvlPresets.Length];
			for (int i = 0; i < lvlBaseNumbers.Length; i++)
			{
				lvlBaseNumbers[i] = Convert.ToChar("0");
			}
			string LVLBase = new string(lvlBaseNumbers);
			PlayerPrefs.SetString(_allRoundsForPlaying, LVLBase);
		}
		_currentLvlBase = PlayerPrefs.GetString(_allRoundsForPlaying).ToCharArray();
		for (int i = 0; i < 10000; i++)
		{
			int num = UnityEngine.Random.Range(0, _currentLvlBase.Length);
			if (_currentLvlBase[num] == Convert.ToChar("0"))
			{
				_currentLvlNum = num;
				return num;
			}
		}
		_currentLvlNum = 0;
		return 0;
	}
	public int GetCurrentLvlNum()
	{
		return _currentLvlNum;
	}
	public void SaveCurrentLvl()
	{
		PlayerPrefs.SetInt(_notBonusLvlsCounter, PlayerPrefs.GetInt(_notBonusLvlsCounter) + 1);
		if (IsRandom)
		{
			_currentLvlBase[_currentLvlNum] = Convert.ToChar("1");
			string LVLBase = new string(_currentLvlBase);
			PlayerPrefs.SetString(_allRoundsForPlaying, LVLBase);
		}
		else
		{
			PlayerPrefs.SetInt(_roundForPlaying, PlayerPrefs.GetInt(_roundForPlaying) + 1);
		}
	}
	public void SaveCurrentBonusLvl()
	{
		PlayerPrefs.SetInt(_notBonusLvlsCounter, 0);
		if(PlayerPrefs.GetInt(_bonusLvlsCounter) == _gameController.BonusLvlPresets.Length - 1)
		{
			PlayerPrefs.SetInt(_bonusLvlsCounter, 0);
		}
		else
		{
			PlayerPrefs.SetInt(_bonusLvlsCounter, PlayerPrefs.GetInt(_bonusLvlsCounter) + 1);
		}
		
	}
	public bool IsNextLvlBonus()
	{
		if (PlayerPrefs.GetInt(_notBonusLvlsCounter) == 3)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public int GetBonusLvlNum()
	{
		Debug.Log("BonusLvlNum = " + PlayerPrefs.GetInt(_bonusLvlsCounter));
		return PlayerPrefs.GetInt(_bonusLvlsCounter);
	}
}
