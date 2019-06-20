using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public class GiveExp : MonoBehaviour
    {
        void Update(){
            if(Input.GetKey(KeyCode.Z)){
                Party.Instance.GiveExp(100);
            }
        }
    }
}
