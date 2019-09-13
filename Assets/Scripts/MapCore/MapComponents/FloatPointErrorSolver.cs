using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapCore
{
    public class FloatPointErrorSolver : MonoBehaviour, IMapComponent
    {
        [SerializeField] private Transform _player;
        [SerializeField] private float _maxFloatValue = 2f;


        private void Solve()
        {
            foreach (var tile in GameObject.FindGameObjectsWithTag("Tile"))
                tile.transform.position -= Vector3.forward * _maxFloatValue;

            _player.transform.position -= Vector3.forward * _maxFloatValue;
        }


        public void Initialize()
        {
            _player = _player ?? FindObjectOfType<PlayerController>().transform;

            if (_player == null)
            {
                Debug.Log("FloatPointErrorSolver::player is null");
                Destroy(this);
                return;
            }
        }

        public void UpdateMap()
        {
            if (_player.position.z > _maxFloatValue)
                Solve();
        }

        public void Restart()
        {
            
        }
    }
}
