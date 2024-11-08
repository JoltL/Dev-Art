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
        Destroy(gameObject, 15f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            _playerController = other.GetComponent<PlayerController>();
            other.GetComponent<PlayerController>()._score += _malus;

            if(_malus < 0)
            {
            _playerController.SetCharacterState("Hit");

            }
           

            other.GetComponent<PlayerController>().UpdateText();
            Destroy(gameObject);
            print(gameObject.name);


        }
    }

}
