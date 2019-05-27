using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSkillTree{
    //public Party party;

    void Start(){
        // le Skill Table do Gregorim;
        // para cada skill da tabela: 
        //     party.heroes."Gregorim".skills.Add(New skill); //adiciona a skill na lista de skills do heroi
            
        //     para cada skill pai:
        //     party.heroes["Gregorim"].skills[i].prerequisiteSkill.Add(pai); //adiciona o pai na lista de pre requisitos da skill
            
        //     //se a skill tiver level 0
        //     if(!party.heroes["Gregorim"].skills[i].CheckLevel(1)){
        //         bool check = TRUE;

        //         //checa se todos os pais ja estao destravados
        //         foreach (PlayerSkill pai in party.heroes["Gregorim"].skills[i].prerequisiteSkill){
        //             check &= pai.CheckLevel(1)
        //         }

        //         //se todos os pais ja foram destravados, insere a skill na lista
        //         if(chek) party.heroes["Gregorim"].skillsToUpdate.Add(skill);
        //     }


        // ...faz o mesmo pros outros herois.
    }

    
}
