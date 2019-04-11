using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII
{
    public enum AxisEnum
    {
        Horizontal,
        Vertical
    }
    
    public class AIIManager : MonoBehaviour
    {
        [SerializeField] private AxisInteractableItem currentItem;
        public AxisEnum orientation;
        public KeyCode actKey;

        private float _time;
        public float timeBetweenClicks;

        [SerializeField] private bool active;

        void Start()
        {
            //Active();
            _time = 0f;
            active = false;
        }

        void Update()
        {
            if (active)
            {
                _time += Time.deltaTime;
                if (_time >= timeBetweenClicks)
                {
                    float direction = Input.GetAxisRaw(orientation.ToString());
                    if (direction > .5f)
                    {
                        GoPositive();
                        _time = 0f;
                    }
                    else if (direction < -.5f)
                    {
                        GoNegative();
                        _time = 0f;
                    }
                }

                if (Input.GetKeyDown(actKey))
                {
                    currentItem.Act();
                }
            }
        }

        public void Active()
        {
            active = true;
            if (currentItem != null)
            {
                currentItem.Enter();
            }
        }

        public void Desactive()
        {
            active = false;
            if (currentItem != null)
            {
                currentItem.Exit();
            }
        }

        private void GoPositive()
        {
            if (currentItem != null && currentItem.positiveItem != null)
            {
                currentItem.Exit();
                currentItem = currentItem.positiveItem;
                currentItem.Enter();
            }
        }

        private void GoNegative()
        {
            if (currentItem != null && currentItem.negativeItem != null)
            {
                currentItem.Exit();
                currentItem = currentItem.negativeItem;
                currentItem.Enter();
            }
        }
    }

}
