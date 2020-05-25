using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [RequireComponent(typeof(BattleUnit))]
    public class CompositeBattleUnit : MonoBehaviour
    {
        private BattleUnit thisUnit;
        private bool alreadySetup = false;
        [SerializeField] private List<BattleUnit> appendages = new List<BattleUnit>();

        void Awake()
        {
            thisUnit = GetComponent<BattleUnit>();
        }

        public void AddApendage(BattleUnit newAppendage){
            // Garante que não seja possivel adicionar o mesmo elemento 2 vezes
            if(!appendages.Contains(newAppendage)){
                appendages.Add(newAppendage);
            }
        }

        public void RemoveAppendage(BattleUnit appendage){
            appendages.Remove(appendage);
        }

        void Align(){
            foreach(BattleUnit appendage in appendages){
                Vector2 offset = (thisUnit.transform.position - appendage.transform.position);
                appendage.battleItem.defaultOffset += offset;
            }
        }

        void LateUpdate()
        {
            if(thisUnit.battleItem.CurrentOffset != Vector2.zero){
                // Se o elemento principal tiver sido movido, move todos os apendices
                foreach(BattleUnit unit in appendages){
                    unit.transform.Translate(thisUnit.battleItem.CurrentOffset);
                }
            }else{
                // Checa todos os apendices para ver se algum deles foi movido
                Vector2 offset = Vector2.zero;
                BattleUnit selected = null;
                foreach(BattleUnit unit in appendages){
                    if(unit.battleItem.CurrentOffset != Vector2.zero){
                        offset = unit.battleItem.CurrentOffset;
                        selected = unit;
                        // Para a busca quando encontrar o primeiro elemento com offset
                        // Faz isso porque apenas um deles deveria ter offset
                        break;
                    }
                }
                // Caso tenha sido, movimenta todos os outros elementos
                // Se mais de um elemento tiver sido movido pode dar erro
                // Mas isso não deve acontecer pq apenas a unidade atual é movida
                if(offset != Vector2.zero){
                    thisUnit.transform.Translate(offset);
                    foreach(BattleUnit unit in appendages){
                        if(unit != selected){
                            unit.transform.Translate(offset);
                        }
                    }
                }
            }
            Align();
        }
    }
}