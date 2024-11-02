using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject _image;
    public bool _isStarting = false;

    public GameObject _pausePanel;

    [Header("CHRONO")]
    //CHRONO OF THE GAME
    private float _time;
    private int _chrono;
    [SerializeField] private TMP_Text _chronoText;

    [SerializeField] private float _targetTime = 120f;
    [SerializeField] private GameObject _endPanel;


    private void Awake()
    {
        Time.timeScale = 1f;
        if (Instance != null)
            Debug.LogWarning("There is another " + this.name + " instance in this scene");
        else
            Instance = this;
    }

    private void Update()
    {
        _targetTime -= Time.deltaTime;
        _chrono = (int)_targetTime;
        _chronoText.text = _chrono.ToString();

        if(_targetTime <= 0)
        {
            _targetTime = 0;
            UIManager.Instance._isStarting = false;
            _endPanel.SetActive(true);
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
        PauseGame();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            _image.SetActive(true);
            if (!_isStarting)
            {
                if (Input.GetButton("Down"))
                {
                    other.GetComponent<PlayerController>().SetCharacterState("Wakeup");
                    print("touch");
                    _isStarting = true;
                }
            }
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {

            if (Input.GetButton("Down"))
            {
                other.GetComponent<PlayerController>().SetCharacterState("Wakeup");
                _isStarting = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            _image.SetActive(false);
        }
    }

    public void PauseGame()
    {


        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
            _pausePanel.SetActive(true);

        }
        else
        {
            Time.timeScale = 1f;
            _pausePanel.SetActive(false);

        }

    }

    public void ChangeScene(int scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

}
