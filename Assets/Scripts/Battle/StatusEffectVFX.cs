using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
    public class StatusEffectVFX : MonoBehaviour
    {
        public enum TurnBehaviour{
            ShowLongest,
            ShowShortest,
            ShowOldest,
            ShowNewest
        }
        [SerializeField] private TurnBehaviour visualBehaviour = TurnBehaviour.ShowLongest;
        public TurnBehaviour VisualBehaviour { get => visualBehaviour; }
        [Tooltip("If the value is set to Default, the position is not changed, otherwise, it will be set to the selected position automatically")]
        [SerializeField] private SkillVFX.TargetPosition position = SkillVFX.TargetPosition.Default;
        public SkillVFX.TargetPosition Position { get => position; }
        // Ideia temporariamente descartada, não sei como fazer na maquina de estados
        // [Tooltip("Should the remove animation be displayed whenever an effect ends even if a similar one is still in effect?")]
        // [SerializeField] private bool alwaysShowRemove = false;
        // public bool AlwaysShowRemove { get => alwaysShowRemove; }

        private List<ParticleSystem> particleSystems;
        // Referencias para os componentes que precisam ser encontradas muitas vezes
        // A propriedade é usada para garantir que só vai chamar getcomponent uma vez
        private Animator anim = null;
        private Animator Anim{
            get{
                if(anim == null){
                    anim = GetComponent<Animator>();
                }
                return anim;
            }
        }
        private SpriteRenderer sr = null;
        private SpriteRenderer SRenderer{
            get{
                if(sr == null){
                    sr = GetComponent<SpriteRenderer>();
                }
                return sr;
            }
        }

        private bool hidden = true;

        // As propriedades são usadas para só precisar procurar uma vez se o parametro existe
        private bool? hasTurnsParameter = null;
        private bool HasTurnsParameter{
            get{
                if(hasTurnsParameter == null){
                    hasTurnsParameter = System.Array.Find(Anim.parameters, param => param.name == "turnsLeft") != null;
                }
                return hasTurnsParameter ?? false;
            }
        }
        private bool? hasApplyTrigger = null;
        private bool HasApplyTrigger{
            get{
                if(hasApplyTrigger == null){
                    hasApplyTrigger = System.Array.Find(Anim.parameters, param => param.name == "Apply") != null;
                }
                return hasApplyTrigger ?? false;
            }
        }
        private bool? hasUpdateTrigger = null;
        private bool HasUpdateTrigger{
            get{
                if(hasUpdateTrigger == null){
                    hasUpdateTrigger = System.Array.Find(Anim.parameters, param => param.name == "Update") != null;
                }
                return hasUpdateTrigger ?? false;
            }
        }
        private bool? hasRemoveTrigger = null;
        private bool HasRemoveTrigger{
            get{
                if(hasRemoveTrigger == null){
                    hasRemoveTrigger = System.Array.Find(Anim.parameters, param => param.name == "Remove") != null;
                }
                return hasRemoveTrigger ?? false;
            }
        }
        private bool? hasResetTrigger = null;
        private bool HasResetTrigger{
            get{
                if(hasResetTrigger == null){
                    hasResetTrigger = System.Array.Find(Anim.parameters, param => param.name == "Reset") != null;
                }
                return hasResetTrigger ?? false;
            }
        }
        private int turnsLeft = 0;
        public int TurnsLeft {
            get => turnsLeft;
            set{
                if(HasTurnsParameter){
                    Anim.SetInteger("turnsLeft", value);
                }
                turnsLeft = value;
            }
        }

        public void Awake(){
            hidden = true;
            SRenderer.enabled = !hidden;
            particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>(true));
        }

        public void UpdatePosition(BattleUnit unit){
            switch(position){
                case SkillVFX.TargetPosition.Feet:
                    transform.position = unit.transform.position + new Vector3(unit.FeetPosition.x, unit.FeetPosition.y);
                    break;
                case SkillVFX.TargetPosition.Torso:
                    transform.position = unit.transform.position + new Vector3(unit.TorsoPosition.x, unit.TorsoPosition.y);
                    break;
                case SkillVFX.TargetPosition.Head:
                    transform.position = unit.transform.position + new Vector3(unit.HeadPosition.x, unit.HeadPosition.y);
                    break;
                case SkillVFX.TargetPosition.Overhead:
                    transform.position = unit.transform.position + new Vector3(unit.OverheadPosition.x, unit.OverheadPosition.y);
                    break;
            }
        }

        // Deixa o efeito visivel
        public void Show(){
            hidden = false;
            SRenderer.enabled = true;
        }

        // Esconde o efeito visual
        public void Hide(){
            hidden = true;
            SRenderer.enabled = false;
            StopAllParticles();
        }

        public void UpdateTrigger(){
            if(HasUpdateTrigger){
                Debug.Log("Chamou o trigger de animação Update");
                Anim.SetTrigger("Update");
            }
        }

        public void ApplyTrigger(){
            if(HasApplyTrigger){
                Debug.Log("Chamou o trigger de animação Apply");
                Anim.SetTrigger("Apply");
            }
        }

        public void ResetTrigger(){
            if(HasResetTrigger){
                Debug.Log("Chamou o trigger de animação Reset");
                Anim.SetTrigger("Reset");
            }
        }

        public void RemoveTrigger(){
            if(HasRemoveTrigger){
                Debug.Log("Chamou o trigger de animação Remove");
                Anim.SetTrigger("Remove");
            }
        }

        void StartParticle(ParticleSystem particle){
            if(particle != null && !particle.isPlaying){
                particle.Play(true);
            }
        }

        void RestartParticle(ParticleSystem particle){
            if(particle != null){
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particle.Play(true);
            }
        }

        void StopParticle(ParticleSystem particle){
            if(particle != null){
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        void StopAllParticles(){
            foreach(ParticleSystem particle in particleSystems){
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }
}