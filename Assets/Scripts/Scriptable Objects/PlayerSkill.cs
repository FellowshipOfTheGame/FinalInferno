﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FinalInferno;
using System.Data;

//engloba todas as "skills" dos personagens do jogador, que ganham nivel
[CreateAssetMenu(fileName = "PlayerSkill", menuName = "ScriptableObject/PlayerSkill", order = 5)]
public class PlayerSkill : Skill{
    public int level; //inivel da "skill"
    public long xp; //experiencia da "skill"
    public long xpNext; //experiencia necessaria para a "skill" subir de nivel
    public string description; //descricao da "skill" que aparecera para o jogador durante a batalha
    public bool active; //sinaliza se a "skill" esta ativa ou nao
    public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level dessa skill
    public List<PlayerSkill> prerequisiteSkills; //lista de skills que sao pre requisitos para essa skill destravar
    public List<int> prerequisiteSkillsLevel; //level que a skill de pre requisito precisa estar para essa skill destravar
    public int prerequisiteHeroLevel; //level que o heroi precisa estar para essa skill destravar
    [SerializeField] private TextAsset skillTable;
    private DynamicTable table;

    void Awake(){
        table = DynamicTable.Create(skillTable);
    }

    public void LevelUp(){
        //atualiza o value dos efeitos, se for necessario.
    }
    

    //Adiciona os pontos de experiência ao utilizar a skill
    public bool GiveExp(){
        xp += 2;

        //testa se a skill subiu de nivel
        if(xp >= xpNext){
            xpNext = table.Rows[level].Field<long>("XP para próximo nível");
            level++;
            LevelUp();
        
            return true;
        }
        return false;
    }

    // checa se todos os pre requisitos foram cumpridos para essa skill ser destravada,
    // em caso positivo destrava a skill e retorna TRUE, caso contrario retorna FALSE
    public bool CheckUnlock(int heroLevel){
        bool check = true;

        if(heroLevel >= prerequisiteHeroLevel){
            //checa se todos os pre requisitos foram atendidos
            for(int i = 0; i < prerequisiteSkills.Count; i++){
                check &= prerequisiteSkills[i].CheckLevel(prerequisiteSkillsLevel[i]);
            }

            //se todos os pre requisitos foram atendidos, destrava a skill
            if(check) this.level = 1;
        }
        else check = false;

        return check;
    }

    //checa se o level dessa skill cumpre um pre requisito, i.e., eh maior ou igual a um certo valor
    public bool CheckLevel(int prerequisite){
        return (level >= prerequisite);
    }

    //funcao que define como a skill sera usada
    /*public override void Use(BattleUnit user, List<BattleUnit> targets){
        GiveExp();

        foreach (BattleUnit trgt in targets) {
            foreach (SkillEffect effect in effects) {
                effect.Apply(user, trgt);
            }
        }
    }*/
}
