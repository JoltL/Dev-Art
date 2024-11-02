using Spine.Unity;
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

    public int _maxScore;

    [Header("Coding")]
    public bool _canCode;
    Computer _computer;

    public int _tapCount;

    [Header("Hit")]

    public GameObject _hitPanel;

    [Header("Spine")]

    public SkeletonAnimation _skeletonAnimation;
    public AnimationReferenceAsset idle, run, jump, hit, code, wakeup;
    public string currentState;
    public string currentAnimation;
    public string previousState;

    public bool isDying = false;


    private void Start()
    {

        _computer = FindObjectOfType<Computer>();
        _rb = GetComponent<Rigidbody>();

        _isGrounded = false;
    }
    private void Update()
    {
        SetCharacterState("Idle");

        Move();
        if(_score < 0)
        { _score = 0; }

        _score = Mathf.Clamp(_score, 0, _maxScore);

        GetPaper();

        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        //!\ADD THIS
            //_vertical -= _gravityForce * Time.deltaTime;

        
    }

    private void Move()
    {
        
        _horizontal = Input.GetAxis("Horizontal");
        float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);

        transform.position = new Vector3(limitsx, transform.position.y, transform.position.z);

        //Gravity
        _rb.AddForce(Vector3.down * _gravityForce);

        if (_horizontal != 0)
        {
            if (!currentState.Equals("Jump"))
            {

                SetCharacterState("Run");

            }
        }
        else
        {
            if (!currentState.Equals("Jump"))
            {
                SetCharacterState("Idle");
            }
        }

    }

    void Jump()
    {
        //Vector applique une force
        //!\REMOVE THIS
        SetCharacterState("Jump");
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _isGrounded = false;

    }

    void GetPaper()
    {
        if (_score >= _maxScore)
        {
            _canCode = true;
        }

        if (_canCode && Input.GetButtonUp("Down") && _computer._closeToPC)
        {
            _tapCount++;

            SetCharacterState("Code");


            if (_tapCount >= _maxScore)
            {
                _computer.Coding();
                //A CHANGER 4
                _score -= _maxScore;
                _canCode = false;

                _tapCount = 0;
            }
            
        }
                UpdateText();

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

    //Set Player Animation
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale = 1)
    {
        //Debug.Log("Setting animation: " + animation.name);
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = _skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale = timeScale;
        animationEntry.Complete += AnimationEntry_Complete;
        currentAnimation = animation.name;
    }

    //Do something after animation completes
    public void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {
        if (currentState.Equals("Jump"))
        {
            SetCharacterState(previousState);
        }
        if (currentState.Equals("Wakeup"))
        {
            SetCharacterState(previousState);
        }
        if (currentState.Equals("Code"))
        {
            SetCharacterState(previousState);
        }
        if (currentState.Equals("Hit"))
        {
            SetCharacterState(previousState);
        }

    }

    public void SetCharacterState(string state)
    {
        //if (isDying)
        //{
        //    SetAnimation(die, false);
        //    GetComponentInChildren<Collider>().enabled = false;
        //    enabled = false;
        //    //StartCoroutine(WaitDie());
        //    return;
        //}

        if (currentState.Equals("Wakeup"))
        {
            SetAnimation(wakeup, false);
        }
        else if (currentState.Equals("Code"))
        {
            SetAnimation(code, true);
        }
        
        else if (state != currentState)
        {
            if (state.Equals("Hit"))
            {
                SetAnimation(hit, true);

            }
            else if (currentState != "Hit")
            {
                if (state.Equals("Run"))
                {
                    SetAnimation(run, true, 1.1f);
                }
                else if (state.Equals("Jump"))
                {
                    SetAnimation(jump, false);
                }
                else
                {
                    SetAnimation(idle, true);
                }
            }
        }
        currentState = state;
    }
}
