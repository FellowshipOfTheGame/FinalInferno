using System.Collections.Generic;
using FinalInferno.UI.AII;
using FinalInferno.UI.Battle.QueueMenu;
using FinalInferno.UI.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.SkillMenu {
    /// <summary>
    /// Componente que reproduz o comportamento do menu de skills, responsável pela visualização das habilidades
    /// especiais do heroi que está em seu turno, assim permitir visualizar detalhes de cada habilidade, 
    /// como custo, tipo de alvo e efeitos aplicados por ela.
    ///</summary>
    public class SkillList : MonoBehaviour {
        [Header("Prefabs")]
        /// <summary>
        /// Objeto template para um item de skill que será mostrado no menu.
        /// </summary>
        [SerializeField] private GameObject skillObject;

        /// <summary>
        /// Objeto template para um item de efeito de skill que será mostrado no menu.
        /// </summary>
        [SerializeField] private GameObject effectObject;

        [Header("Contents references")]
        /// <summary>
        /// Referência para o local onde todas as skills serão armazenadas e mostradas para o jogador.
        /// </summary>
        [SerializeField] private RectTransform skillsContent;

        [SerializeField] private RectTransform viewportRect;

        /// <summary>
        /// Referência para o local onde os efeitos da skill selecionada serão armazenados 
        /// e mostrados para o jogador.
        /// </summary>
        [SerializeField] private RectTransform effectsContent;


        [Header("Managers")]
        /// <summary>
        /// Controlador dos itens de skill.
        /// </summary>
        [SerializeField] private AIIManager manager;

        /// <summary>
        /// Controlador dos itens de efeito.
        /// </summary>
        [SerializeField] private AIIManager effectsManager;

        [Header("UI elements")]
        /// <summary>
        /// Campo de texto onde ficará o nome da skill selecionada.
        /// </summary>
        [SerializeField] private Text skillNameText;

        /// <summary>
        /// Campo de texto onde ficará a descrição da skill selecionada.
        /// </summary>
        [SerializeField] private Text descriptionText;

        /// <summary>
        /// Campo de texto onde ficará o custo da skill selecionada.
        /// </summary>
        [SerializeField] private Text costText;

        /// <summary>
        /// Campo de texto onde ficará a descrição do efeito selecionado.
        /// </summary>
        [SerializeField] private Text effectDescriptionText;

        [SerializeField] private Image skillIconImage;

        /// <summary>
        /// Imagem que mostrará o elemento da skill.
        /// </summary>
        [SerializeField] private Image elementImage;

        /// <summary>
        /// Imagem que mostrará o tipo de alvo da skill.
        /// </summary>
        [SerializeField] private Image targetTypeImage;

        [Header("Click decision")]
        /// <summary>
        /// Decisão que será chamada quando a tecla de ativação for pressionada.
        /// </summary>
        [SerializeField] private ButtonClickDecision clickDecision;

        // private bool shouldActivate = false;

        /// <summary>
        /// Carrega todas as skills ativas do heroi que está em seu turno no menu de skills.
        /// </summary>
        /// <param name="skills"> Lista com todas as skills do heroi. </param>
        public void UpdateSkillsContent(List<Skill> skills) {
            // Deleta todos os itens previamente alocados no content
            foreach (SkillElement element in skillsContent.GetComponentsInChildren<SkillElement>()) {
                Destroy(element.gameObject);
            }

            // Variável auxiliar para a ordenação dos itens
            AxisInteractableItem lastItem = null;

            // Passa por todas as skills da lista, adicionando as ativas no menu e as ordenando
            foreach (PlayerSkill skill in skills) {
                if (skill.active) {
                    // Instancia um novo item e o coloca no content
                    GameObject newSkill = Instantiate(skillObject, skillsContent);
                    newSkill.GetComponent<SkillElement>().skill = skill;

                    // Espera que o objeto tenha um filho Icon e um filho Text
                    newSkill.transform.Find("Icon").GetComponent<Image>().sprite = skill.skillImage;
                    newSkill.transform.Find("Text").GetComponent<Text>().text = skill.name;

                    // Define este script como responsável pelo item criado
                    SkillListItem newSkillListItem = newSkill.GetComponent<SkillListItem>();
                    newSkillListItem.skillList = this;

                    // Adiciona a decisão de clique no item criado
                    ClickableItem newClickableItem = newSkill.GetComponent<ClickableItem>();
                    newClickableItem.buttonClickDecision = clickDecision;

                    // Adiciona a skill no item que a ativará
                    SkillItem newSkillItem = newSkill.GetComponent<SkillItem>();
                    newSkillItem.skill = skill;

                    // Ordena o item na lista
                    AxisInteractableItem newItem = newSkill.GetComponent<AxisInteractableItem>();
                    if (lastItem != null) {
                        newItem.upItem = lastItem;
                        lastItem.downItem = newItem;
                    } else {
                        manager.firstItem = newItem;
                        // BattleSkillManager.currentSkill = skill;
                        // UpdateSkillDescription(skill);
                    }
                    lastItem = newItem;
                }
            }
            // shouldActivate = true;
        }

        // Função e variavel auxiliar foram feitas para permitir chamar isso por animação
        // Dava erro pq ou os objetos criados não tinham chamado awake ainda ou o objeto SkillList estava desativado
        public void ActivateManager() {
            manager.Activate();
            // shouldActivate = false;
        }

        /// <summary>
        /// Mostra detalhes da skill selecionada no menu.
        /// </summary>
        /// <param name="skill"> Skill para ser mostrada no menu. </param>
        public void UpdateSkillDescription(Skill skill) {
            // Mostra as informações básicas da skill
            skillNameText.text = skill.name;

            costText.text = skill.cost.ToString();
            descriptionText.text = skill.ShortDescription;

            if (skill is PlayerSkill) {
                skillIconImage.enabled = true;
                skillIconImage.sprite = (skill as PlayerSkill).skillImage;
            } else {
                skillIconImage.enabled = false;
            }
            elementImage.sprite = Icons.instance.elementSprites[(int)skill.attribute - 1];
            targetTypeImage.sprite = Icons.instance.targetTypeSprites[(int)skill.target];

            // Atualiza a lista de efeitos
            UpdateEffectsContent(skill.effects);
            if (skill is PlayerSkill) {
                effectsManager.Activate();
            }
        }

        public Skill GetFirstSkill() {
            if (manager != null && manager.firstItem != null) {
                return manager.firstItem.GetComponent<SkillElement>().skill;
            }
            return null;
        }

        /// <summary>
        /// Atualiza a posição do content para melhor visualização da lista.
        /// </summary>
        /// <param name="currentTrans"> Transform do item selecionado. </param>
        public void ClampSkillContent(RectTransform currentTrans) {
            // Salva a posição vertical do item em relação ao content
            float itemPos = currentTrans.localPosition.y;
            float curPos = skillsContent.localPosition.y;
            float itemHeight = currentTrans.rect.height;
            float viewportHeight = viewportRect.rect.height;

            // Adapta a posição vertical do content para melhor visualização da lista
            skillsContent.localPosition = new Vector3(skillsContent.localPosition.x,
                                        Mathf.Clamp(skillsContent.localPosition.y, -viewportHeight - itemPos + itemHeight / 2, -itemPos - itemHeight / 2));
        }

        /// <summary>
        /// Carrega todos os efeitos aplicados pela skill selecionada.
        /// </summary>
        /// <param name="effects"> Lista com todos os efeitos aplicados pela skill. </param>
        private void UpdateEffectsContent(List<SkillEffectTuple> effects) {
            // Deleta todos os itens previamente alocados no content
            foreach (EffectElement EE in effectsContent.GetComponentsInChildren<EffectElement>()) {
                Destroy(EE.gameObject);
            }

            // Variável auxiliar para a ordenação dos itens
            AxisInteractableItem previousItem = null;

            // Passa por todas os efeitos da lista, adicionando no menu e ordenando
            foreach (SkillEffectTuple effect in effects) {
                // Instancia um novo item e o coloca no content
                GameObject newEffect = Instantiate(effectObject, effectsContent);
                newEffect.GetComponent<EffectElement>().SetEffect(effect);

                // Define este script como responsável pelo item criado
                EffectListItem newEffectItem = newEffect.GetComponent<EffectListItem>();
                newEffectItem.skillList = this;

                // Ordena o item na lista
                AxisInteractableItem newItem = newEffect.GetComponent<AxisInteractableItem>();
                if (previousItem != null) {
                    newItem.leftItem = previousItem;
                    previousItem.rightItem = newItem;
                } else {
                    effectsManager.firstItem = newItem;
                }
                previousItem = newItem;
                effectsManager.lastItem = newItem;
            }
            if (effects.Count > 0) {
                effects[0].UpdateValues();
                UpdateEffectDescription(effects[0].effect);
            }
        }

        /// <summary>
        /// Atualiza a descrição do efeito selecionado.
        /// </summary>
        /// <param name="effects"> Efeito selecionado. </param>
        public void UpdateEffectDescription(SkillEffect effect) {
            effectDescriptionText.text = (effect.DisplayName != null && effect.DisplayName != "") ? effect.DisplayName : effect.name;
        }

    }

}