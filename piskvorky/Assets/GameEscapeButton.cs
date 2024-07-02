using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEscapeButton : MonoBehaviour
{
    private Button escapeButton;

    private void Awake()
    {
        escapeButton = GetComponent<Button>();

        escapeButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        IngameUICanvas ingameUICanvas = IngameUICanvas.INSTANCE;
        ingameUICanvas.FullyHideEscapePanel();
        ingameUICanvas.ShowEndgameShowcase();
    }
}
