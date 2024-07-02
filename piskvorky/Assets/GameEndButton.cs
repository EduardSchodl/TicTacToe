using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GameEndButton : MonoBehaviour
{
    private const string NEXT_ROUND_TEXT = "NEXT ROUND";
    private const string END_SHOWCASE_TEXT = "END SHOWCASE";

    private Button buttonComponent;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();
        buttonComponent.onClick.AddListener(OnEndButtonClick);

        if(GameSetupValues.INSTANCE.IsOnline)
        {
            gameObject.SetActive(false);
        }
    }

    [SerializeField]
    private TMPro.TextMeshProUGUI buttonText;

    public void UpdateText()
    {
        bool matchEnded = MatchInfo.INSTANCE.MatchHasEnded();
        buttonText.text = matchEnded ? END_SHOWCASE_TEXT : NEXT_ROUND_TEXT;
    }

    public void OnEndButtonClick()
    {
        if(GameSetupValues.INSTANCE.IsOnline)
        {
            Debug.LogWarning("You are online.");
            return;
        }

        GameStateManager.INSTANCE.HandleNextRoundCycle();
    }

}
