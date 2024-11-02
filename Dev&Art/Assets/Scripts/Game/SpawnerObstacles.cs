using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObstacles : MonoBehaviour
{
    [SerializeField] private Transform[] _pos;
    [SerializeField] private GameObject[] _obstacles;
    [SerializeField] private float _speed;

    [SerializeField] private float _time;
    [SerializeField] private float _targetTime;

    private void Update()
    {
        if (UIManager.Instance._isStarting == true)
        {

            _time += Time.deltaTime;

            if (_time >= _targetTime)
            {
                _targetTime = Random.Range(5f, 10f);
                _time = 0;
                SpawnObstacles();
            }
        }
        void SpawnObstacles()
        {
            int randomObstacle = Random.Range(0, _obstacles.Length);
            int randomPos = Random.Range(0, _pos.Length);
            GameObject spawnedObstacles = Instantiate(_obstacles[randomObstacle], _pos[randomPos]);

            if (_pos[0])
            {
                spawnedObstacles.GetComponent<Chair>()._target = _pos[1];
            }
            if (_pos[1])
            {
                spawnedObstacles.GetComponent<Chair>()._target = _pos[0];
            }
            print("spawn" + spawnedObstacles.name);
        }
    }
}
