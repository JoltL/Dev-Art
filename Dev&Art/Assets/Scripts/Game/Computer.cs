using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    public static Computer Instance;

    [Header("Paper")]
    [SerializeField] private int _idPaper;
    [SerializeField] private List<GameObject> _papers = new List<GameObject>();
    public int _nbPapers;

    [Header("ButtonPlay")]

    [SerializeField] private GameObject _button;
    public bool _closeToPC = false;

    [SerializeField] private Slider _tapSlider;


    public void Coding()
    {
        print("coding");
        if (_papers.Count > 1)
        {
            _nbPapers++;
            GameObject paperspawned = _papers[Random.Range(0, _papers.Count - 1)];
            paperspawned.transform.gameObject.SetActive(true);
            _papers.Remove(paperspawned);

        }
        else
        {
            Debug.Log("listevide");
            UIManager.Instance._isStarting = false;
            UIManager.Instance._endPanel[3].SetActive(true);

        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _tapSlider.gameObject.SetActive(true);

            _tapSlider.maxValue = other.GetComponent<PlayerController>()._maxScore;
            _tapSlider.value = other.GetComponent<PlayerController>()._tapCount;

            _closeToPC = true;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _tapSlider.gameObject.SetActive(true);

            _tapSlider.maxValue = other.GetComponent<PlayerController>()._maxScore;
            _tapSlider.value = other.GetComponent<PlayerController>()._tapCount;

            _closeToPC = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<PlayerController>())
        {
            _tapSlider.gameObject.SetActive(false);

            _closeToPC = false;
        }
    }
}
