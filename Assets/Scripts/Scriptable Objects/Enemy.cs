using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using FinalInferno.UI.Battle;

//engloba os inimigos do jogador
[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy", order = 3)]
public class Enemy : Unit{

    public override Color DialogueColor { get { return color; } }
    public override string DialogueName { get { return (name == null)? "" : name; } }
    
    public static Enemy FindEnemy(string enemyName){
        string[] resultsFound = UnityEditor.AssetDatabase.FindAssets(enemyName + " t:" + typeof(Enemy).ToString());
        if(resultsFound.Length > 0){
            return (Enemy)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(resultsFound[0]), typeof(Enemy));
        }
        return null;
    }

    //inteligencia atificial do inimigo na batalha
    public void AIEnemy(){
        Skill skill;
        
        float rand = Random.Range(0, 2);

        if(rand == 0)
            skill = attackSkill;
        else
            skill = defenseSkill;
        
        BattleSkillManager.currentSkill = skill;
        BattleSkillManager.currentTargets = GetTargets(skill.target);

        // BattleSkillManager.UseSkill();
    }

    private List<BattleUnit> GetTargets(TargetType type)
    {
        List<BattleUnit> targets = new List<BattleUnit>();
        List<BattleUnit> team = new List<BattleUnit>();

        switch (type)
        {
            case TargetType.Self:
                targets.Add(BattleManager.instance.currentUnit);
                break;
            case TargetType.MultiAlly:
                targets = team = BattleManager.instance.GetTeam(UnitType.Enemy);
                break;
            case TargetType.MultiEnemy:
                targets = team = BattleManager.instance.GetTeam(UnitType.Hero);
                break;
            case TargetType.SingleAlly:
                team = BattleManager.instance.GetTeam(UnitType.Enemy);
                targets.Add(team[Random.Range(0, team.Count)]);
                break;
            case TargetType.SingleEnemy:
                team = BattleManager.instance.GetTeam(UnitType.Hero);
                targets.Add(team[Random.Range(0, team.Count)]);
                break;
        }

        return targets;
    }
}
