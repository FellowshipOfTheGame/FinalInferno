using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defending : StatusEffect {
    private int defValue;

    public Defending(BattleUnit trgt, float value, int dur = 1) {
        Duration = dur;
        TurnsLeft = Duration;
        Target = trgt;
        Source = Target;
        defValue = Mathf.FloorToInt(trgt.curDef * value);
        Apply();
    }

    public override void Apply() {
        Target.curDef += defValue;
    }

    public override void Remove() {
        Target.curDef -= defValue;
    }
}
