using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FinalInferno
{
    public enum DamageType {
        Physical = 1, // O enum começa em 1 para a tabela ficar mais human friendly
        Magical,
        None // None deve sempre ser o ultimo
    }
    public enum Element {
        Fire = 1, // O primeiro elemento deve ter valor = 1 (mesmo motivo de DamageType)
        Water,
        Wind,
        Earth,
        Neutral // Neutral deve sempre ser o ultimo
    }
    public enum TargetType {
        SingleAlly, // Mira em um unico aliado (incluindo a si próprio)
        MultiAlly, // Mira em todos os aliados vivos
        SingleEnemy, // Mira em um unico inimigo
        MultiEnemy, // Mira em todos os inimigos vivos
        Self, // Mira em si mesmo
        DeadAlly, // Mira em um unico aliado morto
        DeadAllies, // Mira em todos os aliados mortos
        AllAllies, // Mira em todos os aliados, vivos ou mortos
        DeadEnemy, // Mira em um unico inimigo morto
        DeadEnemies, // Mira em todos os inimigos mortos
        AllEnemies, // Mira em todos os inimigos, vivos ou mortos
        Null
    }

    public enum VictoryType {
        Nobody,
        Heroes,
        Enemys
    }

    public enum UnitType {
        Hero,
        Enemy,
        Null
    }

    public enum SkillType {
        Active,
        PassiveOnSpawn,
        PassiveOnStart,
        PassiveOnEnd,
        PassiveOnReceiveBuff,
        PassiveOnReceiveBuffAll,
        PassiveOnReceiveDebuff,
        PassiveOnReceiveDebuffAll,
        PassiveOnTakeDamage,
        PassiveOnTakeDamageAll,
        PassiveOnDeath,
        //PassiveOnGiveBuff,
        //PassiveOnGiveDebuff,
        //PassiveOnDealDamage,
        PassiveOnSkillUsed
    }

    public enum StatusType {
        Buff,
        Debuff,
        Undesirable,
        None
    }

    [System.Serializable]
     public struct SkillInfo{
        [SerializeField] public int level;
        [SerializeField] public long xp;
        [SerializeField] public long xpToNextLevel;
        [SerializeField] public long xpCumulative;
        [SerializeField] public bool active;
        public SkillInfo(PlayerSkill skill){
            if(skill == null){
                level = 1;
                xp = 0;
                xpCumulative = 0;
                active = false;
                xpToNextLevel = 0;
            }else{
                level = skill.Level;
                xp = skill.xp;
                xpCumulative = skill.XpCumulative;
                active = skill.active;
                xpToNextLevel = skill.xpNext;
            }
        }
    }

    [System.Serializable]
    public struct QuestInfo{
        [SerializeField] public string name;
        [SerializeField] public string[] flagsNames;
        [SerializeField] public ulong flagsTrue;
    }

    [System.Serializable]
    public struct SkillInfoArray{
        [SerializeField] public SkillInfo[] skills;
    }

    [System.Serializable]
    public struct SaveInfo{
        [SerializeField] public long xpParty; // exp acumulativa da party
        [SerializeField] public string mapName; // nome do mapa (cena de overworld) atual
        [SerializeField] public QuestInfo[] quest; // Lsta de informações das quests
        //quests de kill
        [SerializeField] public string[] archetype; // Lista com a ordem dos heroes
        [SerializeField] public int[] hpCur; // hp atual de cada personagem
        [SerializeField] public Vector2[] position; // posição no overworld dos personagens
        [SerializeField] public SkillInfoArray[] heroSkills; // Info de skills
    }

    // Struct a ser usada para visualizar os saveSlots
    public struct SavePreviewInfo{
        public int level;
        public string mapName;
        public List<Hero> heroes;
        public SavePreviewInfo(SaveInfo save){
            // Listas são null por default, portanto um save com uma lista nula não foi inicializado
            if(save.archetype == null){
                level = 0;
                mapName = "";
                heroes = null;
            }else{
                level = Party.Instance.GetLevel(save.xpParty);
                mapName = save.mapName;
                heroes = new List<Hero>();
                foreach(string heroName in save.archetype){
                    heroes.Add(AssetManager.LoadAsset<Hero>(heroName));
                }
            }
        }
    }

    [System.Serializable]
    public class QuestDictionary : RotaryHeart.Lib.SerializableDictionary.SerializableDictionaryBase<string, bool>{ }

    [System.Serializable]
    public struct SkillEffectTuple{
        public SkillEffect effect;
        public float value1;
        public float value2;
    }

    [System.Serializable]
    public struct DialogueEntry{
        public Quest quest;
        public string eventFlag;
        public Fog.Dialogue.Dialogue dialogue;
        public DialogueEntry(Quest _quest, string _eventFlag, Fog.Dialogue.Dialogue _dialogue){
            quest = _quest;
            eventFlag = _eventFlag;
            dialogue = _dialogue;
        }
    }
    #if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar DialogueEntry no editor da unity
    [CustomPropertyDrawer(typeof(DialogueEntry))]
    public class DialogueEntryDrawer : PropertyDrawer{

        private SerializedProperty quest, eventFlag, dialogue;
        private int index;
        private Rect questRect;
        private Rect eventRect;
        private Rect dialogueRect; 

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            int i = (_quest == null)? 2 : (_quest.objectReferenceValue == null)? 2 : 3;
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);

            // Define a posicao do campo de quest
            questRect = new Rect(new Vector2(position.position.x, position.position.y + 5f), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // Encontra as referencias para os elementos da struct
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
            dialogue = property.FindPropertyRelative("dialogue");
            // Cria o campo para a referencia de quest
            EditorGUI.PropertyField(questRect, quest);
            // Se a referencia de quest for nula, pula o campo de eventFlag, copiando a posicao do campo de quest
            eventRect = (quest.objectReferenceValue == null)? questRect : new Rect(new Vector2(questRect.x, questRect.y + questRect.height), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // A posicao do campo de dialogue e relativa a posicao do campo de eventFlag
            dialogueRect = new Rect(new Vector2(eventRect.x, eventRect.y + eventRect.height), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            if(quest.objectReferenceValue != null){
                // Caso uma quest tenha sido referenciada, obtem a lista eventos criados nela
                Quest _quest = (Quest)quest.objectReferenceValue;
                string[] keys = new string[_quest.events.Keys.Count];
                _quest.events.Keys.CopyTo(keys, 0);
                // Cria um popup com as chaves definidas pela quest referenciada
                index = Mathf.Clamp(System.Array.IndexOf(keys, eventFlag.stringValue), 0, Mathf.Max(keys.Length-1, 0));
                index = EditorGUI.Popup(eventRect, "Event", index, keys);
                eventFlag.stringValue = (keys.Length > 0)? keys[index] : "";
            }else{
                // Se nao houver referencia de quest, apenas salva string vazia sem criar o campo de popup
                eventFlag.stringValue = "";
            }
            // Cria o campo de referencia de dialogue
            EditorGUI.PropertyField(dialogueRect, dialogue);

            EditorGUI.EndProperty();
        }
    }
    #endif

    [System.Serializable]
    public struct QuestEvent{
        public Quest quest;
        public string eventFlag;
    }
    #if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar QuestEvent no editor da unity
    [CustomPropertyDrawer(typeof(QuestEvent))]
    public class QuestEventDrawer : PropertyDrawer{

        private SerializedProperty quest, eventFlag;
        private int index;
        private Rect questRect;
        private Rect eventRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            int i = (_quest == null)? 1 : (_quest.objectReferenceValue == null)? 1 : 2;
            return (i * EditorGUIUtility.singleLineHeight) + 5f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);

            // Define a posicao do campo de quest
            questRect = new Rect(new Vector2(position.position.x, position.position.y), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // Encontra as referencias para os elementos da struct
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
            // Cria o campo para a referencia de quest
            EditorGUI.PropertyField(questRect, quest);
            // Se a referencia de quest for nula, pula o campo de eventFlag, copiando a posicao do campo de quest
            eventRect = (quest.objectReferenceValue == null)? questRect : new Rect(new Vector2(questRect.x, questRect.y + questRect.height), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            if(quest.objectReferenceValue != null){
                // Caso uma quest tenha sido referenciada, obtem a lista eventos criados nela
                Quest _quest = (Quest)quest.objectReferenceValue;
                string[] keys = new string[_quest.events.Keys.Count];
                _quest.events.Keys.CopyTo(keys, 0);
                // Cria um popup com as chaves definidas pela quest referenciada
                index = Mathf.Clamp(System.Array.IndexOf(keys, eventFlag.stringValue), 0, Mathf.Max(keys.Length-1, 0));
                index = EditorGUI.Popup(eventRect, "Event", index, keys);
                eventFlag.stringValue = (keys.Length > 0)? keys[index] : "";
            }else{
                // Se nao houver referencia de quest, apenas salva string vazia sem criar o campo de popup
                eventFlag.stringValue = "";
            }

            EditorGUI.EndProperty();
        }
    }
    #endif

    [System.Serializable]
    public struct ChangeRule{
        public Quest quest;
        public string eventFlag;
        public string animationFlag;
        public bool newValue;
        public ChangeRule(Quest _quest, string _eventFlag, string _animationFlag, bool _newValue){
            quest = _quest;
            eventFlag = _eventFlag;
            animationFlag = _animationFlag;
            newValue = _newValue;
        }
    }
    #if UNITY_EDITOR
    // PropertyDrawer necessario para exibir e editar ChangeRule no editor da unity
    [CustomPropertyDrawer(typeof(ChangeRule))]
    public class ChangeRuleDrawer : PropertyDrawer{

        private SerializedProperty quest, eventFlag, animationFlag, toggleValue;
        private int index, index2;
        private bool toggle;
        private Rect questRect;
        private Rect eventRect;
        private Rect animRect;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            SerializedProperty _quest = property.FindPropertyRelative("quest");
            Animator _anim = Selection.activeGameObject.GetComponent<Animator>();
            int i = 1 + ((_quest != null && _quest.objectReferenceValue != null)? 1 : 0) + ((_anim != null && _anim.runtimeAnimatorController != null)? 1 : 0);
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);

            // Define a posicao do campo de quest
            questRect = new Rect(new Vector2(position.position.x, position.position.y + 5f), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            // Encontra as referencias para os elementos da struct
            quest = property.FindPropertyRelative("quest");
            eventFlag = property.FindPropertyRelative("eventFlag");
            animationFlag = property.FindPropertyRelative("animationFlag");
            toggleValue = property.FindPropertyRelative("newValue");
            toggle = toggleValue.boolValue;
            // Cria o campo para a referencia de quest
            EditorGUI.PropertyField(questRect, quest);
            // Se a referencia de quest for nula, pula o campo de eventFlag, copiando a posicao do campo de quest
            eventRect = (quest.objectReferenceValue == null)? questRect : new Rect(new Vector2(questRect.x, questRect.y + questRect.height), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            if(quest.objectReferenceValue != null){
                // Caso uma quest tenha sido referenciada, obtem a lista eventos criados nela
                Quest _quest = (Quest)quest.objectReferenceValue;
                string[] keys = new string[_quest.events.Keys.Count];
                _quest.events.Keys.CopyTo(keys, 0);
                // Cria um popup com as chaves definidas pela quest referenciada
                index = Mathf.Clamp(System.Array.IndexOf(keys, eventFlag.stringValue), 0, Mathf.Max(keys.Length-1, 0));
                index = EditorGUI.Popup(eventRect, "Event", index, keys);
                eventFlag.stringValue = (keys.Length > 0)? keys[index] : "";
            }else{
                // Se nao houver referencia de quest, apenas salva string vazia sem criar o campo de popup
                eventFlag.stringValue = "";
            }
            // Cria o campo de referencia de animation
            Animator anim = Selection.activeGameObject.GetComponent<Animator>();
            anim.enabled = false; // asjdhaisduhaloisfbhaslkdjhaskfjhbaskdjahsbnçakjsdbaksdjbasçdajsnd
            anim.enabled = true; // alskdj amsdhna sdcnhdhvasdcjasdcjmasldkc ajsldaml  unity pls
            if(anim == null || anim.runtimeAnimatorController == null){
                animRect = new Rect(eventRect);
                animationFlag.stringValue = "";
                anim = null;
            }else{
                animRect = new Rect(new Vector2(eventRect.x, eventRect.y + eventRect.height), new Vector2(position.size.x - 40, EditorGUIUtility.singleLineHeight));
                Rect toggleRect = new Rect(new Vector2(animRect.xMax, animRect.position.y), new Vector2(position.size.x - animRect.size.x, EditorGUIUtility.singleLineHeight));
                AnimatorControllerParameter[] allParamaters = anim.parameters;
                List<string> parameters = new List<string>();
                foreach(AnimatorControllerParameter param in allParamaters){
                    if(param.type == AnimatorControllerParameterType.Bool)
                        parameters.Add(param.name);
                }
                index2 = Mathf.Clamp(parameters.IndexOf(animationFlag.stringValue), 0, Mathf.Max(parameters.Count-1, 0));
                index2 = EditorGUI.Popup(animRect, "Animation flag", index2, parameters.ToArray());
                animationFlag.stringValue = (parameters.Count > 0)? parameters[index2] : "";
                toggle = EditorGUI.Toggle(toggleRect, "", toggle);
                toggleValue.boolValue = toggle;
                anim = null;
            }

            EditorGUI.EndProperty();
        }
    }
    #endif
}