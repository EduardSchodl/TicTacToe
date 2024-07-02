using System.Collections.Generic;

public static class AiDifficultyIntoValues
{

    private static Dictionary<AIDifficulty, AiDifficultyValues> aiDifficultyIntoValues = new Dictionary<AIDifficulty, AiDifficultyValues>();

    public static AiDifficultyValues GetAiDifficultyValues(AIDifficulty aiDifficulty)
    {
        return aiDifficultyIntoValues[aiDifficulty];
    }

    public static AiDifficultyValues GetAiDifficultyFromSelected()
    {
        return GetAiDifficultyValues(GameSetupValues.INSTANCE.AiDifficulty);
    }

    static AiDifficultyIntoValues()
    {
        aiDifficultyIntoValues.Add(AIDifficulty.EASY, new AiDifficultyValues(99999, 2, 2.0/3.0, 6));
        aiDifficultyIntoValues.Add(AIDifficulty.HARD, new AiDifficultyValues(99999, 2, 1.0, 0));
    }

}
