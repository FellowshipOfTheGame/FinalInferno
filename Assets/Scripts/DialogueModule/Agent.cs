using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fog.Dialogue {
    [RequireComponent(typeof(Collider2D))]
    public class Agent : MonoBehaviour {
        // Singleton
        public static Agent Instance { get; private set; } = null;
        public void Awake() {
            if (!Instance) {
                Instance = this;
            } else {
                Destroy(this);
            }

            nFramesCooldown = Mathf.Max(nFramesCooldown, 1);
        }
        public void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }

        [SerializeField]
        private int maxInteractions = 1;
        [SerializeField]
        private int nFramesCooldown = 5;
        private int wait;
        [HideInInspector]
        public bool canInteract;
        private bool isProcessingInput;
        [SerializeField] private InputActionReference interactAction;

        public List<IInteractable> collidingInteractables = new List<IInteractable>();

        // Valores padrão ao ser criado no editor
        private void Reset() {
            maxInteractions = 1;
            nFramesCooldown = 5;
        }

        // Start is called before the first frame update
        private void Start() {
            canInteract = true;
            isProcessingInput = false;
            wait = nFramesCooldown;
        }

        // Update is called once per frame
        private void Update() {
            // Esse botao precisa ser declarado nos inputs do projeto
            if (interactAction.action.triggered && wait <= 0) {
                wait = nFramesCooldown;
                if (!isProcessingInput && canInteract) {
                    isProcessingInput = true;
                    int count = 0;
                    // Quanto o botao e apertado, obtem todos os colliders em contato
                    foreach (IInteractable interactable in collidingInteractables.ToArray()) {
                        // Para cada collider encontrado, tenta interagir se houver o componente necessario
                        if (interactable != null) {
                            interactable.OnInteractAttempt();
                            count++;
                            if (count >= maxInteractions || !canInteract) {
                                break;
                            }
                        } else {
                            collidingInteractables.Remove(interactable);
                        }
                    }
                    isProcessingInput = false;
                }
            }
            wait = (wait <= 0) ? 0 : (wait - 1);
        }

        // Funcao auxiliar para garantir o bloqueio de input apos dialogos
        public void InputCooldown() {
            wait = nFramesCooldown;
        }
    }
}
