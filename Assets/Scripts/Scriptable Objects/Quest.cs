using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;

[CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObject/Quest")]
public class Quest : ScriptableObject
{
    public bool active;
    [SerializeField]
    public QuestDictionary events;
}
