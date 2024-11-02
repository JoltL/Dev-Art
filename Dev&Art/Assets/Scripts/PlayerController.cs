using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Moves")]
    [SerializeField] private int _limit;
    private float _horizontal;
    private Rigidbody _rb;
    [SerializeField] private float _speed;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityForce = -9.81f;

    [Header("Score")]
    [SerializeField] private int _score;
    [SerializeField] private TMP_Text _scoreText;

    [Header("Paper")]
    [SerializeField] private int _idPaper;
    [SerializeField] private List<GameObject> _papers = new List<GameObject>();

    [Header("ButtonPlay")]

    [SerializeField] private GameObject _button;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        float limitsx = Mathf.Clamp(transform.position.x, -_limit, _limit);

        transform.position = new Vector3(limitsx, transform.position.y, transform.position.z);

        //Gravity
        _rb.AddForce(Vector3.down * _gravityForce);

        GetPaper();
        Jump();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

    }

    void GetPaper()
    {
        if (_score > 0)
        {
            _score = 0;

            if (_papers.Count > 0)
            {
                GameObject paperspawned = _papers[Random.Range(0, _papers.Count - 1)];
                paperspawned.transform.gameObject.SetActive(true);
                _papers.Remove(paperspawned);

            }
            else
            {
                Debug.Log("listevide");
                UIManager.Instance._isStarting = false;
                _button.SetActive(true) ;

            }
        }
    }
    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(_horizontal, 0, 0) * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Items>())
        {
            Destroy(other.gameObject);
            _score++;
            UpdateText();
        }
    }

    void UpdateText()
    {
        _scoreText.text = _score.ToString();
    }
}
