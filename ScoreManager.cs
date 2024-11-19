using Microsoft.Xna.Framework;

namespace Flooper
{
    class ScoreManager
    {
      static readonly int CharWidth = 64; // 文字の幅
      static readonly int CharHeight =64; // 文字の高さ
      static readonly int positionY = 10; //縦の表示位置
      public int Score; //得点

      public void Draw(){
        int num = Score;
        int digits = CountDigits(num);

        //センター揃えで表示
        int positionX = (Game1.WindowWidth/2)-CharWidth + (CharWidth/2*digits);
        Vector2 position = new Vector2(positionX, positionY);

        //各桁を描画
        while(true){
          int singleDigit = num % 10;
          Rectangle rect = new Rectangle(CharWidth * singleDigit, 0, CharWidth, CharHeight);
          Game1.spriteBatch.Draw(Texture.Number,position,rect,Color.White);
          position.X -= CharWidth;
          num /= 10;
          if(num==0){
            break;
          }
        }
      }
      int CountDigits(int num)
      {
        int digits = 1;

        while (true)
        {
          num /= 10;

          if (num == 0)
          {
            return digits;
          }
          digits++;
        }
      }
      public void Reset(){
        Score=0;
      }
    }
}