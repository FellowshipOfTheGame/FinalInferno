using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

[RequireComponent(typeof(Jukebox))]
public class StaticReferences : MonoBehaviour
{
    public static StaticReferences instance = null;
    public static Jukebox BGM{
        get{
            if(instance){
                return instance.bgm;
            }
            return null;
        }
    }
    [SerializeField] private Jukebox bgm;
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

        bgm = GetComponent<Jukebox>();
    }
}
