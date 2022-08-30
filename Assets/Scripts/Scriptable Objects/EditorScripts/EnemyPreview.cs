#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPreview(typeof(Enemy))]
    public class EnemyPreview : UnitPreview { }
#endif
}
