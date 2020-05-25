using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [RequireComponent(typeof(BattleUnit))]
    public class FollowUnitPosition : MonoBehaviour
    {
        public BattleUnit followTarget = null;
        public BattleUnit thisUnit = null;

        void Awake(){
            thisUnit = GetComponent<BattleUnit>();
        }

        void LateUpdate()
        {
            // Provavelmente não vai ser usado pq CompositeBattleUnit faz mas sentido pro que queremos e é mais completo
            if(followTarget){
                if(followTarget.battleItem.CurrentOffset != Vector2.zero){
                    thisUnit.transform.Translate(followTarget.battleItem.CurrentOffset);
                }
            }
        }
    }
}