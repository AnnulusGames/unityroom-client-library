using System.Collections.Generic;
using UnityEngine;
using unityroom.Api.Internals;

namespace unityroom.Api
{
    public class UnityroomApiClient : MonoBehaviour, IUnityroomApiClient
    {
        /// <summary>
        /// シングルトン用インスタンス
        /// </summary>
        private static UnityroomApiClient _instance;
        /// <summary>
        /// HMAC認証用キー
        /// </summary>
        [SerializeField]
        private string HmacKey;
        /// <summary>
        /// スコアボードAPIクライアント
        /// </summary>
        private readonly Dictionary<int, Scoreboard> _scoreboards = new();
        /// <summary>
        /// シングルトンインスタンスを取得
        /// </summary>
        public static IUnityroomApiClient Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindAnyObjectByType<UnityroomApiClient>();
                if (_instance == null) { Debug.LogError($"[unityroom] シーンにUnityroomClientのPrefabを配置してください。"); }

                return _instance;
            }
        }

        private void Awake()
        {
            if (string.IsNullOrEmpty(HmacKey))
            {
                Debug.LogError($"[unityroom] インスペクターにてHMAC認証用キーをセットしてください。", gameObject);
            }
        }

        /// <summary>
        /// スコア送信
        /// </summary>
        /// <param name="boardNo">ボードNo</param>
        /// <param name="score">送信するスコア</param>
        /// <param name="mode">スコアボードの書き込み設定</param>
        public void SendScore(
            int boardNo
            , float score
            , ScoreboardWriteMode mode
        )
        {
            if (!_scoreboards.ContainsKey(boardNo))
            {
                var b = gameObject.AddComponent<Scoreboard>();
                b.Initialize(boardNo, HmacKey);
                _scoreboards.Add(boardNo, b);
            }

            var scoreboard = _scoreboards[boardNo];
            scoreboard.AddScore(score, mode);
        }
    }
}