using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "ScriptableObject/DialogueSystem/NPC")]
public class NPC : Fog.Dialogue.DialogueEntity
{
    [SerializeField]
    private new string name;
    [SerializeField]
    private Color color;
    public override Color DialogueColor { get { return color; } }
    public override string DialogueName { get { return (name == null)? "" : name; } }
}
