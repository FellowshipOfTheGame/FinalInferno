using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName = "BattleUI SM/Actions/Load Info")]
    public class LoadInfoAction : ComponentRequester
    {
        private HeroInfo info;
        private BattleDebug battle;

        public override void Act(StateController controller)
        {
            info.LoadInfo(battle.GetCurrentUnit());
        }

        public override void RequestComponent(GameObject provider)
        {
            HeroInfo newInfo = provider.GetComponent<HeroInfo>();
            BattleDebug newBattle = provider.GetComponent<BattleDebug>();
            if (newInfo != null)
                info = newInfo;
            if (newBattle != null)
                battle = newBattle;
        }
    }

}