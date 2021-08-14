using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    //engloba os personagens do jogador
    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObject/Character")]
    public class Character : ScriptableObject/*, IDatabaseItem*/{
        public Hero archetype; //classe desse personagem
        //public type skillInfo; //
        public int hpCur; //vida atual do personagem, descontando dano da vida maxima
        public Vector2 position; //posicao do personagem no Overworld
        public Vector2 direction; // direção do personagem no Overworld
        //public bool isPresent;
        private CharacterOW overworldInstance;
        public CharacterOW OverworldInstance{
            get => overworldInstance;
            set => overworldInstance = (overworldInstance == null? value : (value == null? null : overworldInstance));
        }

        //funcao que ajusta a vida atual do personagem quando sobe de nivel
        public void LevelUp(int level){
            //Debug.Log(archetype.name + " passou pro level: " + level);
            hpCur = archetype.LevelUp(level);
        }
        
        public void ResetCharacter(){
            position = Vector2.zero;
            archetype.ResetHero();
            hpCur = archetype.hpMax;
        }
    }
}