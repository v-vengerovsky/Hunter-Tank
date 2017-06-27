using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HunterTank
{
	public class PlayView : ViewBase
	{
		[SerializeField]
		private Text _scoreText;
		private int _score;

		public int Score
		{
			get
			{
				return _score;
			}

			set
			{
				_score = value;
				UpdateView();
			}
		}

		public void SetScore(int score)
		{
			Score = score;
		}

		private void Awake()
		{
			Score = 0;
		}

		private void UpdateView()
		{
			_scoreText.text = string.Format(Constants.ScoreFormat,_score);
		}
	}
}