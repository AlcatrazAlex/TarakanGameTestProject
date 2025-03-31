using System;
using System.Collections.Generic;
using TaranaGame.Configs;
using TaranaGame.Pooling;
using TaranaGame.Tarakan;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace TaranaGame.Logic
{
    public class GameManager : ITickable
    {
        private GameSettingsConfig settings;
        private TarakansConfig _tarakansConfig;
        private DiContainer _container;
        private Camera _gameCamera;
        
        private List<TarakanController> _tarakans = new();

        public event Action OnGameEnded;
        public event Action OnGameStarted;
        
        private Dictionary<string, SimplePool<TarakanViewBase>> _pools = new();
        private SimplePool<TarakanController> _tarakanControllersPool;

        public GameManager(TarakansConfig tarakansConfig, DiContainer container, GameSettingsConfig gameSettingsConfig, GameSettings gameSettings)
        {
            _tarakansConfig = tarakansConfig;    
            _container = container;
            _gameCamera = Camera.main;
            _tarakanControllersPool = new SimplePool<TarakanController>(null, _tarakansConfig.TarakanControllerPrefab);
        }
        
        public void StartGame()
        {
            OnGameStarted?.Invoke();
            foreach (var id in _tarakansConfig.StartTarakansKeys)
                SpawnCockroach(id);
        }

        private void OnTarakanReachedPoint()
        {
            foreach (var tarakan in _tarakans)
            {
                _pools[tarakan.Id].ReleaseObject(tarakan.View);
                _tarakanControllersPool.ReleaseObject(tarakan);
            }
            _tarakans.Clear();
            OnGameEnded?.Invoke();
        }

        private TarakanViewBase GetTarakanView(string id)
        {
            if(!_pools.ContainsKey(id))
                _pools.Add(id, new SimplePool<TarakanViewBase>(null, _tarakansConfig.GetTarakanConfig(id)));
            
            return _pools[id].Get();
        }
        
        private void SpawnCockroach(string id)
        {
            var tarakanView = GetTarakanView(id);
            var tarakanController = _tarakanControllersPool.Get();
            tarakanController.OnPointReached = OnTarakanReachedPoint;
            tarakanController.SetView(tarakanView);
            tarakanController.SetId(id);
            tarakanController.transform.position = GetTarakanSpawnPointPosition();
            _container.InjectGameObject(tarakanController.gameObject);
            _tarakans.Add(tarakanController);
            tarakanController.StartTrip();
        }
        
        public void Tick()
        {
            var mousePos = _gameCamera.ScreenToWorldPoint(Input.mousePosition);
            foreach(var tarakan in _tarakans)
            {
                tarakan.UpdateMousePosition(mousePos);
            }
        }

        private Vector3 GetTarakanSpawnPointPosition()
        {
            var screenViewPortPos = new Vector2(Random.Range(0f, 0.3f), Random.Range(0.7f, 1f));
            var worldPos = _gameCamera.ViewportToWorldPoint(screenViewPortPos);
            worldPos.z = 0f;
            return worldPos;
        }
    }
}

