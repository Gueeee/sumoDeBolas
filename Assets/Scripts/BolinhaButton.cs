using UnityEngine;
using UnityEngine.UI;
using System;

public class BolinhaButton : MonoBehaviour {
    public static event Action<int, BolinhaData> OnBolinhaSelect;

    public int playerId;
    public BolinhaData myBolinha;
    private bool selected = false;
    private Button myButton;

    private void Start() {
        myButton = GetComponent<Button>();
    }

    void Update() {
         if(playerId == 0) {
            BolinhaData _P1 = GameManager.Instance?.P1;

            if(_P1 == myBolinha) selected = true;
            else selected = false;
        } else {
            BolinhaData _P2 = GameManager.Instance?.P2;

            if(_P2 == myBolinha) selected = true;
            else selected = false;
        }

        if(selected) myButton.interactable = false;
        else myButton.interactable = true;
    }

    public void ChooseBall() {
        OnBolinhaSelect?.Invoke(playerId, myBolinha);
    }
}
