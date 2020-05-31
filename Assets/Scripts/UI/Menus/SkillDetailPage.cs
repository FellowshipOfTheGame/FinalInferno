using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FinalInferno.UI.SkillsMenu
{
    [RequireComponent(typeof(AII.AxisInteractableItem))]
    public class SkillDetailPage : MonoBehaviour
    {
        public AII.AxisInteractableItem AII { get; private set; }
        public UnityAction<int> FocusPage = null;
        private bool isIndexSet = false;
        private int index = -1;
        public int Index{
            get => index;
            set{
                index = isIndexSet? index : value;
                isIndexSet = true;
            }
        }
        public void AttemptFocus(){
            FocusPage?.Invoke(Index);
        }
        void Awake(){
            AII = GetComponent<AII.AxisInteractableItem>();
            AII.OnEnter += AttemptFocus;
        }
    }
}