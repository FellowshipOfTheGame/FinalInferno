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
    private float baseEncounterRate;
    [SerializeField]
    private float rateIncreaseFactor;
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
            float distance = Vector2.Distance(lastPosition, new Vector2(playerObj.position.x, playerObj.position.y));
            if (distance > 0) {
                CheckEncounter(distance);
                lastPosition = new Vector2(playerObj.position.x, playerObj.position.y);
            }
        }
    }

    private void CheckEncounter(float distance) {
        if (Random.Range(0.0f, 100.0f) < curEncounterRate * (distance)) {
            Debug.Log("Found random encounter");
            curEncounterRate = baseEncounterRate/2;
            // Usar a tabela de encontros aleatorios para definir a lista de inimigos
        } else {
            Debug.Log("Did not find random encounter");
            curEncounterRate += rateIncreaseFactor;
        }
    }
}
