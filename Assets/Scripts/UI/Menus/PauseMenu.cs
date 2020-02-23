using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; } = null;
    public static bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(this);
        }

        IsPaused = false;
    }

    public void ChangePauseState()
    {
        IsPaused = !IsPaused;
    }

    public void OnDestroy(){
        if(Instance == this){
            Instance = null;
        }
    }
}
