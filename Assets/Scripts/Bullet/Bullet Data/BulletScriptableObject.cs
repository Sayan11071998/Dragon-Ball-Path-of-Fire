using UnityEngine;

namespace DragonBall.Bullet.BulletData
{
    [CreateAssetMenu(fileName = "BulletScriptableObject", menuName = "Bullet/BulletScriptableObject")]
    public class BulletScriptableObject : ScriptableObject
    {
        [Header("Bullet Properties")]
        [Range(1f, 50f)]
        public float BulletSpeed = 15f;

        [Range(1, 100)]
        public int BulletDamage = 25;

        [Range(0.5f, 10f)]
        public float BulletLifetime = 2f;
    }
}