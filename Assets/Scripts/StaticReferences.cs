using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

public class StaticReferences : MonoBehaviour
{
    public static StaticReferences instance = null;
    [SerializeField] private Party party;
    public List<Quest> activeQuests;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(this);
        }
    }
}
