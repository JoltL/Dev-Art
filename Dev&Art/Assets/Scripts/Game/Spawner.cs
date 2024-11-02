using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _paper;

    [SerializeField] private GameObject _sleepPanel;
    [SerializeField] private GameObject _wakePanel;


    [SerializeField] private float _time;
    [SerializeField] private float _targetTime;

    [SerializeField] private float _sleepyTime;
    [SerializeField] private float _targetSleepyTime;

    [Header("Spine")]
    public SkeletonAnimation _skeletonAnimation;
    public AnimationReferenceAsset draw, sleep, wakeup;
    public string currentState;
    public string currentAnimation;
    public string previousState;


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

    // Set Player Animation
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale = 1)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = _skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale = timeScale;
        animationEntry.Complete += AnimationEntry_Complete;
        currentAnimation = animation.name;
    }

    // Do something after animation completes
    public void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {

        if (currentState.Equals("Wakeup"))
        {
            SetCharacterState(previousState);
        }

        if (currentState.Equals("Hit"))
        {
            SetCharacterState(previousState);
        }
    }

    public void SetCharacterState(string state)
    {
        if (currentState.Equals("Wakeup"))
        {
            SetAnimation(wakeup, false);
        }
        else if (currentState.Equals("Sleep"))
        {
            SetAnimation(sleep, true);
        }
        else 
               
        {
          SetAnimation(draw, true);
        }
          
        currentState = state;
    }

}
