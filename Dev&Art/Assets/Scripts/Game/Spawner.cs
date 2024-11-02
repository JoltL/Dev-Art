using Spine.Unity;
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

    [Header("Spine")]
    public SkeletonAnimation _skeletonAnimation;
    public AnimationReferenceAsset draw, sleep, wakeup;
    public string currentState;
    public string currentAnimation;

    private void Start()
    {
        _targetSleepyTime = Random.Range(10f, 20f);
        _targetTime = Random.Range(0f, 2f);
    }

    private void Update()
    {
        _sleepyTime += Time.deltaTime;

        // Dormir
        if (_sleepyTime >= _targetSleepyTime)
        {
            UIManager.Instance._isStarting = false;
            _sleepPanel.SetActive(true);
            _wakePanel.SetActive(false);

            SetCharacterState("Sleep");

            _targetSleepyTime = Random.Range(10f, 20f);
            _sleepyTime = 0;
        }

        // Réveil
        if (UIManager.Instance._isStarting)
        {
            _sleepPanel.SetActive(false);
            _wakePanel.SetActive(true);

            // Transition du sommeil au réveil
            if (currentState == "Sleep")
            {
                SetCharacterState("Wakeup");
            }
            else
            {
                SetCharacterState("Draw");
            }

            // Spawn
            _time += Time.deltaTime;
            if (_time >= _targetTime)
            {
                int randomspawn = Random.Range(0, 5);
                for (int i = 0; i < randomspawn; i++)
                {
                    Spawn();
                }
                _targetTime = Random.Range(0f, 2f);
                _time = 0;
            }
        }
    }

    void Spawn()
    {
        Vector3 posx = new Vector3(Random.Range(-8f, 8f), transform.position.y - 0.5f, transform.position.z);
        GameObject spawnedPaper = Instantiate(_paper[Random.Range(0, _paper.Length)], posx, Quaternion.identity);

        float randomdrag = Random.Range(0.1f, 4f);
        spawnedPaper.GetComponent<Rigidbody>().drag = randomdrag;
    }

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

    public void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {
        if (currentState == "Wakeup")
        {
            SetCharacterState("Draw");
        }
    }

    public void SetCharacterState(string state)
    {
        if (state != currentState)
        {
            switch (state)
            {
                case "Sleep":
                    SetAnimation(sleep, true);
                    break;
                case "Wakeup":
                    SetAnimation(wakeup, false);
                    break;
                case "Draw":
                    SetAnimation(draw, true);
                    break;
            }
            currentState = state;
        }
    }
}
