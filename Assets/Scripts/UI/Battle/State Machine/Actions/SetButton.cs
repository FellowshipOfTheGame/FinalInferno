﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.FSM
{
    public class SetButton : MonoBehaviour
    {
        [SerializeField] private ChangeButtonStateAction CBS;
        void Start()
        {
            CBS.SetButton(GetComponent<Button>());
        }

    }

}