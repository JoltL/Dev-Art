using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Chair : MonoBehaviour
{

    public Transform _target;
    [SerializeField] private float _speed = 5;
    void Update()
    {
        float randomSpeed = Random.Range(5f,10f);
        float speed = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed);

        if(transform.position == _target.position)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.GetComponent<PlayerController>()._score--;
            Destroy(gameObject);
        }
    }
   
}
