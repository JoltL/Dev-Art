using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGame2 : MonoBehaviour
{

    [Header("RandomArrows")]

    private KeyCode[] _allDirections = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
    public int _sequenceNumber = 10; //Difficulté Nombre de flèche __________________________________________________________
    private List<KeyCode> _inputSequence = new List<KeyCode>();
    private int _currentIndex = 0;

    [Header("Instantiate")]

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _placement;
    [SerializeField] private int _gapInBetween = 100;

    [SerializeField] private Sprite[] _arrowSprite; //Ref aux sprites des 4 flèches
    private List<GameObject> _instanciatedArrows = new List<GameObject>();

    [Header("Animate")]

    [SerializeField] private Animator _animator;

    [SerializeField] private ScoreGame2 _scoreGame2;

    [SerializeField] private GameObject[] _popmessage;

    [SerializeField] private GameObject _endPanel;

    private void Start()
    {
       _scoreGame2 = GetComponent<ScoreGame2>();
        RandomArrowsSequence();

    }

    private void Update()
    {
        _scoreGame2._score = Mathf.Clamp(_scoreGame2._score, 0, _sequenceNumber);
        //Si la touche cliquée est la même que la séquence; continuer la séquence
        if (Input.anyKeyDown)
        {
            KeyCode pressedKey = GetPressedKey();
            if (pressedKey != KeyCode.None)
            {
                if (pressedKey == _inputSequence[_currentIndex])
                {

                    TriggerAnimation(pressedKey);
                    _currentIndex++;
                    Destroy(_instanciatedArrows[_currentIndex - 1]);
                    _scoreGame2._score++;
                    _popmessage[0].SetActive(true);
                    _popmessage[1].SetActive(false);

                    // Vérifier si la séquence est complète
                    if (_currentIndex >= _inputSequence.Count)
                    {
                        
                        _currentIndex = 0;
                        _instanciatedArrows.Clear();
                        RandomArrowsSequence();
                        ///!\END
                        _endPanel.SetActive(true); 
                    }
                }
                else
                {
                    PlayAnimationByName("Fail");
                    _scoreGame2._score--;

                    //Mauvaise touche : Perd vie
                    //Difficulté Attaque des ennemis _________________________________________________________________________________________
                    _popmessage[0].SetActive(false);
                    _popmessage[1].SetActive(true);

                }
            }
        }

    }

    //Parmis la liste des directions, en prendre le sequenceNumber ; et l'ajouter dans la liste avec les images correspondantes
    void RandomArrowsSequence()
    {

        _inputSequence.Clear();

        float totalWidth = (_sequenceNumber - 1) * _gapInBetween;

        for (int i = 0; i < _sequenceNumber; i++)
        {
            //Créer une séquence random
            KeyCode randomKey = _allDirections[Random.Range(0, _allDirections.Length)];
            _inputSequence.Add(randomKey);


            //Instantiate la sequence
            GameObject arrows = Instantiate(_arrowPrefab, _placement);
            RectTransform rectTransform = arrows.GetComponent<RectTransform>();

            //Mettre au milieu de placement
            float xOffset = i * _gapInBetween - totalWidth;
            rectTransform.anchoredPosition = new Vector2(xOffset, 0);

            //Ajout des prefabs pour supprimer 
            _instanciatedArrows.Add(arrows);

            //Assigner le sprite du prefab au sprite de l'arrow
            Image arrowImage = arrows.GetComponent<Image>();
            arrowImage.sprite = GetSpriteForKey(randomKey);
        }

        //Debug.Log("New Sequence: " + string.Join(", ", _inputSequence));
    }

    //Prendre la touche qui est cliquée
    KeyCode GetPressedKey()
    {
        foreach (KeyCode key in _allDirections)
        {
            if (Input.GetKeyDown(key))
            { return key; }
        }

        return KeyCode.None;

    }

    //Assigner un sprite à chaque KeyCode
    Sprite GetSpriteForKey(KeyCode key)
    {
        if (key == KeyCode.UpArrow)
        {
            return _arrowSprite[0];
        }
        else if (key == KeyCode.DownArrow)
        {
            return _arrowSprite[1];
        }
        else if (key == KeyCode.LeftArrow)
        {
            return _arrowSprite[2];
        }
        else if (key == KeyCode.RightArrow)
        {
            return _arrowSprite[3];
        }
        return null;
    }

    void TriggerAnimation(KeyCode key)
    {
        if (key == KeyCode.UpArrow)
        {
            PlayAnimationByName("Up");
        }
        else if (key == KeyCode.DownArrow)
        {
            PlayAnimationByName("Down");
        }
        else if (key == KeyCode.LeftArrow)
        {
            PlayAnimationByName("Left");
        }
        else if (key == KeyCode.RightArrow)
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
            if(name == "Up")
            {
                _animator.Play(name);
            }
            else if (name == "Down")
            {
                _animator.Play(name);
            }
            else if (name == "Left")
            { 
                _animator.Play(name);
            }
            else if( name == "Right")
            {
                _animator.Play(name);
            }
            else if (name == "Fail")
            {
                _animator.Play("Fail");
            }
            else
            {
                _animator.Play("Idle");
            }

        }


}
