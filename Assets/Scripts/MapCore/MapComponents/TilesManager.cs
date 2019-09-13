using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace MapCore
{
    public class TilesManager : MonoBehaviour, IMapComponent
    {
        public event Action<Transform> onTileAddedCallback;
        public event Action<Transform> onTilePassed;
        public event Action<Transform> onTileDestroyedCallback;

        [SerializeField] private PlayerController _player;
        [SerializeField] private Transform _tilePrefab;
        [SerializeField] private float _generateDistance = 10;
        [SerializeField] private float _tileFallingDistance = 1f;
        [SerializeField] private Vector3[] _tilesCreatingDirections = new Vector3[]
        {
            new Vector3(0, 0, 1f),
            new Vector3(1f, 0, 0),
            new Vector3(-1f, 0, 0),
            new Vector3(0.5f, 0, 0.5f),
            new Vector3(-0.5f, 0, 0.5f)
        };

        private Transform _previousTile;
        private int _previousDirection;

        private GameObjectsPool _pool;
        private Queue<Transform> _generatedTiles;


        private bool CheckComponents()
        {
            _player = _player ?? FindObjectOfType<PlayerController>();

            if (_player == null)
            {
                Debug.Log("TilesManager::Player not found");
                return false;
            }

            if (_tilePrefab == null)
            {
                Debug.Log("TilesManager::tile prefab is null");
                return false;
            }

            return true;
        }

        private void CreatePlatform()
        {
            Transform platform = _pool.Instantiate().transform;

            _generatedTiles.Enqueue(platform);

            platform.position = _player.transform.position - Vector3.up * (_player.transform.localScale.y + platform.localScale.y / 2);
            platform.localScale = new Vector3(3, 1, 3);

            _previousTile = platform;
            _previousDirection = -1;

            CreateNextTiles(0, 3);
        }

        private void CreateNextTiles(int directionIndex = -1, int count = -1)
        {
            while (directionIndex == _previousDirection || directionIndex == -1)
                directionIndex = Random.Range(0, _tilesCreatingDirections.Length);

            Vector3 direction = _tilesCreatingDirections[directionIndex];
            count = count == -1 ? Random.Range(2, 6) : count;

            for (int i = 0; i < count; i++)
            {
                Transform tile = _pool.Instantiate().transform;

                tile.position = _previousTile.position + (direction * (_previousTile.localScale.x / 2 + 0.5f));
                tile.localScale = Vector3.one;

                _generatedTiles.Enqueue(tile);

                _previousTile = tile;
                _previousDirection = directionIndex;

                onTileAddedCallback?.Invoke(tile);
            }
        }

        private void ComputePassedTiles()
        {
            Transform tile = _generatedTiles.Peek();

            if ((tile.position.z + tile.localScale.x / 2 + _tileFallingDistance) < _player.transform.position.z)
            {
                StartCoroutine(DropTile(_generatedTiles.Dequeue(), 1f));
                onTilePassed?.Invoke(tile);
            }
        }

        private IEnumerator DropTile(Transform tile, float timeInSeconds)
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();

            Vector3 direction = new Vector3(
                Random.Range(-1, 2),
                Random.Range(-1, 2),
                Random.Range(-1, 1)).normalized;

            float time = 0;
            while (time < timeInSeconds)
            {
                tile.transform.position += direction * 12 * Time.deltaTime;

                tile.transform.localScale -= Vector3.one * timeInSeconds * Time.deltaTime;

                time += Time.deltaTime;

                yield return wait;
            }

            _pool.Destroy(tile.gameObject);

            onTileDestroyedCallback?.Invoke(tile);
        }


        public void Initialize()
        {
            if (!CheckComponents())
            {
                enabled = false;
                return;
            }

            _pool = new GameObjectsPool(_tilePrefab.gameObject, 10);
            _generatedTiles = new Queue<Transform>();

            CreatePlatform();

            UpdateMap();
        }

        public void UpdateMap()
        {
            ComputePassedTiles();

            if (Vector3.Distance(_player.transform.position, _previousTile.position) < _generateDistance)
                CreateNextTiles();
        }

        public void Restart()
        {
            int count = _generatedTiles.Count;

            for (int i = 0; i < count; i++)
                _pool.Destroy(_generatedTiles.Dequeue().gameObject);

            CreatePlatform();
        }
    }
}