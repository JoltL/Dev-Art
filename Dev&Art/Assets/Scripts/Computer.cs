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

    [Header("ButtonPlay")]

    [SerializeField] private GameObject _button;
    public bool _closeToPC = false;

    [SerializeField] private Slider _tapSlider;

    public void Coding()
    {
        print("coding");
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
            _button.SetActive(true);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _tapSlider.gameObject.SetActive(true);

            _tapSlider.maxValue = 5;
            _tapSlider.value = other.GetComponent<PlayerController>()._tapCount;

            _closeToPC = true;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            _tapSlider.gameObject.SetActive(true);

            _tapSlider.maxValue = 5;
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
