using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno{
    [CreateAssetMenu(fileName = "Eunuch Satyr", menuName = "ScriptableObject/Enemy/EunuchSatyr")]
    public class EunuchSatyr : Enemy
    {
        private bool IsDrainingSpeed(BattleUnit unit){
            if(unit == null || !(unit.Unit is EunuchSatyr)) return false;
            foreach(StatusEffect effect in unit.effects){
                if(effect is DrainingSpeed && effect.Source == unit){
                    return true;
                }
            }
            return false;
        }

        public override Skill AttackDecision(){
            BattleUnit thisUnit = BattleManager.instance.currentUnit;
            return (IsDrainingSpeed(thisUnit))? attackSkill : skills[0];
        }
    }

    #if UNITY_EDITOR
    [CustomPreview(typeof(EunuchSatyr))]
    public class EunuchSatyrPreview : UnitPreview{
        public override bool HasPreviewGUI(){
            return base.HasPreviewGUI();
        }
        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background){
            base.OnInteractivePreviewGUI(r, background);
        }
    }
    #endif
}
