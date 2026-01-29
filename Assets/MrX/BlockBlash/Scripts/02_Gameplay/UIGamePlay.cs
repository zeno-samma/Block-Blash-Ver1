using UnityEngine;

namespace MrX.BlockBlash
{
    public class UIGamePlay : MonoBehaviour
    {
        // test tốc độc asmdef;
        // [SerializeField] public GameObject MainMenuPanel;
        public CanvasGroup canvasGroupLevelUP;
        void Awake()
        {
            canvasGroupLevelUP.alpha = 0;
        }
        //Lắng nghe sự kiện khi mở game
        // ====================================
        private void OnEnable()
        {
            EventBus.Subscribe<StateUpdatedEvent>(HandleGameStateChange);//Lắng nghe trạng thái game do gamemanager quản lý
        }

        //Huỷ lắng nghe sự kiện khi game đóng
        // ====================================
        private void OnDisable()
        {
            EventBus.Unsubscribe<StateUpdatedEvent>(HandleGameStateChange);
        }
        // private void OnInitialUIDataReadyEvent(InitialUIDataReadyEvent value)//Cập nhật dữ liệu ban đầu lên ui
        // {
        //     // HP_Count_Txt.SetText("{0}", value.defHealth);
        //     // Total_HP_Txt.SetText("{/}", value.maxHealth);
        //     // scoreGameGUITxt.text = $"{value.defScore}";
        //     // scoreHomeGUITxt.text = $"{value.defScore}";
        // }// Hàm này được EventBus tự động gọi mỗi khi GameManager thay đổi trạng thái
        //Hàm sự kiện
        // =============================================
        private void HandleGameStateChange(StateUpdatedEvent gameState)//1. Nhận thông báo và quản lý các ui
        {
            canvasGroupLevelUP.alpha = 0;
            switch (gameState.CurState)
            {
                case GameState.PLAYING:
                    canvasGroupLevelUP.alpha = 0;
                    break;
                case GameState.PAUSE:
                    break;
                case GameState.UPGRADEPHASE:
                    Debug.Log("4.vào UPGRADEPHASE");
                    canvasGroupLevelUP.alpha = 1;
                    break;
                case GameState.GAMEOVER:
                    canvasGroupLevelUP.alpha = 0;
                    break;
            }
        }
    }
}


