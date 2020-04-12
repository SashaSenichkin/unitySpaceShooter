using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace SpaceShooter
{
    public class GameManager: MonoBehaviour
    {
        public GameObject[] OrderedButtons;
        public Canvas MenuCanvas;
        public Dictionary<GameObject, LevelParams> AllLevels;
        private void Start()
        {
            DontDestroyOnLoad(this);
            AllLevels = new Dictionary<GameObject, LevelParams>();
            foreach (var button in OrderedButtons)
            {
                var newLevelParams = new LevelParams() { NextLevel = AllLevels.LastOrDefault().Value };
                AllLevels.Add(button, newLevelParams);
                button.GetComponent<Button>().onClick.AddListener(() => ProcessButtonClick(button));
            }

            AllLevels.First().Value.IsLevelOpened = true;
            UpdateButtonColors();
        }
        void UpdateButtonColors()
        {
            foreach (var item in AllLevels)
                item.Key.GetComponent<Image>().color = item.Value.IsLevelOpened ? Color.white : Color.grey;
        }

        public void LevelComplete(GameObject sender, bool isWin)
        {
            var nextLevel = AllLevels[sender].NextLevel;
            if (isWin && nextLevel != null)
                AllLevels[sender].NextLevel.IsLevelOpened = true;

            MenuCanvas.enabled = true;
            UpdateButtonColors();
            SceneManager.UnloadSceneAsync(1);
        }

        void ProcessButtonClick(GameObject sender)
        {
            if (!AllLevels[sender].IsLevelOpened)
                return;

            var asyncLoader = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            asyncLoader.completed += (arg1) =>
            {
                MenuCanvas.enabled = false;
                var lvlControl = new LevelControl(AllLevels[sender]);
                lvlControl.OnLevelFinished += (isWin) => LevelComplete(sender, isWin);
            };
        }
    }
}
