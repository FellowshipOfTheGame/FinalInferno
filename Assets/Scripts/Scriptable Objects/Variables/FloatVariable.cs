﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [CreateAssetMenu(menuName="Variable/Float")]
    public class FloatVariable : GenericVariable<float> {
        public void IncrementValue(float increment){
            value += increment;
        }
    }
}