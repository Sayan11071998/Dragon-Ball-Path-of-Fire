using System.Collections.Generic;
using UnityEngine;
using DragonBall.Utilities;

namespace DragonBall.Enemy
{
    public class EnemyService
    {
        public static EnemyService Instance { get; private set; }

        private readonly Dictionary<EnemyType, (EnemyView prefab, EnemyScriptableObject so)> configs;
        private readonly Dictionary<EnemyType, GenericObjectPool<EnemyView>> pools;
        private readonly List<EnemyController> controllers;

        public EnemyService(Dictionary<EnemyType, (EnemyView, EnemyScriptableObject)> enemyConfigs)
        {
            Instance = this;
            configs = enemyConfigs;
            pools = new Dictionary<EnemyType, GenericObjectPool<EnemyView>>();
            controllers = new List<EnemyController>();

            foreach (var kvp in configs)
            {
                var type = kvp.Key;
                var prefab = kvp.Value.prefab;
                pools[type] = new EnemyPool(prefab);
            }
        }

        public void Update()
        {
            for (int i = controllers.Count - 1; i >= 0; i--)
                controllers[i].Update();
        }

        public void SpawnEnemy(EnemyType type, Vector3 position)
        {
            var pool = pools[type];
            var view = pool.GetItem<EnemyView>();
            view.gameObject.SetActive(true);

            var data = configs[type].so;
            var model = new EnemyModel(data);
            var controller = new EnemyController(model, view);
            controller.Initialize(position);
            controllers.Add(controller);
        }

        public void HandleEnemyDeath(EnemyController controller)
        {
            var view = controller.View;
            view.gameObject.SetActive(false);
            pools[controller.Type].ReturnItem(view);
            controllers.Remove(controller);
        }
    }
}