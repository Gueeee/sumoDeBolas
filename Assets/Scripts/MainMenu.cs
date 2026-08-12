using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour {
    
    public TextMeshProUGUI p1StatsText;
    public TextMeshProUGUI p2StatsText;

    void Start() {
        p1StatsText.text = $"P1\nFOR.:07\nVEL.:10";
        p2StatsText.text = $"P1\nFOR.:07\nVEL.:10";
    }

    private void OnEnable() {
        BolinhaButton.OnBolinhaSelect += ChangeBolinhaStatsText;
    }

    private void ChangeBolinhaStatsText(int playerId, BolinhaData bolinhaData) {
        float str = bolinhaData.pushStrength;
        float vel = bolinhaData.speed;

        string strText = "00";
        string velText = "00";

        if(str > 9) strText = $"{str}";
        else strText = $"0{str}";
        
        if(vel > 9) velText = $"{vel}";
        else velText = $"0{vel}";

        if(playerId == 0) {
            p1StatsText.text = $"P1\nFOR.:{strText}\nVEL.:{velText}";
        } else {
            p2StatsText.text = $"P2\nFOR.:{strText}\nVEL.:{velText}";
        }
    }

    public void StartGame() {
        GameManager.Instance?.mudarCena("Gameplay");
    }
}
