using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(fileName = "OverworldSkill", menuName = "ScriptableObject/OverworldSkill")]
    public class OverworldSkill : PlayerSkill
    {
        #region OverworldCallbacks
        private List<IOverworldSkillListener> activationListeners = new List<IOverworldSkillListener>();

        public void AddActivationListener(IOverworldSkillListener listener){
            if(!activationListeners.Contains(listener)){
                activationListeners.Add(listener);
            }
        }

        public void RemoveActivationListener(IOverworldSkillListener listener){
            if(activationListeners.Contains(listener)){
                activationListeners.Remove(listener);
            }
        }

        public void Activate(){
            if(!active){
                active = true;
                for(int i = activationListeners.Count-1; i > 0; i--){
                    activationListeners[i]?.ActivatedSkill(this);
                }
            }
        }

        public void Deactivate(bool ignoreCallbacks = false){
            if(active){
                active = false;
                for(int i = activationListeners.Count-1; i > 0; i--){
                    if(!ignoreCallbacks)
                        activationListeners[i]?.DeactivatedSkill(this);
                }
            }
        }
        #endregion

        #region SkillOverrides
        public override void LoadTables(){}
        public override void Preload(){}
		public override void Use(BattleUnit user, BattleUnit target, bool shouldOverride1 = false, float value1 = 0, bool shouldOverride2 = false, float value2 = 0){}
		public override void Use(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0, bool shouldOverride2 = false, float value2 = 0){}	
		protected override void UseCallback(BattleUnit user, List<BattleUnit> targets, bool shouldOverride1 = false, float value1 = 0, bool shouldOverride2 = false, float value2 = 0){}	
        public override void ResetSkill(){} 
        #endregion
    }
}
