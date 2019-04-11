using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{

    [CreateAssetMenu(menuName = "BattleUI SM/Decisions/Key")]
    public class KeyDecision : Decision
    {
        [SerializeField] private KeyCode key;

        public override bool Decide(StateController controller)
        {
            return Input.GetKeyDown(key);
        }

    }

}
