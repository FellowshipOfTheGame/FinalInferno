using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAnimator : MonoBehaviour
{
    [SerializeField] private SetTriggerAction[] STAs;
    void Start()
    {
        foreach (SetTriggerAction STA in STAs)
        {
            STA.SetAnimator(GetComponent<Animator>());
        }
    }

}
