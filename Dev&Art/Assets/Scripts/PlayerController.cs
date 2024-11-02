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
    private float _vertical;
    private Rigidbody _rb;
    [SerializeField] private float _speed;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityForce = -9.81f;

    private bool _isGrounded;

    [Header("Score")]
    public int _score;
    [SerializeField] private TMP_Text _scoreText;

    [Header("Coding")]
    public bool _canCode;
    Computer _computer;

    public int _tapCount;

    [Header("Hit")]

    public GameObject _hitPanel;


    private void Start()
    {
        _computer = FindObjectOfType<Computer>();
        _rb = GetComponent<Rigidbody>();

        _isGrounded = false;
    }
    private void Update()
    {


        _horizontal = Input.GetAxis("Horizontal");
        float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);

        transform.position = new Vector3(limitsx, transform.position.y, transform.position.z);

        //Gravity
        _rb.AddForce(Vector3.down * _gravityForce);

        if(_score < 0)
        { _score = 0; }

        GetPaper();

        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        //!\ADD THIS
            //_vertical -= _gravityForce * Time.deltaTime;

        
    }

    void Jump()
    {
        //Vector applique une force
        //!\REMOVE THIS
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isGrounded = false;

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
        //!\ADD THIS
        //_rb.velocity = new Vector3(_horizontal, _vertical, 0) * _speed;
        _rb.velocity = new Vector3(_horizontal, 0, 0) * _speed;
    }

    public void UpdateText()
    {
        _scoreText.text = _score.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            print("collideGround");
            _isGrounded = true;
        }
    }
}
