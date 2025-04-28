using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        private EnemyController controller;

        public void SetController(EnemyController controllerToSet) => controller = controllerToSet;

        public void SetPosition(Vector3 position) => transform.position = position;

        public EnemyType GetEnemyType() => enemyType;
    }
}