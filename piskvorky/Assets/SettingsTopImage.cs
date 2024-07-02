using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SettingsTopImage : MonoBehaviour
{
    private Image imageComponent;

    [SerializeField]
    private Sprite aiEasySprite;

    [SerializeField]
    private Sprite aiHardSprite;

    [SerializeField]
    private Sprite localGameSprite;

    [SerializeField]
    private Sprite onlineGameSprite;

    private Dictionary<AIDifficulty, Sprite> aiDifficultyIntoSprite = new Dictionary<AIDifficulty, Sprite>();




    private void Awake()
    {
        aiDifficultyIntoSprite.Add(AIDifficulty.EASY, aiEasySprite);
        aiDifficultyIntoSprite.Add(AIDifficulty.HARD, aiHardSprite);

        imageComponent = GetComponent<Image>();
    }

    private void OnEnable()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        imageComponent.sprite = PickCorrectSprite();
    }

    private Sprite PickCorrectSprite()
    {
        GameSetupValues setupValues = GameSetupValues.INSTANCE;

        if(setupValues.IsOnline)
        {
            return onlineGameSprite;
        }

        if(setupValues.PlayingAgainstAi)
        {
            return aiDifficultyIntoSprite[setupValues.AiDifficulty];
        }

        return localGameSprite;
    }
}
