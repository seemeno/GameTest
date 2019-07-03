using UnityEngine;
using UnityEngine.UI;


using System.Collections;

//wanghao
namespace Com.MyCompany.MyGame
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields


        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;


        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField]
        private Slider playerHealthSlider;

        [SerializeField]
        private Player target;


        #endregion


        #region MonoBehaviour Callbacks

        void Start()
        {
            if (target != null && playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
            Debug.Log(playerNameText.text);
        }

        void Update()
        {
            // Reflect the Player Health
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.curr_Health_Point;
            }
        }
        #endregion


        #region Public Methods

        public void SetTarget(Player _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
        }
        #endregion


    }
}