#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPreview(typeof(Mammon))]
    public class MammonPreview : UnitPreview { }
#endif
}
