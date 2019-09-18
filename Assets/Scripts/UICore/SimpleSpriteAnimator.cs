using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UICore
{
    [RequireComponent(typeof(Image))]
    public class SimpleSpriteAnimator : MonoBehaviour
    {
        [SerializeField] private Frame[] _frames = null;
        [SerializeField] private bool _repeat = true;

        private Image _image;
        private int _currentFrame;
        private float _passedTime;
        private bool _isWorking = false;

        private void Awake()
        {
            if (_frames == null || _frames.Length == 0)
            {
                Debug.Log("SimpleSpriteAnimator::frames count less then 1");
                enabled = false;
                return;
            }

            _image = GetComponent<Image>();

            _currentFrame = 0;
            _passedTime = 0;
            _isWorking = true;

            ApplyFrame();
        }

        private void Update()
        {
            if (!_isWorking)
                return;

            _passedTime += Time.deltaTime;

            if (_passedTime > _frames[_currentFrame].duration)
            {
                _passedTime = 0;

                if (_currentFrame == _frames.Length - 1)
                {
                    if (!_repeat)
                    {
                        _isWorking = false;

                        return;
                    }
                }

                NextFrame();

                ApplyFrame();
            }
        }

        private void NextFrame()
        {
            _currentFrame++;

            if (_currentFrame >= _frames.Length)
                _currentFrame = 0;
        }

        private void ApplyFrame()
        {
            _image.sprite = _frames[_currentFrame].sprite;
        }
    }

    [System.Serializable]
    public struct Frame
    {
        public Sprite sprite;
        public float duration;
    }
}
