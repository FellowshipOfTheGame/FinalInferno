#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPreview(typeof(EvilSpirit))]
    public class EvilSpiritPreview : UnitPreview { }
#endif
}
