using UnityEngine;

namespace DragonBall.Core
{
    public static class GameStateUtility
    {
        public const string SUPER_SAIYAN_KEY = "IsSuperSaiyan";
        public const string DRAGON_BALL_COUNT_KEY = "DragonBallCount";

        public static void ResetPlayerState()
        {
            PlayerPrefs.DeleteKey(SUPER_SAIYAN_KEY);
            PlayerPrefs.DeleteKey(DRAGON_BALL_COUNT_KEY);
            PlayerPrefs.Save();
        }

        public static void SavePlayerState(bool isSuperSaiyan, int dragonBallCount)
        {
            PlayerPrefs.SetInt(SUPER_SAIYAN_KEY, isSuperSaiyan ? 1 : 0);
            PlayerPrefs.SetInt(DRAGON_BALL_COUNT_KEY, dragonBallCount);
            PlayerPrefs.Save();
        }
    }
}