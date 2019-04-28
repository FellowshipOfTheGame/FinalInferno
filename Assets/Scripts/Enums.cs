using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno
{
    public enum DamageType {
        Physical,
        Magical,
        None
    }
    public enum Element {
        Fire,
        Ice,
        Wind,
        Earth,
        Neutral
    }
    public enum TargetType {
        SingleAlly,
        MultiAlly,
        SingleEnemy,
        MultiEnemy,
        Self
    }

    public enum VictoryType {
        Nobody,
        Heroes,
        Enemys
    }

    public enum UnitType {
        Hero,
        Enemy
    }
}