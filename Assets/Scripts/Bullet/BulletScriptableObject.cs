using UnityEngine;

namespace DragonBall.Bullet
{
    [CreateAssetMenu(fileName = "BulletScriptableObject", menuName = "Bullet/BulletScriptableObject")]
    public class BulletScriptableObject : ScriptableObject
    {
        [Header("Bullet Properties")]
        public float BulletSpeed = 15f;
        public int BulletDamage = 25;
        public float BulletLifetime = 2f;
    }
}