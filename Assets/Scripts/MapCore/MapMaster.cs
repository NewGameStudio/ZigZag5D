using System;
using System.Linq;
using UnityEngine;

namespace MapCore
{
    public class MapMaster : MonoBehaviour
    {
        private IMapComponent[] _mapComponents;

        private void Awake()
        {
            enabled = false;
        }

        public void Initialize()
        {
            _mapComponents = FindObjectsOfType<Component>()
                .Where(c => c is IMapComponent)
                .Select(c => c as IMapComponent)
                .ToArray();

            if (_mapComponents == null || _mapComponents.Length == 0)
            {
                Debug.Log("MapMaster::no map components found");
                Destroy(this);
                return;
            }

            foreach (var mapComponent in _mapComponents)
                mapComponent.Initialize();

            enabled = true;
        }

        private void Update()
        {
            for (int i = 0; i < _mapComponents.Length; i++)
                _mapComponents[i].UpdateMap();
        }

        public void Restart()
        {
            for (int i = 0; i < _mapComponents.Length; i++)
                _mapComponents[i].Restart();
        }
    }
}
