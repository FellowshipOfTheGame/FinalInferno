using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//engloba todas as "skills" dos personagens do jogador, que ganham nivel
[CreateAssetMenu(fileName = "PlayerSkill", menuName = "ScriptableObject/PlayerSkill", order = 5)]
public class PlayerSkill : Skill{
    public int level; //inivel da "skill"
    public long xp; //experiencia da "skill"
    public long xpNext; //experiencia necessaria para a "skill" subir de nivel
    public string description; //descricao da "skill" que aparecera para o jogador durante a batalha
    public bool active; //sinaliza se a "skill" esta ativa ou nao
    public List<PlayerSkill> skillsToUpdate; //lista de skills que podem ser destravadas com o level dessa skill
    public List<PlayerSkill> prerequisiteSkill; //lista de skills que sao pre requisitos para essa skill destravar
    public List<int> prerequisiteSkillLevel; //level que a skill de pre requisito precisa estar para essa skill destravar
    public int prerequisiteHeroLevel; //level que o heroi precisa estar para essa skill destravar

    //CONSTRUTOR
    // public PlayerSkill(int level, long xp, long xpNext, string description, bool active, List<PlayerSkill> skillsToUpdate, List<PlayerSkill> prerequisiteSkill, List<int> prerequisiteSkillLevel){
    //     this.level = level;
    //     this.xp = xp;
    //     this.xpNext = xpNext;
    //     this.description = description;
    //     this.active = active;
    //     this.skillsToUpdate = skillsToUpdate;
    //     this.prerequisiteSkill = prerequisiteSkill;
    //     this.prerequisiteSkillLevel = prerequisiteSkillLevel;
    // }

    // checa todos os pre requisitos foram cumpridos para essa skill ser destravada,
    // em caso positivo destrava a skill e retorna TRUE, caso contrario retorna FALSE
    public bool Unlock(){
        bool check = true;

        //checa se todos os pre requisitos foram atendidos
        for(int i = 0; i < prerequisiteSkill.Count; i++){
            check &= prerequisiteSkill[i].CheckLevel(prerequisiteSkillLevel[i]);
        }

        //se todos os pre requisitos foram atendidos, destrava a skill
        if(check) this.level = 1;
        
        return check;
    }

    //checa se o level dessa skill cumpre um pre requisito, i.e., eh maior ou igual a um certo valor
    public bool CheckLevel(int prerequisite){
        return (level >= prerequisite);
    }

    
}
