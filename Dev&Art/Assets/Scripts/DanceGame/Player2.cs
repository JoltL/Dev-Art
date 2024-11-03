using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{
    [Header("RandomArrows")]
    // Ajouter les deux types d'entrées pour chaque direction
    private KeyCode[][] _allDirections =
    {
        new KeyCode[] { KeyCode.UpArrow, KeyCode.JoystickButton3 },    // Haut
        new KeyCode[] { KeyCode.DownArrow, KeyCode.JoystickButton0 },  // Bas
        new KeyCode[] { KeyCode.LeftArrow, KeyCode.JoystickButton2 },  // Gauche
        new KeyCode[] { KeyCode.RightArrow, KeyCode.JoystickButton1 }  // Droite
    };

    public int _sequenceNumber = 10; // Difficulté Nombre de flèches
    private List<KeyCode[]> _inputSequence = new List<KeyCode[]>();
    private int _currentIndex = 0;

    [Header("Instantiate")]
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _placement;
    [SerializeField] private int _gapInBetween = 100;

    [SerializeField] private Sprite[] _arrowSprite; // Ref aux sprites des 4 flèches
    private List<GameObject> _instanciatedArrows = new List<GameObject>();

    [Header("Animate")]
    [SerializeField] private Animator _animator;

    [SerializeField] private ScoreGame2 _scoreGame2;

    [SerializeField] private GameObject[] _popmessage;

    [SerializeField] private GameObject _endPanel;

    [SerializeField] private Sprite[] _imagedance;

    public EventSystem _eventSystem;

    public GameObject _endButton;

    private bool _canPlay = true;

    private void Start()
    {
        _scoreGame2 = GetComponent<ScoreGame2>();
        RandomArrowsSequence();
    }

    private void Update()
    {
        if(_canPlay)
        {

        _scoreGame2._score = Mathf.Clamp(_scoreGame2._score, 0, _sequenceNumber);
        if (Input.anyKeyDown)
        {
            KeyCode pressedKey = GetPressedKey();
            if (pressedKey != KeyCode.None)
            {
                if (IsCorrectKey(pressedKey, _inputSequence[_currentIndex]))
                {
                    TriggerAnimation(pressedKey);
                    _currentIndex++;
                    Destroy(_instanciatedArrows[_currentIndex - 1]);
                    _scoreGame2._score++;
                    _popmessage[0].SetActive(true);
                    _popmessage[1].SetActive(false);

                    if (_currentIndex >= _inputSequence.Count)
                    {
                        //END//
                        _currentIndex = 0;
                        _instanciatedArrows.Clear();
                            _canPlay = false;
                        //RandomArrowsSequence();

                        _eventSystem.SetSelectedGameObject(_endButton);
                        _endPanel.SetActive(true);
                    }
                }
                else
                {
                    PlayAnimationByName("Fail");
                    _scoreGame2._score--;
                    _popmessage[0].SetActive(false);
                    _popmessage[1].SetActive(true);
                }
            }
        }
        }
    }

    void RandomArrowsSequence()
    {
        _inputSequence.Clear();
        float totalWidth = (_sequenceNumber - 1) * _gapInBetween;

        for (int i = 0; i < _sequenceNumber; i++)
        {
            KeyCode[] randomKeys = _allDirections[Random.Range(0, _allDirections.Length)];
            _inputSequence.Add(randomKeys);

            GameObject arrows = Instantiate(_arrowPrefab, _placement);
            RectTransform rectTransform = arrows.GetComponent<RectTransform>();

            float xOffset = i * _gapInBetween - totalWidth;
            rectTransform.anchoredPosition = new Vector2(xOffset, 0);

            _instanciatedArrows.Add(arrows);

            Image arrowImage = arrows.GetComponent<Image>();
            arrowImage.sprite = GetSpriteForKey(randomKeys[0]); // Utiliser le sprite associé à la direction
        }
    }

    KeyCode GetPressedKey()
    {
        foreach (KeyCode[] keys in _allDirections)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKeyDown(key))
                {
                    return key;
                }
            }
        }
        return KeyCode.None;
    }

    bool IsCorrectKey(KeyCode pressedKey, KeyCode[] correctKeys)
    {
        foreach (KeyCode key in correctKeys)
        {
            if (pressedKey == key)
            {
                return true;
            }
        }
        return false;
    }

    Sprite GetSpriteForKey(KeyCode key)
    {
        if (key == KeyCode.UpArrow || key == KeyCode.JoystickButton3)
        {
            return _arrowSprite[0]; // Haut (Y)
        }
        else if (key == KeyCode.DownArrow || key == KeyCode.JoystickButton0)
        {
            return _arrowSprite[1]; // Bas (A)
        }
        else if (key == KeyCode.LeftArrow || key == KeyCode.JoystickButton2)
        {
            return _arrowSprite[2]; // Gauche (X)
        }
        else if (key == KeyCode.RightArrow || key == KeyCode.JoystickButton1)
        {
            return _arrowSprite[3]; // Droite (B)
        }
        return null;
    }

    void TriggerAnimation(KeyCode key)
    {
        if (key == KeyCode.UpArrow || key == KeyCode.JoystickButton3)
        {
            PlayAnimationByName("Up");
            gameObject.GetComponent<Image>().sprite = _arrowSprite[0];
        }
        else if (key == KeyCode.DownArrow || key == KeyCode.JoystickButton0)
        {
            PlayAnimationByName("Down");
        }
        else if (key == KeyCode.LeftArrow || key == KeyCode.JoystickButton2)
        {
            PlayAnimationByName("Left");
        }
        else if (key == KeyCode.RightArrow || key == KeyCode.JoystickButton1)
        {
            PlayAnimationByName("Right");
        }
        else
        {
            _animator.Play("Idle");
        }
    }

    void PlayAnimationByName(string name)
    {
        _animator.Play(name);
    }
}
