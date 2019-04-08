using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetButton : MonoBehaviour
{
    [SerializeField] private ChangeButtonStateAction CBS;
    void Start()
    {
        CBS.SetButton(GetComponent<Button>());
    }

}
