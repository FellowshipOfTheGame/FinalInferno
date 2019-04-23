using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// Reference to the main axis of the manager.
	/// </summary>
    public enum AxisEnum
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// A component implementing a manager for a group of items that can
    /// be selected by keyboard arrows.
    /// </summary>
    public class AIIManager : MonoBehaviour
    {
        /// <summary>
        /// Current item selected by the manager.
        /// </summary>
        public AxisInteractableItem currentItem;

        /// <summary>
        /// Orientation of the system, responsible for the identification
        /// of the main axis (Horizontal/Vertical).
        /// </summary>
        public AxisEnum orientation;

        /// <summary>
        /// Key pressed to execute the action relative of the 
        /// current item.
        /// </summary>
        [SerializeField] private string activatorAxis;

        /// <summary>
        /// Time gone since the last key down.
        /// </summary>
        private float _time;

        /// <summary>
        /// Minimum time between clicks.
        /// </summary>
        public float timeBetweenClicks;

        /// <summary>
        /// State of the manager.
        /// </summary>
        public bool active;

        void Start()
        {
            _time = 0f;
            active = false;
        }

        void Update()
        {
            if (active)
            {
                // Verify if the axis keys are down only after the minimum time has passed.
                _time += Time.deltaTime;
                if (_time >= timeBetweenClicks)
                {
                    // Validate and change the current item by the relative axis.
                    float direction = Input.GetAxisRaw(orientation.ToString());
                    if (direction > .5f)
                    {
                        ChangeItem(currentItem.positiveItem);
                        _time = 0f;
                    }
                    else if (direction < -.5f)
                    {
                        ChangeItem(currentItem.negativeItem);
                        _time = 0f;
                    }
                }

                // If the act key is down, start the current item actions.
                if (Input.GetAxisRaw(activatorAxis) != 0)
                {
                    currentItem.Act();
                }
            }
        }

        /// <summary>
        /// Active the manager and the current item.
        /// </summary>
        public void Active()
        {
            active = true;
            if (currentItem != null)
            {
                currentItem.Enter();
            }
        }

        /// <summary>
        /// Desactive the manager and the current item.
        /// </summary>
        public void Desactive()
        {
            active = false;
            if (currentItem != null)
            {
                currentItem.Exit();
            }
        }

        /// <summary>
        /// Change the current item to the next.
        /// </summary>
        /// <param name="nextItem"> Next item to be activated if exists. </param>
        private void ChangeItem(AxisInteractableItem nextItem)
        {
            if (currentItem != null && nextItem != null)
            {
                currentItem.Exit();
                currentItem = nextItem;
                currentItem.Enter();
            }
        }
    }

}
