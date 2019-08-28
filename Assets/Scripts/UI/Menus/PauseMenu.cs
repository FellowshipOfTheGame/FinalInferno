﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    private void Start()
    {
        isPaused = false;
    }

    public static void ChangePauseState()
    {
        isPaused = !isPaused;
    }
}
