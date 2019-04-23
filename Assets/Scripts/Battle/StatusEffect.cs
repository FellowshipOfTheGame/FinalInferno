using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect {
    public int Duration { protected set; get; }
    public int TurnsLeft { protected set; get; }
    public BattleUnit Source { protected set; get; }
    public BattleUnit Target { protected set; get; }

    public abstract void Apply();
    public abstract void Remove();
    public void Update() {
        TurnsLeft--;
        if (TurnsLeft == 0)
            Remove();
    }
}
