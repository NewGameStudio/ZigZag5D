using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UICore
{
    public class UIBlinkingEffect : UIEffectBase
    {
        [SerializeField] private Vector2 _deltaSize = Vector3.one * 0.1f;
        [SerializeField] private float _blinkingTime = 0.2f;

        private Vector2 _baseSize;
        private Vector2 _maxSize;

        private bool _isIncrease;
        private float _currentTime;


        private void Awake()
        {
            _baseSize = transform.localScale;
            _maxSize = _baseSize + _deltaSize;

            _isIncrease = true;
            _currentTime = 0;
        }

        protected override void Update()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _blinkingTime)
            {
                _currentTime = 0;
                _isIncrease = !_isIncrease;
            }

            if (_isIncrease)
                transform.localScale = _baseSize + _deltaSize * (_currentTime / _blinkingTime);

            else
                transform.localScale = _maxSize - _deltaSize * (_currentTime / _blinkingTime);
        }
    }
}
