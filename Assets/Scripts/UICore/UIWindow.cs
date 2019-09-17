using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace UICore
{
    [RequireComponent(typeof(RectTransform))]
    public class UIWindow : MonoBehaviour
    {
        [SerializeField] private Vector3 _hidePosition;
        [SerializeField] private Vector3 _showPosition;
        [SerializeField] private float _speed = 10;

        private RectTransform _transform;
        private new RectTransform transform
        {
            get
            {
                if (_transform == null)
                    _transform = GetComponent<RectTransform>();

                return _transform;
            }
        }

        private bool _isShowing = false;
        public bool isShowing { get; private set; } = false;


        private IEnumerator MoveWindowRoutine(Vector3 targetPosition)
        {
            WaitForEndOfFrame wait = new WaitForEndOfFrame();

            while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, _speed * Time.deltaTime);

                yield return wait;
            }

            transform.localPosition = targetPosition;
        }


        public void Show()
        {
            StopAllCoroutines();

            StartCoroutine(MoveWindowRoutine(_showPosition));

            isShowing = true;
        }

        public void Hide()
        {
            StopAllCoroutines();

            StartCoroutine(MoveWindowRoutine(_hidePosition));

            isShowing = false;
        }


        public void ShowImmediate()
        {
            StopAllCoroutines();

            transform.localPosition = _showPosition;

            isShowing = true;
        }

        public void HideImmediate()
        {
            StopAllCoroutines();

            transform.localPosition = _hidePosition;

            isShowing = false;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(UIWindow))]
    public class UIWindowEditor : Editor
    {
        protected new UIWindow target
        {
            get
            {
                return (UIWindow) base.target;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Show"))
                target.ShowImmediate();

            if (GUILayout.Button("Hide"))
                target.HideImmediate();
        }
    }

#endif
}
