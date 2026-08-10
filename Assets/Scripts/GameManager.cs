using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public enum STATES {
        START,
        MAIN_MENU,
        GAMEPLAY
    }

    public STATES current_state = STATES.START;
    
    public BolinhaData P1;
    public BolinhaData P2;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
    
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        mudarCena("MainMenu");
    }

    void Update() {
        switch(current_state) {
            case STATES.GAMEPLAY:
                // Carrega a UI junto com a fase
                if(!IsSceneLoaded("GameplayUI")) {
                    SceneManager.LoadScene("GameplayUI", LoadSceneMode.Additive);
                }
            break;
        }
    }

    void OnEnable() {
        BolinhaButton.OnBolinhaSelect += ChangeBall;
    }

    void Disable() {
        BolinhaButton.OnBolinhaSelect -= ChangeBall;
    }

    public void ChangeBall(int player, BolinhaData bolinha) {
        if(player == 0) P1 = bolinha;
        else P2 = bolinha;
    }

    public void mudarCena(string cena) {
        SceneManager.LoadScene(cena);
        mudarState(cena);
    }

    public void reiniciarCena() {
        string nomeDaCena = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(nomeDaCena);
        mudarState(nomeDaCena);
    }

    // Muda o 'current_state' e coloca os códigos de inicialização de Scenes
    public void mudarState(string cena) {
        string stateText = "";

        switch(cena) {
            case "Splash":
                current_state = STATES.START;
                stateText = "STATES.START";
            break;

            case "MainMenu":
                current_state = STATES.MAIN_MENU;
                stateText = "STATES.MAINMENU";
            break;

            case "Gameplay":
                current_state = STATES.GAMEPLAY;
                
                stateText = "STATES.GAMEPLAY";
            break;

            default:
            break;
        }
    }

    public bool IsSceneLoaded(string sceneName) {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }
}
