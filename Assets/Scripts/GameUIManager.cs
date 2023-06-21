using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PyramidSolitaire
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private GameObject _wonPanel;
        [SerializeField] private GameObject _lostPanel;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            _playAgainButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
        }

        public void ShowGameOver(EndState state)
        {
            _canvas.enabled = true;
            _wonPanel.SetActive(state == EndState.Won);
            _lostPanel.SetActive(state == EndState.Lost);
        }
    }
}