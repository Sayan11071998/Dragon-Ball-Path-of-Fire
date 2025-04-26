using UnityEngine;

namespace DragonBall.Bullet
{
    [CreateAssetMenu(fileName = "BulletScriptableObject", menuName = "Bullet/BulletScriptableObject")]
    public class BulletScriptableObject : ScriptableObject
    {
        public float Speed = 10f;
        public int Damage = 5;
        public float Lifetime = 3f;
    }
}