using UnityEngine;

namespace DragonBall.Bullet
{
    [CreateAssetMenu(fileName = "BulletScriptableObject", menuName = "Player/BulletScriptableObject")]
    public class BulletScriptableObject : ScriptableObject
    {
        [Header("Bullet Properties")]
        public float BulletSpeed = 15f;
        public int BulletDamage = 25;
        public float BulletLifetime = 2f;
        public float BulletCooldown = 0.5f;
    }
}