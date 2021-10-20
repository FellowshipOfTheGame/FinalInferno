using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public interface IVariableObserver<T> {
        void ValueChanged(T value);
    }
}
