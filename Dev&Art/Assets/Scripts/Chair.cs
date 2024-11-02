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
            StartCoroutine(Wakeuptime(collision.gameObject.GetComponent<PlayerController>()));
            Destroy(gameObject);
        }
    }
    IEnumerator Wakeuptime(PlayerController player)
    {
        player._hitPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        player._hitPanel.gameObject.SetActive(false);
    }
}
