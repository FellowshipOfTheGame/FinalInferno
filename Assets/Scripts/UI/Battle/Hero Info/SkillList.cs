using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinalInferno.UI.AII;
using FinalInferno.UI.FSM;
using FinalInferno.UI.Battle.QueueMenu;

namespace FinalInferno.UI.Battle.SkillMenu
{
    /// <summary>
    /// Componente que reproduz o comportamento do menu de skills, responsável pela visualização das habilidades
    /// especiais do heroi que está em seu turno, assim permitir visualizar detalhes de cada habilidade, 
    /// como custo, tipo de alvo e efeitos aplicados por ela.
    ///</summary>
    public class SkillList : MonoBehaviour
    {
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

        /// <summary>
        /// Carrega todas as skills ativas do heroi que está em seu turno no menu de skills.
        /// </summary>
        /// <param name="skills"> Lista com todas as skills do heroi. </param>
        public void UpdateSkillsContent(List<Skill> skills)
        {
            // Deleta todos os itens previamente alocados no content
            foreach (SkillElement element in skillsContent.GetComponentsInChildren<SkillElement>())
            {
                Destroy(element.gameObject);
            }

            // Variável auxiliar para a ordenação dos itens
            AxisInteractableItem lastItem = null;

            // Passa por todas as skills da lista, adicionando as ativas no menu e as ordenando
            foreach (PlayerSkill skill in skills)
            {
                if (skill.active)
                {
                    // Instancia um novo item e o coloca no content
                    GameObject newSkill = Instantiate(skillObject);
                    newSkill.GetComponent<SkillElement>().skill = skill;
                    newSkill.transform.SetParent(skillsContent);

                    // Define este script como responsável pelo item criado
                    SkillListItem newSkillListItem = newSkill.GetComponent<SkillListItem>();
                    newSkillListItem.skillList = this;

                    // Adiciona a decisão de clique no item criado
                    ClickableItem newClickableItem = newSkill.GetComponent<ClickableItem>();
                    newClickableItem.BCD = clickDecision;

                    // Adiciona a skill no item que a ativará
                    SkillItem newSkillItem = newSkill.GetComponent<SkillItem>();
                    newSkillItem.skill = skill;

                    // Ordena o item na lista
                    AxisInteractableItem newItem = newSkill.GetComponent<AxisInteractableItem>();
                    if (lastItem != null)
                    {
                        newItem.positiveItem = lastItem;
                        lastItem.negativeItem = newItem;
                    }
                    else
                    {
                        manager.firstItem = newItem;                    
                    }
                    lastItem = newItem;
                }
            }
        }

        /// <summary>
        /// Mostra detalhes da skill selecionada no menu.
        /// </summary>
        /// <param name="skill"> Skill para ser mostrada no menu. </param>
        public void UpdateSkillDescription(PlayerSkill skill)
        {
            // Mostra as informações básicas da skill
            skillNameText.text = skill.name;

            costText.text = skill.cost.ToString();            
            descriptionText.text = skill.description;

            elementImage.sprite = Icons.instance.elementSprites[(int) skill.attribute];
            targetTypeImage.sprite = Icons.instance.targetTypeSprites[(int) skill.target];

            // Atualiza a lista de efeitos
            UpdateEffectsContent(skill.effects);
            effectsManager.Active();
        }

        /// <summary>
        /// Atualiza a posição do content para melhor visualização da lista.
        /// </summary>
        /// <param name="currentTrans"> Transform do item selecionado. </param>
        public void ClampSkillContent(RectTransform currentTrans)
        {
            // Salva a posição vertical do item em relação ao content
            float itemPos = currentTrans.localPosition.y;

            // Adapta a posição vertical do content para melhor visualização da lista
            skillsContent.localPosition = new Vector3(skillsContent.localPosition.x, 
                                        Mathf.Clamp(skillsContent.localPosition.y, -itemPos-239, -itemPos-92));
        }

        /// <summary>
        /// Carrega todos os efeitos aplicados pela skill selecionada.
        /// </summary>
        /// <param name="effects"> Lista com todos os efeitos aplicados pela skill. </param>
        private void UpdateEffectsContent(List<SkillEffectTuple> effects)
        {
            // Deleta todos os itens previamente alocados no content
            foreach (EffectElement EE in effectsContent.GetComponentsInChildren<EffectElement>())
            {
                Destroy(EE.gameObject);
            }

            // Variável auxiliar para a ordenação dos itens
            AxisInteractableItem lastItem = null;

            // Passa por todas os efeitos da lista, adicionando no menu e ordenando
            foreach (SkillEffectTuple effect in effects)
            {
                // Instancia um novo item e o coloca no content
                GameObject newEffect = Instantiate(effectObject);
                newEffect.GetComponent<EffectElement>().UpdateEffect(effect);
                newEffect.transform.SetParent(effectsContent);

                // Define este script como responsável pelo item criado
                EffectListItem newEffectItem = newEffect.GetComponent<EffectListItem>();
                newEffectItem.skillList = this;

                // Ordena o item na lista
                AxisInteractableItem newItem = newEffect.GetComponent<AxisInteractableItem>();
                if (lastItem != null)
                {
                    newItem.negativeItem = lastItem;
                    lastItem.positiveItem = newItem;
                }
                else
                {
                    effectsManager.firstItem = newItem;
                }
                lastItem = newItem;
            }
        }

        /// <summary>
        /// Atualiza a descrição do efeito selecionado.
        /// </summary>
        /// <param name="effects"> Efeito selecionado. </param>
        public void UpdateEffectDescription(SkillEffect effect)
        {
            // effectDescriptionText.text = effect.description;
        }

    }

}