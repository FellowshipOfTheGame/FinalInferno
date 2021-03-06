﻿using System.Collections;
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
        PassiveOnSkillUsed
        //PassiveOnGiveBuff,
        //PassiveOnGiveDebuff,
        //PassiveOnDealDamage,
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
        public static bool operator ==(SkillInfo left, SkillInfo right){
            return left.Equals(right);
        }
        public static bool operator !=(SkillInfo left, SkillInfo right){
            return !(left == right);
        }
        public override bool Equals(object obj){
            if(obj.GetType() != typeof(SkillInfo))
                return false;

            return Equals((SkillInfo)obj);
        }
        public bool Equals(SkillInfo other){
            if(level != other.level || xp != other.xp || xpToNextLevel != other.xpToNextLevel || xpCumulative != other.xpCumulative || active != other.active)
                return false;

            return true;
        }
        public override int GetHashCode(){
            return (3 * level.GetHashCode() + 5 * xp.GetHashCode() + 7 * xpToNextLevel.GetHashCode() + 11 * xpCumulative.GetHashCode() + 13 * active.GetHashCode());
        }
    }

    [System.Serializable]
    public struct QuestInfo{
        [SerializeField] public string name;
        [SerializeField] public string[] flagsNames;
        [SerializeField] public ulong flagsTrue;
        public static bool operator ==(QuestInfo left, QuestInfo right){
            return left.Equals(right);
        }
        public static bool operator !=(QuestInfo left, QuestInfo right){
            return !(left == right);
        }
        public override bool Equals(object obj){
            if(obj.GetType() != typeof(QuestInfo))
                return false;

            return Equals((QuestInfo)obj);
        }
        public bool Equals(QuestInfo other){
            if(name != other.name)
                return false;

            if(flagsNames != null && other.flagsNames != null){
                if(flagsNames.Length != other.flagsNames.Length)
                    return false;
                for(int i = 0; i < flagsNames.Length; i++){
                    if(flagsNames[i] != other.flagsNames[i])
                        return false;
                }
            }else if(flagsNames != null || other.flagsNames != null){
                return false;
            }

            if(flagsTrue != other.flagsTrue)
                return false;

            return true;
        }
        public override int GetHashCode(){
            return (3 * name.GetHashCode() + 5 * flagsNames.GetHashCode() + 7 * flagsTrue.GetHashCode());
        }
    }

    [System.Serializable]
    public struct SkillInfoArray{
        [SerializeField] public SkillInfo[] skills;
        public static bool operator ==(SkillInfoArray left, SkillInfoArray right){
            return left.Equals(right);
        }
        public static bool operator !=(SkillInfoArray left, SkillInfoArray right){
            return !(left == right);
        }
        public override int GetHashCode(){
            return skills.GetHashCode();
        }
        public override bool Equals(object obj){
            if(obj.GetType() != typeof(SkillInfoArray))
                return false;

            return Equals((SkillInfoArray)obj);
        }
        public bool Equals(SkillInfoArray other){
            if(skills != null && other.skills != null){
                if(skills.Length != other.skills.Length)
                    return false;
                for(int i = 0; i < skills.Length; i++){
                    if(skills[i] != other.skills[i])
                        return false;
                }
            }else if(skills != null || other.skills != null){
                return false;
            }

            return true;
        }
    }

    [System.Serializable]
    public struct BestiaryEntry{
        [SerializeField] public string monsterName;
        [SerializeField] public int numberKills;
        public BestiaryEntry(Enemy enemy, int n){
            if(enemy != null){
                monsterName = enemy.AssetName;
            }else{
                monsterName = "";
            }
            numberKills = Mathf.Max(1, n);
        }
        public static bool operator ==(BestiaryEntry left, BestiaryEntry right){
            return left.Equals(right);
        }
        public static bool operator !=(BestiaryEntry left, BestiaryEntry right){
            return !(left == right);
        }
        public override bool Equals(object obj){
            if(obj.GetType() != typeof(BestiaryEntry))
                return false;

            return Equals((BestiaryEntry)obj);
        }
        public bool Equals(BestiaryEntry other){
            if(monsterName != other.monsterName || numberKills != other.numberKills)
                return false;

            return true;
        }
        public override int GetHashCode(){
            return (3 * monsterName.GetHashCode() + 5 * numberKills.GetHashCode());
        }
    }

    [System.Serializable]
    public struct SaveInfo{
        [SerializeField] public long xpParty; // exp acumulativa da party
        [SerializeField] public string mapName; // nome do mapa (cena de overworld) atual
        [SerializeField] public QuestInfo[] quest; // Lsta de informações das quests
        //quests de kill
        [SerializeField] public BestiaryEntry[] bestiary;
        [SerializeField] public string[] archetype; // Lista com a ordem dos heroes
        [SerializeField] public int[] hpCur; // hp atual de cada personagem
        [SerializeField] public Vector2[] position; // posição no overworld dos personagens
        [SerializeField] public SkillInfoArray[] heroSkills; // Info de skills
        [SerializeField] public VolumeController.VolumeInfo volumeInfo; // Configuração de volumes do usuario
        [SerializeField] public bool autoSave; // Configuração de autosave do usuario
        [SerializeField] public string version; // Versão do jogo quando o save foi criado
        public bool Equals(SaveInfo other){
            if(xpParty != other.xpParty)
                return false;
            if(mapName != other.mapName)
                return false;

            if(quest != null && other.quest != null){
                if(quest.Length != other.quest.Length)
                    return false;
                for(int i = 0; i < quest.Length; i++){
                    if(quest[i] != other.quest[i])
                        return false;
                }
            }else if(quest != null || other.quest != null){
                return false;
            }

            if(bestiary != null && other.bestiary != null){
                if(bestiary.Length != other.bestiary.Length){
                    return false;
                }
                for(int i= 0; i < bestiary.Length; i++){
                    if(bestiary[i] != other.bestiary[i])
                        return false;
                }
            }else if(bestiary != null || other.bestiary != null){
                return false;
            }

            if(archetype != null && other.archetype != null){
                if(archetype.Length != other.archetype.Length)
                    return false;
                for(int i = 0; i < archetype.Length; i++){
                    if(archetype[i] != other.archetype[i])
                        return false;
                }
            }else if(archetype != null || other.archetype != null){
                return false;
            }

            if(hpCur != null && other.hpCur != null){
                if(hpCur.Length != other.hpCur.Length)
                    return false;
                for(int i = 0; i < hpCur.Length; i++){
                    if(hpCur[i] != other.hpCur[i])
                        return false;
                }
            }else if(hpCur != null || other.hpCur != null){
                return false;
            }

            if(position != null && other.position != null){
                if(position.Length != other.position.Length)
                    return false;
                for(int i = 0; i < position.Length; i++){
                    if(position[i] != other.position[i])
                        return false;
                }
            }else if(position != null || other.position != null){
                return false;
            }

            if(heroSkills != null && other.heroSkills != null){
                if(heroSkills.Length != other.heroSkills.Length)
                    return false;
                for(int i = 0; i < heroSkills.Length; i++){
                    if(heroSkills[i] != other.heroSkills[i])
                        return false;
                }
            }else if(heroSkills != null || other.heroSkills != null){
                return false;
            }

            return true;
        }
    }

    // Struct a ser usada para visualizar os saveSlots
    public struct SavePreviewInfo{
        public int level;
        public string mapName;
        public List<Hero> heroes;
        public SavePreviewInfo(SaveInfo save){
            // Listas são null por default, portanto um save com uma lista nula não foi inicializado
            if(save.xpParty <= 0){
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
    public class ElementResistanceDictionary : RotaryHeart.Lib.SerializableDictionary.SerializableDictionaryBase<Element, float>{ }

    [System.Serializable]
    public struct SkillEffectTuple{
        public SkillEffect effect;
        public float value1;
        public float value2;
        public void UpdateValues(){
            effect.value1 = value1;
            effect.value2 = value2;
        }
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
                string[] keys = _quest.FlagNames;
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
                string[] keys = _quest.FlagNames;
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
                string[] keys = _quest.FlagNames;
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

    [System.Serializable]
    public class ScenePicker{
        [SerializeField] private string sceneName = "";
        [SerializeField] private string assetPath = "";
        [SerializeField] private string guid = "";
        public string Name { get => sceneName; private set => sceneName = value; }
        public string Path { get => assetPath; private set => assetPath = value; }
        public string GUID { get => guid; private set => guid = value; }

        public ScenePicker(){
            sceneName = "";
            assetPath = "";
            guid = "";
        }
    }
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScenePicker))]
    public class ScenePickerEditor : PropertyDrawer{
        SerializedProperty sceneName;
        SerializedProperty assetPath;
        SerializedProperty guid;
        Rect rect;
        Object sceneObj = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            rect = new Rect(position.x, position.y, position.size.x, EditorGUIUtility.singleLineHeight);
            sceneName = property.FindPropertyRelative("sceneName");
            assetPath = property.FindPropertyRelative("assetPath");
            guid = property.FindPropertyRelative("guid");

            if(sceneObj == null && assetPath.stringValue != ""){
                sceneObj = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath.stringValue);
            }else if(assetPath.stringValue == ""){
                sceneObj = null;
            }

            sceneObj = EditorGUI.ObjectField(rect, sceneObj, typeof(SceneAsset), false);

            if(sceneObj != null){
                sceneName.stringValue = sceneObj.name;
                assetPath.stringValue = AssetDatabase.GetAssetPath(sceneObj.GetInstanceID());
                guid.stringValue = AssetDatabase.AssetPathToGUID(assetPath.stringValue);
            }else{
                sceneName.stringValue = "";
                assetPath.stringValue = "";
                guid.stringValue = "";
            }

            EditorGUI.EndProperty();
        }
    }
    #endif

    [System.Serializable]
    public struct SceneWarp{
        public string scene;
        public Vector2 position;
    }
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SceneWarp))]
    public class SceneWarpDrawer : PropertyDrawer{
        private SerializedProperty sceneName, scenePos;
        private Rect nameRect;
        private Rect posRect;
        Object sceneObj = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
            SerializedProperty _sceneName = property.FindPropertyRelative("scene");
            int i = (_sceneName == null || _sceneName.stringValue == null || _sceneName.stringValue == "")? 1 : 3;
            return (i * EditorGUIUtility.singleLineHeight) + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);

            nameRect = new Rect(new Vector2(position.position.x, position.position.y + 5f), new Vector2(position.size.x, EditorGUIUtility.singleLineHeight));
            posRect = new Rect(new Vector2(nameRect.position.x, nameRect.position.y + nameRect.height), new Vector2(nameRect.size.x, EditorGUIUtility.singleLineHeight));
            sceneName = property.FindPropertyRelative("scene");
            scenePos = property.FindPropertyRelative("position");

            if(sceneObj == null){
                // Tenta achar a cena salva atualmente na pasta de cenas
                string[] objectsFound = AssetDatabase.FindAssets(sceneName.stringValue + " t:sceneAsset", new[] {"Assets/Scenes"});
                if(sceneName.stringValue != null && sceneName.stringValue != "" && objectsFound != null && objectsFound.Length > 0 && objectsFound[0] != null &&  objectsFound[0] != ""){
                    sceneObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(objectsFound[0]), typeof(Object));
                }else{
                    //Debug.Log("Não achou");
                    sceneObj = null;
                }
            }

            sceneObj = EditorGUI.ObjectField(nameRect, sceneObj, typeof(SceneAsset), false);
            sceneName.stringValue = (sceneObj != null)? sceneObj.name : "";

            if(sceneObj != null){
                scenePos.vector2Value = EditorGUI.Vector2Field(posRect, "Position", scenePos.vector2Value);
            }else{
                scenePos.vector2Value = Vector2.zero;
            }

            EditorGUI.EndProperty();
        }
    }
    #endif
}