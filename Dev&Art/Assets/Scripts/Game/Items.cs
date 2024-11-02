using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Items : MonoBehaviour
{
    public int _malus;

    public PlayerController _playerController;

    private void Update()
    {
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            _playerController = other.GetComponent<PlayerController>();
            other.GetComponent<PlayerController>()._score += _malus;

            _playerController.SetCharacterState("Hit");
            if (other.GetComponent<PlayerController>()._score < 1)
            {
                other.GetComponent<PlayerController>()._score = 0;
            }

            other.GetComponent<PlayerController>().UpdateText();
            Destroy(gameObject);


        }
    }

}
