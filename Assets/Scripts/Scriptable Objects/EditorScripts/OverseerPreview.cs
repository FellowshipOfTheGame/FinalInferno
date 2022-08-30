#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPreview(typeof(Overseer))]
    public class OverseerPreview : UnitPreview { }
#endif
}
