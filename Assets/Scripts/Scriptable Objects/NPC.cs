using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "ScriptableObject/DialogueSystem/NPC")]
public class NPC : Fog.Dialogue.DialogueEntity {
    [Space(10)]
    [Header("Dialogue Info")]
    [SerializeField] private new string name;
    public override string DialogueName => (name == null) ? "" : name;
    [SerializeField] private Color color;
    public override Color DialogueColor => color;
    [SerializeField] protected Sprite dialoguePortrait = null;
    public override Sprite DialoguePortrait => dialoguePortrait;

}
