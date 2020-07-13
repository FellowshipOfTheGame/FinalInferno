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
        [SerializeField] private TurnBehaviour visualBehaviour;
        public TurnBehaviour VisualBehaviour { get => visualBehaviour; }
        // Ideia temporariamente descartada, não sei como fazer na maquina de estados
        // [Tooltip("Should the remove animation be displayed whenever an effect ends even if a similar one is still in effect?")]
        // [SerializeField] private bool alwaysShowRemove = false;
        // public bool AlwaysShowRemove { get => alwaysShowRemove; }

        private List<GameObject> particles = new List<GameObject>();
        // Referencias para os componentes que precisam ser encontradas muitas vezes
        // A propriedade é usada para garantir que só vai chamar getcomponent uma vez
        private Animator anim = null;
        private Animator Anim{
            get{
                if(!anim){
                    anim = GetComponent<Animator>();
                }
                return anim;
            }
        }
        private SpriteRenderer sr = null;
        private SpriteRenderer SRenderer{
            get{
                if(!sr){
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
            SRenderer.enabled = !hidden;
        }

        // Deixa o efeito visivel
        public void Show(){
            hidden = false;
            SRenderer.enabled = true;
            foreach(GameObject go in particles){
                ParticleSystemRenderer renderer = go?.GetComponent<ParticleSystemRenderer>();
                if(renderer){
                    renderer.enabled = true;
                }
            }
        }

        // Esconde o efeito visual
        public void Hide(){
            hidden = true;
            SRenderer.enabled = false;
            foreach(GameObject go in particles){
                ParticleSystemRenderer renderer = go?.GetComponent<ParticleSystemRenderer>();
                if(renderer){
                    renderer.enabled = false;
                }
            }
        }

        public void UpdateTrigger(){
            if(HasUpdateTrigger){
                Anim.SetTrigger("Update");
            }
        }

        public void ApplyTrigger(){
            if(HasApplyTrigger){
                Anim.SetTrigger("Apply");
            }
        }

        public void ResetTrigger(){
            if(HasResetTrigger){
                Anim.SetTrigger("Reset");
            }
        }

        public void RemoveTrigger(){
            if(HasRemoveTrigger){
                Anim.SetTrigger("Remove");
            }
        }

        // Função para instanciar um objeto de particula durante a animação
        void CreateParticles(GameObject particle)
        {
            GameObject newParticle = Instantiate(particle, new Vector3(transform.position.x, transform.position.y+(SRenderer.sprite.bounds.size.y/2.0f), transform.position.z), transform.rotation, this.transform);
            particles.Add(newParticle);
            ParticleSystemRenderer renderer = newParticle?.GetComponent<ParticleSystemRenderer>();
            if(renderer){
                renderer.enabled = !hidden;
                renderer.sortingLayerID = SRenderer.sortingLayerID;
                renderer.sortingLayerName = SRenderer.sortingLayerName;
                renderer.sortingOrder = SRenderer.sortingOrder;
            }
        }

        // Função para destruir as particulas criadas
        void DestroyParticles(){
            foreach(GameObject particle in particles){
                if(particle){
                    Destroy(particle);
                }
            }
        }

        // Função para destruir o objeto inteiro
        public void DestroyVFX(){
            DestroyParticles();
            Destroy(gameObject);
        }
    }
}