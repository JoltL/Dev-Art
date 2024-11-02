using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    [Header("Moves")]
    [SerializeField] private int _limit;
    private float _horizontal;
    private Rigidbody _rb;
    [SerializeField] private float _speed;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityForce = -9.81f;

    [Header("Score")]
    public int _score;
    [SerializeField] private TMP_Text _scoreText;

    [Header("Coding")]
    public bool _canCode;
    Computer _computer;

    public int _tapCount;

  
    private void Start()
    {
        _computer = FindObjectOfType<Computer>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {


        _horizontal = Input.GetAxis("Horizontal");
        float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);

        transform.position = new Vector3(limitsx, transform.position.y, transform.position.z);

        //Gravity
        _rb.AddForce(Vector3.down * _gravityForce);

        _score = Mathf.Clamp(_score, 0, 100);

        GetPaper();
        Jump();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        //Vector3 Movement = 
        //_jumpForce -= _gravityForce * Time.deltaTime;
        //Movement += _jumpForce * Time.deltaTime;
        //transform.position += Movement;



    }

    void GetPaper()
    {
        if (_score >= 2)
        {
            _canCode = true;
        }

        if (_canCode && Input.GetButtonUp("Down") && _computer._closeToPC)
        {
            _tapCount++;


            if (_tapCount >= 5)
            {
                _computer.Coding();
                //A CHANGER 4
                _score = _score - 2;
                _canCode = false;

                UpdateText();
                _tapCount = 0;
            }
            
        }

    }
    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(_horizontal, 0, 0) * _speed;
    }

    public void UpdateText()
    {
        _scoreText.text = _score.ToString();
    }
}
