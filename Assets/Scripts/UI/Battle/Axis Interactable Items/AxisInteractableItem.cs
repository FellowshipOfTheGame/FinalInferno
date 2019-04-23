using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII
{
    /// <summary>
	/// A component implementing an item that can be controlled by 
    /// a keyboard shortcut.
	/// </summary>
    public class AxisInteractableItem : MonoBehaviour
    {
        /// <summary>
        /// A game object that references whether this item is active.
        /// </summary>
        [SerializeField] private GameObject activeReference;

        /// <summary>
        /// A delegate to an action of this item.
        /// </summary>
        public delegate void ItemAction();

        /// <summary>
        /// An event called every time this item become active.
        /// </summary>
        public event ItemAction OnEnter;

        /// <summary>
        /// An event called every time this item become unactive.
        /// </summary>
        public event ItemAction OnExit;

        /// <summary>
        /// An event called every time this item acts.
        /// </summary>
        public event ItemAction OnAct;

        /// <summary>
        /// Reference to the item in a negative position (left/down).
        /// </summary>
        public AxisInteractableItem negativeItem;

        /// <summary>
        /// Reference to the item in a positive position (right/up).
        /// </summary>
        public AxisInteractableItem positiveItem;

        /// <summary>
        /// Represents if the item is active or not.
        /// </summary>
        //[SerializeField] private bool active;

        void Awake()
        {
            OnEnter += EnableGO;
            OnExit += DisableGO;
        }

        /// <summary>
        /// Activates this item.
        /// </summary>
        public void Enter()
        {
            OnEnter();
            //active = true;
        }

        /// <summary>
        /// Deactivates this item.
        /// </summary>
        public void Exit()
        {
            OnExit();
            //active = false;
        }

        /// <summary>
        /// Execute the item actions.
        /// </summary>
        public void Act()
        {
            OnAct();
        }

        /// <summary>
        /// Enable the game object that references this item.
        /// </summary>
        protected void EnableGO()
        {
            activeReference.SetActive(true);
        }

        /// <summary>
        /// Disable the game object that references this item.
        /// </summary>
        protected void DisableGO()
        {
            activeReference.SetActive(false);
        }
    }

}
