using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesLives : MonoBehaviour
{
    public delegate void HeroUpdate();
    public event HeroUpdate OnUpdate;

    public void UpdateLives()
    {
        OnUpdate();
    }

}
