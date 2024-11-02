using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerGame2 : MonoBehaviour
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
    private KeyCode _inputSequence2;
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

    public EventSystem _eventSystem;

    public GameObject _endButton;

    private bool _canPlay = true;

    public float _rightTime = 1f;

    Coroutine _timing;

    private void Start()
    {
        _scoreGame2 = GetComponent<ScoreGame2>();
        RandomArrowsSequence();

    }

    private void Update()
    {
        if (_canPlay)
        {
            _scoreGame2._score = Mathf.Clamp(_scoreGame2._score, 0, _sequenceNumber);

            if (Input.anyKeyDown)
            {
                KeyCode pressedKey = GetPressedKey();
                if (pressedKey != KeyCode.None)
                {
                    if (IsCorrectKey(pressedKey, _inputSequence2))
                    {
                        StopCoroutine(_timing);
                        TriggerAnimation(pressedKey);
                        Destroy(_instanciatedArrows[_currentIndex]);
                        _currentIndex++;
                        _scoreGame2._score++;
                        _popmessage[0].SetActive(true);
                        _popmessage[1].SetActive(false);

                        if(_scoreGame2._score >= _sequenceNumber) 
                        {
                            _canPlay = false;
                            _endPanel.SetActive(true);
                        }

                        if (_currentIndex >= _inputSequence.Count)
                        {
                            //END//
                            _currentIndex = 0;
                            _instanciatedArrows.Clear();
                            //_canPlay = false;
                            RandomArrowsSequence();

                            _eventSystem.SetSelectedGameObject(_endButton);
                            //_endPanel.SetActive(true);
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

        KeyCode randomKeys = _allDirections[Random.Range(0, _allDirections.Length)][0];
        _inputSequence2 = randomKeys;

        GameObject arrows = Instantiate(_arrowPrefab, _placement);

        _instanciatedArrows.Add(arrows);

        Image arrowImage = arrows.GetComponent<Image>();
        arrowImage.sprite = GetSpriteForKey(randomKeys); // Utiliser le sprite associé à la direction

        _timing = StartCoroutine(StartTimer());
        
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_rightTime);
        RandomArrowsSequence();
        Destroy(_instanciatedArrows[_currentIndex]);
        _currentIndex++;
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

    bool IsCorrectKey(KeyCode pressedKey, KeyCode correctKeys)
    {
        if (pressedKey == correctKeys)
        {
            return true;
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
