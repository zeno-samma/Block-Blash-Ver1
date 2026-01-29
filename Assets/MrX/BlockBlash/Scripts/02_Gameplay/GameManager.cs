using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace MrX.BlockBlash
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Ins;
        public GameState CurrentState { get; private set; }
        //Lắng nghe sự kiện khi mở game
        // ====================================
        private void OnEnable()
        {
            EventBus.Subscribe<PlayerDiedEvent>(GameOver);
            EventBus.Subscribe<PlayerLeveledUpEvent>(OnPlayerLeveledUp);
            EventBus.Subscribe<UpgradeChosenEvent>(OnUpgradeChosen);
        }
        //Huỷ lắng nghe sự kiện khi game đóng
        // ====================================
        private void OnDisable()
        {
            EventBus.Unsubscribe<PlayerDiedEvent>(GameOver);
            EventBus.Unsubscribe<PlayerLeveledUpEvent>(OnPlayerLeveledUp);
            EventBus.Unsubscribe<UpgradeChosenEvent>(OnUpgradeChosen);
        }
        //Hàm sự kiện
        // =============================================
        private void OnUpgradeChosen(UpgradeChosenEvent value)
        {
            UpdateGameState(GameState.PLAYING);
        }

        private void OnPlayerLeveledUp(PlayerLeveledUpEvent value)
        {

            Debug.Log("2.Lắng nghe vào UPGRADEPHASE");
            UpdateGameState(GameState.UPGRADEPHASE);
        }
        // =====================================
        void Awake()
        {
            // Ra lệnh cho game chạy ở 60 FPS
            Application.targetFrameRate = 60;
            // Singleton Pattern
            if (Ins != null && Ins != this)
            {
                // Nếu log này xuất hiện, đây chính là lỗi của bạn
                Debug.LogError("GAME MANAGER BI HUY NHAM!", this.gameObject);
                Destroy(gameObject);
                return; // Thêm return để chắc chắn
            }
            else
            {
                Debug.Log("Game Manager khoi tao thanh cong", this.gameObject);
                Ins = this;
                DontDestroyOnLoad(gameObject); // Giữ GameManager tồn tại giữa các scene
            }
        }

        void Start()
        {
            Debug.Log("--- GameManager START CALLED! ---");
            // Khi game vừa bắt đầu, phát nhạc loading/menu
            // Kiểm tra AudioManager
            if (AudioManager.Instance == null)
            {
                Debug.LogError("LOI: AudioManager.Instance BI NULL!");
                return; // Dừng lại ở đây nếu có lỗi
            }
            Debug.Log("AudioManager OK.");
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
            // SceneManager.LoadScene("MainMenu");
            // Kiểm tra SceneLoader
            if (SceneLoader.Instance == null)
            {
                Debug.LogError("LOI: SceneLoader.Instance BỊ NULL!");
                return; // Dừng lại ở đây nếu có lỗi
            }
            Debug.Log("SceneLoader OK. Chuan bi tai MainMenu...");
            SceneLoader.Instance.LoadScene("MainMenu");
            // Bắt đầu game bằng trạng thái khởi tạo
            UpdateGameState(GameState.PREPAIR);
        }

        public void UpdateGameState(GameState newState)
        {
            // Tránh gọi lại logic nếu không có gì thay đổi
            if (newState == CurrentState)
            {
                Debug.Log("Trùng");
                return;
            }
            CurrentState = newState;
            // Xử lý logic đặc biệt ngay khi chuyển sang state mới
            switch (newState)
            {
                case GameState.PREPAIR:
                    // Debug.Log("code chuẩn bị game");
                    // ... code chuẩn bị game ...
                    // Sau khi chuẩn bị xong, tự động chuyển sang Playing
                    // EventBus.Publish(new InitialUIDataReadyEvent { defScore = Pref.coins });//Phát thông báo lần đầu để ui cập nhật lên màn hình đầu game.
                    break;
                case GameState.PLAYING:
                    Time.timeScale = 1f;
                    break;
                case GameState.PAUSE:
                    Time.timeScale = 0f;
                    break;
                case GameState.UPGRADEPHASE:
                    Debug.Log("3.vào UPGRADEPHASE");
                    Time.timeScale = 0f; // Dừng game để người chơi nâng cấp
                    break;
                case GameState.GAMEOVER:
                    Time.timeScale = 0f; // Dừng game
                    break;
            }

            // 4. Phát đi "báo cáo" về trạng thái mới cho các hệ thống khác lắng nghe
            EventBus.Publish(new StateUpdatedEvent { CurState = newState });//Phát đi thông báo về sự kiện đã đăng ký, Các hệ thống đã đăng ký sẽ nhận được thông báo này.
            Debug.Log("Game state changed to: " + newState);
        }
    
        public void PlayGame()///1.Sau khi ấn nút play
        {

            // Khi vào màn chơi, đổi sang nhạc gameplay
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameplayMusic);
            SceneLoader.Instance.LoadScene("Gameplay");
            UpdateGameState(GameState.PLAYING);
            EventBus.Publish(new EnemySpawnedEvent { });//Phát thông báo lần đầu thay đổi state
            // ActivePlayer();
        }
        public void GameOver(PlayerDiedEvent value)//							   
        {

            UpdateGameState(GameState.GAMEOVER);
        }


















































    }
}