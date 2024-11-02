using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGame2 : MonoBehaviour
{
    public float _score;

    [SerializeField] private Slider _slider;

    private PlayerGame2 _game;

    private void Start()
    {
        _game = GetComponent<PlayerGame2>();
    }

    private void Update()
    {
        Score();
    }
    void Score()
    {
        _slider.maxValue = _game._sequenceNumber;
        _slider.value = _score;
    }
}
