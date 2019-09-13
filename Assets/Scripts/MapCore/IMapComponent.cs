using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapCore
{
    public interface IMapComponent
    {
        void Initialize();

        void UpdateMap();

        void Restart();
    }
}
