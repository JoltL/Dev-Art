using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _paper;

    [SerializeField] private GameObject _sleepPanel;
    [SerializeField] private GameObject _wakePanel;


    [SerializeField] private float _time;
    [SerializeField] private float _targetTime;

    [SerializeField] private float _sleepyTime;
    [SerializeField] private float _targetSleepyTime;

    private void Start()
    {
        _targetSleepyTime = Random.Range(10f, 20f);
        _targetTime = Random.Range(0f, 2f);
    }

    private void Update()
    {
        _sleepyTime += Time.deltaTime;

        if (_sleepyTime >= _targetSleepyTime)
        {
            UIManager.Instance._isStarting = false;
            _sleepPanel.gameObject.SetActive(true);
            _wakePanel.gameObject.SetActive(false);

            _targetSleepyTime = Random.Range(10f, 20f);
            _sleepyTime = 0;
        }

        if (UIManager.Instance._isStarting == true)
        {
            _sleepPanel.gameObject.SetActive(false);
            _wakePanel.gameObject.SetActive(true);
 

            _time += Time.deltaTime;

            if (_time >= _targetTime)
            {

                int randomspawn = Random.Range(0, 5);
                for (int i = 0; i < randomspawn; i++)
                {
                Spawn();
                _targetTime = Random.Range(0f, 2f);

                }
                _time = 0;
            }
        }
    }

    void Spawn()
    {

        Vector3 posx = new Vector3(Random.Range(-8f,8f), transform.position.y -0.5f, transform.position.z);
        GameObject spawnedPaper = Instantiate(_paper[Random.Range(0, _paper.Length)], posx, Quaternion.identity);

        float randomdrag = Random.Range(0.1f, 4f);
        spawnedPaper.GetComponent<Rigidbody>().drag = randomdrag;
    }
}
