using Microsoft.Xna.Framework;

namespace Flooper
{
    class Dokan
    {
        public static readonly int DokanVerticalSpace = 150; // 土管の上下の隙間の広さ
        public static readonly int Width = 64; // 土管の幅
        int upDokanHeight; // 上の土管の高さ
        int downDokanHeight; // 下の土管の高さ
        int downDokanTop; // 下の土管のふた位置
        public float positionX; // 左端位置
        public bool Score = false; //スコアを計上したかの子
        public Dokan(int left, int offsetFromCenter)
        {
            positionX = left; // 画面左端からの距離
            upDokanHeight = (Game1.WindowHeight - Game1.FloorHeight) / 2 - DokanVerticalSpace / 2 + offsetFromCenter; // 土管の高さの計算

            downDokanTop = upDokanHeight + DokanVerticalSpace; // 下の土管のふた位置の計算
            downDokanHeight = Game1.WindowHeight - downDokanTop - Game1.FloorHeight; // 下の土管の高さの計算
        }

        // 更新処理
        public void Update()
        {
            // 左へ移動する
            positionX -= Game1.ScrollSpeed;
        }

        // 描画処理
        public void Draw()
        {
            // 上側土管の描画先範囲を計算
            Rectangle upDokanDest = new Rectangle((int)positionX, -10, Width, upDokanHeight);
            Game1.spriteBatch.Draw(Texture.Dokan, upDokanDest, Color.White);

            // 上側の土管の下端部分を描画
            Game1.spriteBatch.Draw(Texture.Dokan_ue, new Vector2(positionX, upDokanHeight - 20), Color.White);

            // 下側の土管の上端部分を描画
            Game1.spriteBatch.Draw(Texture.Dokan_ue, new Vector2(positionX, downDokanTop + 6), Color.White);

            // 下側土管の描画先範囲を計算
            Rectangle downDokanDest = new Rectangle((int)positionX, downDokanTop + 18, Width, downDokanHeight - 9);
            Game1.spriteBatch.Draw(Texture.Dokan, downDokanDest, Color.White);
        }

        // 画面外に出たら消す
        public bool ScrolledOut()
        {
            return positionX + Width < 0;
        }

        // 上側の土管の当たり判定を返す
        public Rectangle GetUpCollisionRect()
        {
            return new Rectangle((int)positionX, 0, Width, upDokanHeight);
        }

        // 下側の土管の当たり判定を返す
        public Rectangle GetDownCollisionRect()
        {
            return new Rectangle((int)positionX, downDokanTop, Width, downDokanHeight);
        }

        //土管の座標
        public Vector2 GetDownDokanPosition()
        {
            return new Vector2(positionX, downDokanTop);
        }

    }
}
