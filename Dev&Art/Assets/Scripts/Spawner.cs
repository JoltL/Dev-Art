using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _paper;

    [SerializeField] private float _time;
    [SerializeField] private float _targetTime;


    private void Update()
    {
        if (UIManager.Instance._isStarting == true)
        {

        _time += Time.deltaTime;

        if (_time >= _targetTime)
        {
            Spawn();
            _targetTime = Random.Range(2f, 5f);
            _time = 0;
        }
        }
    }
    void Spawn()
    {
        Vector3 posx = new Vector3(Random.Range(-3f, 3f), transform.position.y - .5f, transform.position.z);
        Instantiate(_paper[Random.Range(0, _paper.Length)], posx, Quaternion.identity);
    }
}
