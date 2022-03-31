using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FinalInferno {
#if UNITY_EDITOR
    [CustomPreview(typeof(CerberusHead))]
    public class CerberusHeadPreview : UnitPreview {
        protected override void DrawHeadIndicator(Unit unit, Rect texRect) {
            int previousValue = CerberusHead.heads;
            for (int headIndex = 1; headIndex <= 3; headIndex++) {
                CerberusHead.heads = headIndex;
                base.DrawHeadIndicator(unit, texRect);
            }
            CerberusHead.heads = previousValue;
        }
    }
#endif
}
