using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalInferno.UI.Battle.LifeMenu
{
    public class EnemyContent : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        private List<UnitLife> lives = new List<UnitLife>();

        private float xPosition = 0f;

        void Update()
        {
            content.localPosition = new Vector3(Mathf.Lerp(content.localPosition.x, xPosition, .15f), 0f);
        }

        public void LoadLives(List<UnitLife> newLives)
        {
            lives = newLives;
        }

        public void ShowAllLives()
        {
            SetContentToPosition(0);
        }

        public void ShowEnemyInfo(BattleUnit enemy)
        {
            for (int i = 0; i < lives.Count; i++)
                if (lives[i].thisUnit == enemy)
                {
                    SetContentToPosition(i+1);
                    return;
                }
            SetContentToPosition(0);
        }

        private void SetContentToPosition(int index)
        {
            xPosition = -index * 324;
        }
    }

}