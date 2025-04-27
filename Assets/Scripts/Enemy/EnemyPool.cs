using System.Collections.Generic;
using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private Dictionary<EnemyType, (EnemyView prefab, EnemyScriptableObject config)> enemyConfigs;

        public EnemyPool(Dictionary<EnemyType, (EnemyView prefab, EnemyScriptableObject config)> configs)
        {
            this.enemyConfigs = configs;
        }

        public EnemyController GetEnemy(EnemyType enemyType)
        {
            if (!enemyConfigs.ContainsKey(enemyType))
            {
                Debug.LogError($"Enemy type {enemyType} not found in enemy configs!");
                return null;
            }

            return GetItem<EnemyController>();
        }

        protected override EnemyController CreateItem<U>()
        {
            foreach (var kvp in enemyConfigs)
            {
                var (prefab, config) = kvp.Value;
                EnemyView enemyView = Object.Instantiate(prefab);

                EnemyModel enemyModel = new EnemyModel(
                    config.Health,
                    config.MoveSpeed,
                    config.AttackPower,
                    config.AttackRange,
                    config.AttackCooldown,
                    config.SensingRange,
                    config.StoppingDistance
                );

                return new EnemyController(enemyModel, enemyView);
            }

            return null;
        }
    }

    public enum EnemyType
    {
        FRIEZA,
        CELL,
        BUU,
    }
}