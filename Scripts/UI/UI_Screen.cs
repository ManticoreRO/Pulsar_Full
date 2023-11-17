using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace QDS.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]

    public class UI_Screen : MonoBehaviour
    {
        private Animator animator;

        [Header("Main properties")]
        public Selectable m_startSelectable;

        [Header("Screen events")]
        public UnityEvent onScreenStart;
        public UnityEvent onScreenClose;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();

            if (m_startSelectable)
            {
                EventSystem.current.SetSelectedGameObject(m_startSelectable.gameObject);
            }
        }

        public virtual void StartScreen()
        {
            if (onScreenStart != null)
            {
                onScreenStart.Invoke();
            }
            HandleAnimator("show");
        }

        public virtual void CloseScreen()
        {
            if (onScreenClose != null)
            {
                onScreenClose.Invoke();
            }
            HandleAnimator("hide");
        }
        
        void HandleAnimator(string key)
        {
            if (animator)
                animator.SetTrigger(key);
        }
    }
}
