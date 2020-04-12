using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;

namespace SpaceShooter
{
    public class GameManager: MonoBehaviour
    {
        [SerializeField]
        public GameObject[] OrderedButtons;
        [SerializeField]
        private Canvas MenuCanvas;

        private const string SaveFileName = "gameData.xml";
        private Dictionary<GameObject, LevelParams> AllLevels;
        private void Start()
        {
            DontDestroyOnLoad(this);
            AllLevels = new Dictionary<GameObject, LevelParams>();
            foreach (var button in OrderedButtons)
            {
                var newLevelParams = new LevelParams();
                if (AllLevels.Any())
                    AllLevels.LastOrDefault().Value.NextLevel = newLevelParams;

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
        public void SaveGame()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(LevelParams));
            using (FileStream fs = new FileStream(SaveFileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, AllLevels.First().Value);
            }
        }

        public void LoadGame()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(LevelParams));
            if (!File.Exists(SaveFileName))
            {
                Debug.LogError("deserialization no file");
                return;
            }

            using (FileStream fs = new FileStream(SaveFileName, FileMode.OpenOrCreate))
            {
                var levelParam = formatter.Deserialize(fs) as LevelParams;
                if (levelParam == null)
                    Debug.LogError("deserialization fail");

                var newLevelData = new Dictionary<GameObject, LevelParams>();
                foreach (var item in AllLevels)
                {
                    newLevelData.Add(item.Key, levelParam);
                    levelParam = levelParam.NextLevel;
                }

                AllLevels = newLevelData;
                UpdateButtonColors();

            }
        }
    }
}
