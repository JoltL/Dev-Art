using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManagerGame22 : MonoBehaviour
{
    public float _score;

    [SerializeField] private Slider _slider;

    private PlayerGame2 _game;

    public TMP_Text _scoreText;

    private void Start()
    {
        _game = GetComponent<PlayerGame2>();

    }

    private void Update()
    {
        Score();

        _scoreText.text = _score.ToString();
           
    }
    void Score()
    {
        _slider.maxValue = _game._sequenceNumber;
        _slider.value = _score;
    }
}
