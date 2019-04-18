using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroLife : MonoBehaviour
{
    [SerializeField] private HeroesLives manager;
    private BattleUnit thisHero;
    public Text heroText;

    void Awake()
    {
        manager.OnUpdate += UpdateHeroLife;
    }

    public void UpdateHeroLife()
    {
        heroText.text = thisHero.unit.name + " - " + thisHero.curHP + "/" + thisHero.unit.hpMax;
        heroText.color = thisHero.unit.color;
    }
}
