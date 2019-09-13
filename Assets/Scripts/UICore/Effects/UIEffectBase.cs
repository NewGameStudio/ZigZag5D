using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UICore
{ 
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIEffectBase : MonoBehaviour
    {
        private RectTransform _transform;
        protected new RectTransform transform
        {
            get
            {
                _transform = _transform ?? GetComponent<RectTransform>();

                return _transform;
            }
        }


        protected abstract void Update();
    }
}
