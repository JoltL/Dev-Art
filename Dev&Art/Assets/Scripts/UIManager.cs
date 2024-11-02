using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject _image;
    public bool _isStarting = false;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogWarning("There is another " + this.name + " instance in this scene");
        else
            Instance = this;
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
                print("touch");
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
}
