using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameplayUI : MonoBehaviour {
    public static GameplayUI Instance { get; private set; }
    public List<int> points = new List<int>();
    public Image[] pointsImage;
    public int[] coins;

    public TextMeshProUGUI roundText;
    
    public Image p1Portrait;
    public Image p2Portrait;
    public TextMeshProUGUI p1CoinsText;
    public TextMeshProUGUI p2CoinsText;

    public GameObject GameplayUIC;
    public GameObject WinUI;
    public GameObject p1WinText;
    public GameObject p2WinText;

    public GameObject p1CooldownComponent;
    public GameObject p2CooldownComponent;

    public float p1Cooldown = 0f;
    public float p2Cooldown = 0f;

    void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
    
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        WinUI.SetActive(false);

        coins[0] = 0;
        coins[1] = 0;
    }

    void OnEnable() {
        BolinhaController.OnUpdatePortrait += UpdatePortrait;

        BolinhaController.OnCoinCollect += UpdateCoin;
        BolinhaController.OnDefeat += UpdateScore;
    }

    void OnDisable() {
        BolinhaController.OnUpdatePortrait -= UpdatePortrait;

        BolinhaController.OnCoinCollect -= UpdateCoin;
        BolinhaController.OnDefeat -= UpdateScore;
    }

    private void UpdatePortrait(int playerId, Sprite _sprite) {
        if(playerId == 0) {
            p1Portrait.sprite = _sprite;
        } else {
            p2Portrait.sprite = _sprite;
        }
    }
 
    private void UpdateScore(int playerId) {
        int _playerWhoScored = 0;

        if(playerId == 0) { _playerWhoScored = 1;
        } else { _playerWhoScored = 0; }

        points.Add(_playerWhoScored);

        Debug.Log($"{points[0]}");
        UpdateScoreImages();

        StartCoroutine(RestartScene());
    }

    private void UpdateScoreImages() {
        int p1Points = 0;
        int p2Points = 0;
        
        for(int i = 0; i < pointsImage.Length; i++) {

            pointsImage[i].color = Color.white;
            
            if(i < points.Count) {
                if(points[i] == 0) {
                    p1Points++;
                    pointsImage[i].color = Color.red;
                } else {
                    p2Points++;
                    pointsImage[i].color = Color.blue;
                }
            }
        }
        
        // para o tempo caso alguém vença
        if(p1Points == 2) {
            WinUI.SetActive(true);
            GameplayUIC.SetActive(false);
            p2WinText.SetActive(false);

            Time.timeScale = 0f;
        } else if (p2Points == 2) {
            WinUI.SetActive(true);
            GameplayUIC.SetActive(false);
            p1WinText.SetActive(false);

            Time.timeScale = 0f;
        }
    }

    private void UpdateCoin(int playerId, int _coins) {
        coins[playerId] = _coins;

        UpdateCoinText();
    }

    private void UpdateCoinText() {
        p1CoinsText.text = $"MOEDAS: 0{coins[0]}";
        p2CoinsText.text = $"MOEDAS: 0{coins[1]}";
    }

    public void GoToMainMenu(string cena) {
        Destroy(gameObject);
        Time.timeScale = 1f;
        GameManager.Instance?.mudarCena(cena);
    }

    IEnumerator RestartScene() {
        yield return new WaitForSeconds(1f);

        GameManager.Instance?.reiniciarCena();
    }
}
