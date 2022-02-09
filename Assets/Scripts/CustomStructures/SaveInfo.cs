using UnityEngine;

namespace FinalInferno {
    [System.Serializable]
    public struct SaveInfo {
        [SerializeField] public long xpParty; // exp acumulativa da party
        [SerializeField] public string mapName; // nome do mapa (cena de overworld) atual
        [SerializeField] public QuestInfo[] quest; // Lsta de informações das quests
        //quests de kill
        [SerializeField] public BestiaryEntry[] bestiary;
        [SerializeField] public string[] archetype; // Lista com a ordem dos heroes
        [SerializeField] public int[] hpCur; // hp atual de cada personagem
        [SerializeField] public Vector2[] position; // posição no overworld dos personagens
        [SerializeField] public SkillInfoArray[] heroSkills; // Info de skills
        [SerializeField] public VolumeController.VolumeInfo volumeInfo; // Configuração de volumes do usuario
        [SerializeField] public bool autoSave; // Configuração de autosave do usuario
        [SerializeField] public string version; // Versão do jogo quando o save foi criado
        public bool Equals(SaveInfo other) {
            if (xpParty != other.xpParty) {
                return false;
            }

            if (mapName != other.mapName) {
                return false;
            }

            if (quest != null && other.quest != null) {
                if (quest.Length != other.quest.Length) {
                    return false;
                }

                for (int i = 0; i < quest.Length; i++) {
                    if (quest[i] != other.quest[i]) {
                        return false;
                    }
                }
            } else if (quest != null || other.quest != null) {
                return false;
            }

            if (bestiary != null && other.bestiary != null) {
                if (bestiary.Length != other.bestiary.Length) {
                    return false;
                }
                for (int i = 0; i < bestiary.Length; i++) {
                    if (bestiary[i] != other.bestiary[i]) {
                        return false;
                    }
                }
            } else if (bestiary != null || other.bestiary != null) {
                return false;
            }

            if (archetype != null && other.archetype != null) {
                if (archetype.Length != other.archetype.Length) {
                    return false;
                }

                for (int i = 0; i < archetype.Length; i++) {
                    if (archetype[i] != other.archetype[i]) {
                        return false;
                    }
                }
            } else if (archetype != null || other.archetype != null) {
                return false;
            }

            if (hpCur != null && other.hpCur != null) {
                if (hpCur.Length != other.hpCur.Length) {
                    return false;
                }

                for (int i = 0; i < hpCur.Length; i++) {
                    if (hpCur[i] != other.hpCur[i]) {
                        return false;
                    }
                }
            } else if (hpCur != null || other.hpCur != null) {
                return false;
            }

            if (position != null && other.position != null) {
                if (position.Length != other.position.Length) {
                    return false;
                }

                for (int i = 0; i < position.Length; i++) {
                    if (position[i] != other.position[i]) {
                        return false;
                    }
                }
            } else if (position != null || other.position != null) {
                return false;
            }

            if (heroSkills != null && other.heroSkills != null) {
                if (heroSkills.Length != other.heroSkills.Length) {
                    return false;
                }

                for (int i = 0; i < heroSkills.Length; i++) {
                    if (heroSkills[i] != other.heroSkills[i]) {
                        return false;
                    }
                }
            } else if (heroSkills != null || other.heroSkills != null) {
                return false;
            }

            return true;
        }
    }

}