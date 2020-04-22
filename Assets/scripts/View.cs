using System;
using UnityEngine;
#pragma warning disable CS0649

namespace SpaceShooter
{
    public class View : MonoBehaviour
    {
        public UnityEngine.UI.Text ScoreText;
        public UnityEngine.UI.Text LifeText;
        public UnityEngine.UI.Text GameOverText;
        public UnityEngine.UI.Button ExitToMenu;

        public const string UI_Score = "Score: {0} of {1}";
        public const string UI_Life = "Lives: ";
        public const string UI_LevelWin = "Level completed";
        public const string UI_LevelLose = "YOU DIED";

        private const string AxisNameVert = "Vertical";
        private const string AxisNameHor = "Horizontal";
        private const string ButtonName = "Fire1";


        [SerializeField]
        private Player playerScript;

        private Rigidbody SpaceshipRig;
        private AudioSource SpaceshipAudio;
        private Player SpaceshipPlayer;

        private LevelController MyControl;

        /// <summary>
        /// call before start!
        /// </summary>
        /// <param name="control">link to main presenter</param>
        public void Initialize(LevelController control)
        {
            MyControl = control;
            GeneralParams.Instance.OnScoreChanged += ScoreChanged;
            control.OnLevelFinished += Control_OnLevelFinished;
            GameOverText.enabled = false;
            ExitToMenu.onClick.AddListener(() => MyControl.GameOverLogic(null));

            var playerGO = Instantiate(playerScript.gameObject);
            SpaceshipRig = playerGO.GetComponent<Rigidbody>();
            SpaceshipAudio = playerGO.GetComponent<AudioSource>();
            SpaceshipPlayer = playerGO.GetComponent<Player>(); //не очень красиво, но лучше каждый раз получать чем править ручками если юнити поломает сцену
            var localDestroyInfo = playerGO.GetComponent<DestroyInfo>();

            localDestroyInfo.OnCollision += (collider) =>
            {
                localDestroyInfo.Health--;
                if (localDestroyInfo.Health <= 0)
                {
                    var explosion = Instantiate(localDestroyInfo.MyExplosion, transform.position, Quaternion.identity);
                    Destroy(explosion, 2);
                    MyControl.GameOverLogic(false);
                    Destroy(this);
                }
                else
                    LifeText.text = View.UI_Life + localDestroyInfo.Health;
            };

            LifeText.text = View.UI_Life + localDestroyInfo.Health;
        }

        private void Control_OnLevelFinished(bool isWin)
        {
            GameOverText.text = isWin ? View.UI_LevelWin : View.UI_LevelLose;
            GameOverText.enabled = true;
        }

        private void ScoreChanged(int score)
        {
            ScoreText.text = string.Format(View.UI_Score, score, MyControl.CurrentLevelParams.LevelScoreToFin);
        }

        public void Update()
        {
            if (MyControl == null)
                return; //have to be initilized

            if (Input.GetButton(ButtonName))
                MyControl.PlayerShot(SpaceshipPlayer.ShotSpawn, SpaceshipAudio);

            var moveHorizontal = Input.GetAxis(AxisNameHor);
            var moveVertical = Input.GetAxis(AxisNameVert);

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            SpaceshipRig.velocity = movement * 10;

            SpaceshipRig.position = new Vector3 //вообще не помню, зачем это... проверить.
            (
                Mathf.Clamp(SpaceshipRig.position.x, -SpaceshipPlayer.xSize, SpaceshipPlayer.xSize),
                0.0f,
                Mathf.Clamp(SpaceshipRig.position.z, -SpaceshipPlayer.zSize, SpaceshipPlayer.zSize)
            );

            SpaceshipRig.rotation = Quaternion.Euler(0.0f, 0.0f, -SpaceshipRig.velocity.x * SpaceshipPlayer.Tilt);
        }
    }
}
