using System.Reflection.Emit;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flooper
{
    // テクスチャー管理クラス
    static class Texture
    {//テクスチャ―の静的クラスの作成
        public static Texture2D Floor; // 地面
        public static Texture2D[] Character; // キャラクター
        public static Texture2D Dokan_ue; //土管上
        public static Texture2D Dokan; //土管ボディ
        public static Texture2D[] TitleImages; //title画像の配列
        public static Texture2D GameOver; //GameOver画像
        public static Texture2D Number; //数値の画像
        public static Texture2D Easy; //難易度の画像
        public static Texture2D Hard; //難易度の画像
        public static Texture2D yajirusi; //⇧⇩の画像
        public static Texture2D name; //名前の画像
        public static Texture2D level; //難易度の画像
        public static Texture2D setei; //名前の画像
        public static SpriteFont Font; //フォント
        public static Texture2D enterK; //エンターで決定の画像
        public static Texture2D sifutBack; //シフトで戻るの画像
        public static Texture2D ranking; //ランキング表の画像
        public static Texture2D enterRestart; //エンターでリスタートの画像
        public static Texture2D[] rank; //ランキングのアイコンの画像配列
        public static Texture2D rankingIcon; //ランキングアイコン画像
        public static Texture2D seteiIcon; //設定アイコンの画像
        public static Texture2D start; //設定アイコンの画像
        public static void Load(ContentManager contentManager)
        {
            // 画像の読み込み
            Floor     = contentManager.Load<Texture2D>("jimen");
            Character = new Texture2D[]{
                contentManager.Load<Texture2D>("nico"),
                contentManager.Load<Texture2D>("dansyaku"),
                contentManager.Load<Texture2D>("toribo"),
            };
            Dokan_ue      = contentManager.Load<Texture2D>("dokan_ue");
            Dokan      = contentManager.Load<Texture2D>("dokan");
            GameOver      = contentManager.Load<Texture2D>("GameOver");
            Number = contentManager.Load<Texture2D>("number");
            TitleImages = new Texture2D[]{
                contentManager.Load<Texture2D>("Title1"),
                contentManager.Load<Texture2D>("Title2")
            };
            Easy=contentManager.Load<Texture2D>("easy");
            Hard=contentManager.Load<Texture2D>("hard");
            Font=contentManager.Load<SpriteFont>("Font");
            yajirusi=contentManager.Load<Texture2D>("uesita");
            name=contentManager.Load<Texture2D>("name");
            level=contentManager.Load<Texture2D>("level");
            setei=contentManager.Load<Texture2D>("setei");
            enterK=contentManager.Load<Texture2D>("enterk");
            sifutBack=contentManager.Load<Texture2D>("sifutBack");
            ranking=contentManager.Load<Texture2D>("ranking");
            enterRestart=contentManager.Load<Texture2D>("enterRstart");
            rank = new Texture2D[]{
                contentManager.Load<Texture2D>("rank1"),
                contentManager.Load<Texture2D>("rank2"),
                contentManager.Load<Texture2D>("rank3"),
                contentManager.Load<Texture2D>("rank4"),
                contentManager.Load<Texture2D>("rank5"),
                contentManager.Load<Texture2D>("rank6"),
                contentManager.Load<Texture2D>("rank7"),
                contentManager.Load<Texture2D>("rank8"),
                contentManager.Load<Texture2D>("rank9"),
                contentManager.Load<Texture2D>("rank10")
            };
            rankingIcon=contentManager.Load<Texture2D>("rankingIcon");
            seteiIcon=contentManager.Load<Texture2D>("seteiicon");
            start=contentManager.Load<Texture2D>("start");
        }
    }
}