using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fog.Dialogue{
    [RequireComponent(typeof(Collider2D))]
    public class Agent : MonoBehaviour
    {

        // Singleton
        private static Agent instance = null;
        public void Awake() {
            if (!instance) {
                instance = this;
            } else
                Destroy(this);
        }

        [SerializeField]
        private int maxInteractions;
        [HideInInspector]
        public bool canInteract;
        private bool isProcessingInput;

        void Reset(){
            maxInteractions = 1;
        }

        // Start is called before the first frame update
        void Start()
        {
            canInteract = true;
            isProcessingInput = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Esse botao precisa ser declarado nos inputs do projeto
            if (Input.GetButtonDown("Interact")) {
                Debug.Log("entrou1");
                if (!isProcessingInput && canInteract) {
                    Debug.Log("entrou2");
                    isProcessingInput = true;

                    Collider2D[] colliders = new Collider2D[maxInteractions];
                    ContactFilter2D contactFilter = new ContactFilter2D();
                    contactFilter.useTriggers = true;
                    GetComponent<Collider2D>().OverlapCollider(contactFilter, colliders);
                    foreach (Collider2D col in colliders) {
                        if (col) {
                            Debug.Log("checking");
                            IInteractable interact = col.GetComponent<IInteractable>();
                            if (interact != null)
                                interact.OnInteractAttempt(this, GetComponent<FinalInferno.Movable>());
                        }
                    }

                    isProcessingInput = false;
                }
            }
        }
    }
}
