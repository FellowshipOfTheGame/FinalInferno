using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Thanks to https://medium.com/@silviocarrera/unity-brightlib-animator-audio-da141b2d6609

namespace FinalInferno.AudioAnimation
{
    public enum PlayCondition { OnEnter, OnUpdate, OnExit }

    public class PlayAudioClip : StateMachineBehaviour
    {
        public bool useMultiple;
        public AudioClip clip;
        public AudioClip[] clips;
        private int _clipIndex;
        public float delay;
        public PlayCondition condition;
        [Tooltip("What's the interval between playing the audio on the update")]
        public float onUpdateInterval;
        private float _lastUpdateTime;

        private AudioSource _source;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_source == null) FetchAudioSource(animator);
            _lastUpdateTime = Time.time;

            if (condition != PlayCondition.OnEnter) return;
            Execute();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (condition != PlayCondition.OnUpdate) return;

            if (Time.time - _lastUpdateTime < onUpdateInterval) return;
            _lastUpdateTime = Time.time;
            Execute();
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (condition != PlayCondition.OnExit) return;
            Execute();
        }

        private void Execute()
        {
            if (_source == null) throw new NullReferenceException();
            if (useMultiple && clips == null) throw new NullReferenceException();
            if (useMultiple && clips.Length == 0) throw new IndexOutOfRangeException();

            _source.clip = !useMultiple ? clip : clips[_clipIndex++ % clips.Length];
            _source.volume = 0.35f;
            _source.PlayDelayed(delay);
        }


        private void FetchAudioSource(Animator animator)
        {
            _source = animator.GetComponent<AudioSource>();
            if (_source == null)
            {
                Debug.LogWarning("PlayAudioClip: No AudioSource found on the Animator's GameObject");
            }
        }
    }

}