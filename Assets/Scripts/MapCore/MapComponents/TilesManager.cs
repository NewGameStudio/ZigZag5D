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
        private Vector3 _previousTilePosition;
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

            platform.localScale = new Vector3(3, 1, 3);
            platform.position = _player.transform.position - Vector3.up * (_player.transform.localScale.y + platform.localScale.y / 2);

            _previousTile = platform;
            _previousTilePosition = platform.position;
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
                Vector3 tileTargetPosition = _previousTilePosition + (direction * (_previousTile.localScale.x / 2 + 0.5f));

                tile.localScale = Vector3.one;

                StartCoroutine(PlaceTile(tile, tileTargetPosition, 0.5f));

                _generatedTiles.Enqueue(tile);

                _previousTile = tile;
                _previousTilePosition = tileTargetPosition;
                _previousDirection = directionIndex;

                onTileAddedCallback?.Invoke(tile);
            }
        }

        private void ComputePassedTiles()
        {
            Transform tile = _generatedTiles.Peek();

            if ((tile.position.z + tile.localScale.x / 2 + _tileFallingDistance) < _player.transform.position.z)
            {
                StartCoroutine(DropTile(_generatedTiles.Dequeue(), 0.8f));
                onTilePassed?.Invoke(tile);
            }
        }


        private IEnumerator PlaceTile(Transform tile, Vector3 targetPosition, float time)
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();

            Vector3 direction = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                1f).normalized;

            Vector3 startPosition = targetPosition + direction * 8;

            float timePassed = 0f;
            while (timePassed <= time)
            {
                tile.transform.position = Vector3.Lerp(startPosition, targetPosition, timePassed / time);
                timePassed += Time.deltaTime;

                yield return wait;
            }

            tile.transform.position = targetPosition;
        }

        private IEnumerator DropTile(Transform tile, float time)
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();

            Vector3 direction = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                -1f).normalized;

            Vector3 startPosition = tile.position;
            Vector3 targetPosition = startPosition + direction * 15;

            float timePassed = 0;
            while (timePassed < time)
            {
                tile.transform.position = Vector3.Lerp(startPosition, targetPosition, timePassed / time);

                timePassed += Time.deltaTime;

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

            if (Vector3.Distance(_player.transform.position, _previousTilePosition) < _generateDistance)
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