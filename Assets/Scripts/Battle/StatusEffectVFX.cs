using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno {
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(SpriteRenderer))]
    public class StatusEffectVFX : MonoBehaviour {
        public enum TurnBehaviour {
            ShowLongest,
            ShowShortest,
            ShowOldest,
            ShowNewest
        }
        private const string TurnsLeftAnimString = "turnsLeft";
        private const string ApplyAnimString = "Apply";
        private const string UpdateAnimString = "Update";
        private const string RemoveAnimString = "Remove";
        private const string ResetAnimString = "Reset";
        [SerializeField] private TurnBehaviour visualBehaviour = TurnBehaviour.ShowLongest;
        public TurnBehaviour VisualBehaviour => visualBehaviour;
        [Tooltip("If the value is set to Default, the position is not changed, otherwise, it will be set to the selected position automatically")]
        [SerializeField] private TargetPosition position = TargetPosition.Default;
        public TargetPosition Position => position;
        private List<ParticleSystem> particleSystems;

        private bool hidden = true;

        private Animator anim = null;
        private Animator Anim {
            get {
                anim = this.GetComponentIfNull(anim);
                return anim;
            }
        }
        private SpriteRenderer spriteRenderer = null;
        private SpriteRenderer SpriteRenderer {
            get {
                spriteRenderer = this.GetComponentIfNull(spriteRenderer);
                return spriteRenderer;
            }
        }

        private bool AnimatorHasParameter(string parameterName) {
            return System.Array.Find(Anim.parameters, param => param.name == parameterName) != null;
        }

        private bool? hasTurnsParameter = null;
        private bool HasTurnsParameter {
            get {
                if (hasTurnsParameter == null)
                    hasTurnsParameter = AnimatorHasParameter(TurnsLeftAnimString);
                return hasTurnsParameter ?? false;
            }
        }

        private bool? hasApplyTrigger = null;
        private bool HasApplyTrigger {
            get {
                if (hasApplyTrigger == null)
                    hasApplyTrigger = AnimatorHasParameter(ApplyAnimString);
                return hasApplyTrigger ?? false;
            }
        }
        private bool? hasUpdateTrigger = null;
        private bool HasUpdateTrigger {
            get {
                if (hasUpdateTrigger == null)
                    hasApplyTrigger = AnimatorHasParameter(UpdateAnimString);
                return hasUpdateTrigger ?? false;
            }
        }
        private bool? hasRemoveTrigger = null;
        private bool HasRemoveTrigger {
            get {
                if (hasRemoveTrigger == null)
                    hasApplyTrigger = AnimatorHasParameter(RemoveAnimString);
                return hasRemoveTrigger ?? false;
            }
        }
        private bool? hasResetTrigger = null;
        private bool HasResetTrigger {
            get {
                if (hasResetTrigger == null)
                    hasApplyTrigger = AnimatorHasParameter(ResetAnimString);
                return hasResetTrigger ?? false;
            }
        }
        private int turnsLeft = 0;
        public int TurnsLeft {
            get => turnsLeft;
            set {
                if (HasTurnsParameter)
                    Anim.SetInteger(TurnsLeftAnimString, value);
                turnsLeft = value;
            }
        }

        public void Awake() {
            hidden = true;
            SpriteRenderer.enabled = false;
            particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>(true));
        }

        public void UpdatePosition(BattleUnit unit) {
            switch (position) {
                case TargetPosition.Feet:
                    transform.position = unit.transform.position + new Vector3(unit.FeetPosition.x, unit.FeetPosition.y);
                    break;
                case TargetPosition.Torso:
                    transform.position = unit.transform.position + new Vector3(unit.TorsoPosition.x, unit.TorsoPosition.y);
                    break;
                case TargetPosition.Head:
                    transform.position = unit.transform.position + new Vector3(unit.HeadPosition.x, unit.HeadPosition.y);
                    break;
                case TargetPosition.Overhead:
                    transform.position = unit.transform.position + new Vector3(unit.OverheadPosition.x, unit.OverheadPosition.y);
                    break;
            }
        }

        public void Show() {
            if (!hidden)
                return;
            hidden = false;
            SpriteRenderer.enabled = true;
        }

        public void Hide() {
            if (hidden)
                return;
            hidden = true;
            SpriteRenderer.enabled = false;
            StopAllParticles();
        }

        public void UpdateTrigger() {
            if (HasUpdateTrigger)
                Anim.SetTrigger(UpdateAnimString);
        }

        public void ApplyTrigger() {
            if (HasApplyTrigger)
                Anim.SetTrigger(ApplyAnimString);
        }

        public void ResetTrigger() {
            if (HasResetTrigger)
                Anim.SetTrigger(ResetAnimString);
        }

        public void RemoveTrigger() {
            if (HasRemoveTrigger)
                Anim.SetTrigger(RemoveAnimString);
        }

        private void StartAllParticles() {
            foreach (ParticleSystem particle in particleSystems) {
                particle.Play(true);
            }
        }

        private void RestartAllParticles() {
            foreach (ParticleSystem particle in particleSystems) {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particle.Play(true);
            }
        }

        private void StopAllParticles() {
            foreach (ParticleSystem particle in particleSystems) {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }
}