using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreGame2 : MonoBehaviour
{
    public float _score;
    public float smoothSpeed = 5f; // smooth

    [SerializeField] private Slider _slider;

    private PlayerGame2 _game;

    //public TMP_Text _scoreText;

    private void Start()
    {
        Time.timeScale = 1f;
        _game = GetComponent<PlayerGame2>();
    }

    private void Update()
    {
        Score();

        //_scoreText.text = _score.ToString(); // Afficher le score entier
    }

    void Score()
    {
        _slider.maxValue = _game._sequenceNumber;

        // Interpolation de la valeur actuelle du slider vers le score cible
        _slider.value = Mathf.Lerp(_slider.value, _score, smoothSpeed * Time.deltaTime);
    }
}
