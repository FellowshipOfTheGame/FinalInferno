﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno.UI.FSM
{
    [CreateAssetMenu(menuName="BattleUI SM/Actions/Change String Variable")]
    public class ChangeStringVariable : ChangeGenericVariable<StringVariable, string> { }
}