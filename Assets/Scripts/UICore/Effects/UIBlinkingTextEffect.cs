using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UICore
{
    [RequireComponent(typeof(Text))]
    public class UIBlinkingTextEffect : UIEffectBase
    {
        [SerializeField] private Color[] _colors = new Color[] { Color.red, Color.green };
        [SerializeField] private float _colorChangeTime = 0.1f;

        private Text _text;
        private int _currentColor;
        private float _passedTime;

        private void Awake()
        {
            if (_colors == null || _colors.Length < 1)
            {
                Debug.Log("UIBlinkingTextEffect::colors count less then 1");

                enabled = false;

                return;
            }

            _text = GetComponent<Text>();

            _text.color = _colors[0];

            _passedTime = 0;

            NextColor();
        }

        private void NextColor()
        {
            _currentColor++;

            if (_currentColor >= _colors.Length)
                _currentColor = 0;
        }

        protected override void Update()
        {
            _passedTime += Time.deltaTime;

            if (_passedTime > _colorChangeTime)
            {
                NextColor();

                _passedTime = 0;
            }

            int prevColor = _currentColor == 0 ? _colors.Length - 1 : _currentColor - 1;

            _text.color = Color.Lerp(_colors[prevColor], _colors[_currentColor], _passedTime / _colorChangeTime);
        }
    }
}
