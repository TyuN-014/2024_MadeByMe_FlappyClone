using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Flooper
{
    public class RankingEntry
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public bool IsNewRanking{get;set;}

        public RankingEntry(string playerName, int score)
        {
            PlayerName = playerName;
            Score = score;
            IsNewRanking = false;
        }
    }

    public class RankingManager
    {
        private const string RankingFilePathEasy = @"C:\Users\沖　美幸\Documents\制作\24_個人製作\Flooper\bin\Debug\net8.0\ranking_easy.json"; //Easy保存ファイス
        private const string RankingFilePathHard = @"C:\Users\沖　美幸\Documents\制作\24_個人製作\Flooper\bin\Debug\net8.0\ranking_hard.json";//Hard保存ファイル
        private List<RankingEntry> rankingsEasy; // Easyのランキングリスト
        private List<RankingEntry> rankingsHard; // Hardのランキングリスト

        public RankingManager()
        {
            rankingsEasy = LoadRankings(Difficulty.Easy);
            rankingsHard = LoadRankings(Difficulty.Hard); 
        }

        // ランキングをファイルに保存
        public void SaveRankings(Difficulty difficulty)
        {
            //難易度別のファイルのif
            string filepath = difficulty ==Difficulty.Easy ? RankingFilePathEasy :RankingFilePathHard;
            List<RankingEntry> rankings = difficulty == Difficulty.Easy ? rankingsEasy:rankingsHard;
            string json = JsonConvert.SerializeObject(rankings,Formatting.Indented); // JSON形式に変換
            File.WriteAllText(filepath,json); //ファイルに書き込む
        }

        // ランキングをファイルから読み込む
        private List<RankingEntry> LoadRankings(Difficulty difficulty)
        {
            string filePath = difficulty == Difficulty.Easy ? RankingFilePathEasy:RankingFilePathHard;

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath); // ファイルを読み込み
                return JsonConvert.DeserializeObject<List<RankingEntry>>(json); // JSONからリストに変換
            }
            return new List<RankingEntry>(); // ファイルがなければ空のリストを返す
        }

        // 新しいスコアを追加して保存
        public void AddRanking(Difficulty difficulty,string playerName, int score)
        {
            //難易度の分岐
            List<RankingEntry> rankings = difficulty==Difficulty.Easy ? rankingsEasy:rankingsHard;

            // 新しいスコアをランキングリストに追加
            RankingEntry newEntry = new RankingEntry(playerName,score);
            rankings.Add(newEntry);

            // スコアを降順でソート（スコアが高い順）
            rankings.Sort((a, b) => b.Score.CompareTo(a.Score));

            //トップ10に入ったかどうかチェック
            for(int i = 0;i<rankings.Count;i++){
                if(i<10){
                    rankings[i].IsNewRanking=false;
                }
                else{
                    break;
                }
            }

            //スコア更新したら
            if(rankings.IndexOf(newEntry)<10){
                newEntry.IsNewRanking=true;
                SoundManager.RankUp.Play();
            }

            // トップ10を保持
            if (rankings.Count > 10)
            {
                rankings.RemoveAt(rankings.Count - 1); // 10位以下を削除
            }

            SaveRankings(difficulty); // 更新されたランキングを保存
        }

        // トップ10のランキングを取得
        public List<RankingEntry> GetTopRankings(Difficulty difficulty)
        {
            return difficulty == Difficulty.Easy ? rankingsEasy:rankingsHard; // ランキングリストを返す
        }

        // ランキングをリセット
        public void ResetRankings(Difficulty difficulty)
        {
            if(difficulty == Difficulty.Easy){
                rankingsEasy.Clear();
            }
            else{
                rankingsHard.Clear();
            }
            SaveRankings(difficulty);//リセット後に保存
        }
    }
}
