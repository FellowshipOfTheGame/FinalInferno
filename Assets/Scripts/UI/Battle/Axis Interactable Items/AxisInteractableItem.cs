using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.AII
{
    public class AxisInteractableItem : MonoBehaviour
    {

        [SerializeField] private GameObject activeReference;

        public delegate void ItemAction();
        public event ItemAction OnEnter;
        public event ItemAction OnExit;
        public event ItemAction OnAct;

        public AxisInteractableItem negativeItem;
        public AxisInteractableItem positiveItem;

        [SerializeField] private bool active;

        void Awake()
        {
            OnEnter += EnableGO;
            OnExit += DisableGO;
        }

        public void Enter()
        {
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        public void Act()
        {
            OnAct();
        }

        private void EnableGO()
        {
            activeReference.SetActive(true);
            active = true;
        }

        private void DisableGO()
        {
            activeReference.SetActive(false);
            active = false;
        }
    }

}
