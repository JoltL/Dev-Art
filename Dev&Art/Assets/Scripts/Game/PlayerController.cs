using Spine.Unity;

using TMPro;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Moves")]
    [SerializeField] private int _limit;
    private float _horizontal;
    private Rigidbody _rb;
    [SerializeField] private float _speed;

    [SerializeField] private float _jumpForce;

    private bool _isGrounded;

    [Header("Score")]
    public int _score;
    [SerializeField] private TMP_Text _scoreText;

    public int _maxScore;

    [Header("Coding")]
    public bool _canCode;
    Computer _computer;

    public int _tapCount;

    [Header("Spine")]
    public SkeletonAnimation _skeletonAnimation;
    public AnimationReferenceAsset idle, run, jump, hit, code, wakeup;
    public string currentState;
    public string currentAnimation;
    public string previousState;

    //Pour bouton wakeup
    public Animator _animator;


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

        if (_score < 0)
        {
            _score = 0;
        }

        _score = Mathf.Clamp(_score, 0, _maxScore);
        GetPaper();

        // Vérifie si le joueur est au sol et appuie sur "Jump" pour sauter
        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Move()
    {
        _horizontal = Input.GetAxis("Horizontal");
        float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);
        transform.position = new Vector3(limitsx, transform.position.y, transform.position.z);

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
        SetCharacterState("Jump");
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z); // Applique la force de saut uniquement sur l'axe vertical
        _isGrounded = false;
    }

    private void FixedUpdate()
    {
        Vector3 newVelocity = new Vector3(_horizontal * _speed, _rb.velocity.y, 0);
        _rb.velocity = newVelocity;
    }

    private void GetPaper()
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
                _score -= _maxScore;
                _canCode = false;
                _tapCount = 0;
            }
        }
        UpdateText();
    }

    public void UpdateText()
    {
        _scoreText.text = _score.ToString() + "/5";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    // Set Player Animation
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale = 1)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = _skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale = timeScale;
        animationEntry.Complete += AnimationEntry_Complete;
        currentAnimation = animation.name;
    }

    // Do something after animation completes
    public void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {

        if (currentState.Equals("Wakeup"))
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
        if (currentState.Equals("Wakeup"))
        {
            SetAnimation(wakeup, false);

            _animator.SetTrigger("Wakeup");
        }
        else if (currentState.Equals("Code"))
        {
            SetAnimation(code, true);
        }
        else if (currentState.Equals("Hit"))
        {
            SetAnimation(hit, false);
        }
        else if (state != currentState)
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
        currentState = state;
    }
}
