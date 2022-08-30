#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPreview(typeof(Skeleton))]
    public class SkeletonPreview : UnitPreview { }
#endif
}
