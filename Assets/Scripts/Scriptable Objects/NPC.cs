using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewNPC", menuName = "ScriptableObject/DialogueSystem/NPC")]
public class NPC : Fog.Dialogue.DialogueEntity
{
    [Space(10)]
    [Header("Dialogue Info")]
    [SerializeField]
    private new string name;
    [SerializeField]
    private Color color;
    public override Color DialogueColor { get { return color; } }
    public override string DialogueName { get { return (name == null)? "" : name; } }
    [SerializeField]
    protected Sprite dialoguePortrait = null; //
    public override Sprite DialoguePortrait { get => dialoguePortrait; }

}
