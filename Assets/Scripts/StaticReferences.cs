using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

[RequireComponent(typeof(Jukebox))]
public class StaticReferences : MonoBehaviour
{
    public static StaticReferences instance = null;
    public Jukebox BGM;
    public AudioClip mainMenuBGM;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(this);
        }

        BGM = GetComponent<Jukebox>();
    }
}
