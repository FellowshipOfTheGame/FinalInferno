using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RECalculator : MonoBehaviour
{
    public static bool encountersEnabled = true;
    public static List<PlayerSkill> encounterSkils;
    // Tabela de encontros aleatorios pra este mapa (public)
    // To do
    public Transform playerObj;
    public Sprite BattleBG;
    public AudioClip BattleBGM;
    [SerializeField]
    private float baseEncounterRate = 10.0f;
    [SerializeField]
    private float rateIncreaseFactor = 0.05f;
    private float curEncounterRate;
    private Vector2 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        curEncounterRate = baseEncounterRate;
        lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (encountersEnabled) {
            // Calcula a distancia entre a posicao atual e a distance no ultimo update
            float distance = Vector2.Distance(lastPosition, new Vector2(playerObj.position.x, playerObj.position.y));
            if (distance > 0) {
                // Caso o player tenha se movido, verifica se encontrou batalha
                CheckEncounter(distance);
                // Atualiza lastPosition
                lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
            }
        }
    }

    private void CheckEncounter(float distance) {
        // A distancia percorrida e usada para aumentar/diminuir a chance de encontro
        if (Random.Range(0.0f, 100.0f) < curEncounterRate * (distance)) {
            // Quando encontrar uma batalha
            Debug.Log("Found random encounter");
            // Diminui a taxa de encontro para metade do valor base
            // Isso reduz a chance de batalhas consecutivos
            curEncounterRate = baseEncounterRate/2;
            // Salvar a posição atual de cada player dentro do seu respectivo SO
            // Usar a tabela de encontros aleatorios para definir a lista de inimigos
            // To do
            SceneLoader.LoadBattleScene(new List<Enemy>(), new int[0], BattleBG, BattleBGM);
        } else {
            // Caso nao encontre uma batalha
            Debug.Log("Did not find random encounter");
            // Aumenta a chance de encontro linearmente
            curEncounterRate += rateIncreaseFactor;
        }
    }
}
