using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace QDS.UI
{
    public class UI_System : MonoBehaviour
    {
        [Header("Main properties")]
        public UI_Screen m_StartScreen;

        [Header("System events")]
        public UnityEvent onSwitchScreen = new UnityEvent();

        [Header("Fader properties")]
        public Image m_Fader;
        public float m_fadeInDuration = 1.0f;
        public float m_fadeOutDuration = 0.5f;
        
        private Component[] screens = new Component[0];
        private UI_Screen currentScreen;
        private UI_Screen previousScreen;

        #region Getters
        public UI_Screen CurrentScreen()
        {
            return currentScreen;
        }

        public UI_Screen PreviousScreen()
        {
            return previousScreen;
        }
        #endregion

        #region Start
        // Use this for initialization
        void Start()
        {
            // Get all screens that is a child of UI system (active and inactive)
            screens = GetComponentsInChildren<UI_Screen>(true);

         
            if (m_StartScreen)
            {
                SwitchScreen(m_StartScreen);
            }

            if (m_Fader)
            {
                m_Fader.gameObject.SetActive(true);
            }

            FadeIn();

            InitializeScreens();
        }

        void InitializeScreens()
        {
            foreach (var screen in screens)
            {
                screen.gameObject.SetActive(true);
            }
        }
        #endregion

        #region Helper functions
        public void SwitchScreen(UI_Screen aScreen)
        {
            if (aScreen)
            {
                if (currentScreen)
                {
                    currentScreen.CloseScreen();
                    previousScreen = currentScreen;
                }

                currentScreen = aScreen;
                currentScreen.gameObject.SetActive(true);
                currentScreen.StartScreen();
            }
            
            if (onSwitchScreen != null)
            {
                // the screen was changed.... tell that to the system
                onSwitchScreen.Invoke();
            }
        }

        public void GoToPreviousScreen()
        {
            if (previousScreen)
            {
                SwitchScreen(previousScreen);
            }
        }

        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(WaitToLoadScene(sceneIndex));
        }

        // Screen Fading
        public void FadeIn()
        {
            if (m_Fader)
            {
                m_Fader.CrossFadeAlpha(0, m_fadeInDuration, false);
            }
        }

        public void FadeOut()
        {
            if (m_Fader)
            {
                m_Fader.CrossFadeAlpha(1, m_fadeOutDuration, false);
            }
        }
        IEnumerator WaitToLoadScene(int sceneIndex)
        {
            yield return null;
        }
        #endregion
    }
}
