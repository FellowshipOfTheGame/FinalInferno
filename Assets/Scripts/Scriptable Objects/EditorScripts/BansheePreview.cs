#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPreview(typeof(Banshee))]
    public class BansheePreview : UnitPreview { }
#endif
}
