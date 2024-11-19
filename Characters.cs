using System.Data;
using Microsoft.Xna.Framework;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Flooper
{
    public enum Difficulty
    {
      Easy,
      Hard
    }
  class Character
  {
    enum State{
      Ready,
      Play,
      Dead,
    }
    static readonly int Width = 48; // 幅
    static readonly int Height = 48; // 高さ
    static readonly int CollisionOffset = 8; // 当たり判定    
    static  float JumpVelocity = 5f; // ジャンプ力
    static  float Gravity = 0.1f; // 重力
    public Vector2 position = new Vector2(100, 200); // 位置
    float velocityY = 0f; // 垂直方向の移動速度
    State state = State.Ready; //状態
    public Difficulty difficulty = Difficulty.Easy; // 難易度のプロパティ
    Random rand = new Random(); //ランダム
    int characterIndex;

    public Character(){
      characterIndex = rand.Next(0,3);
    }
    
    public void Update()
    {
      //難易度に応じて変更
      AdjustDifficulty();
      if(state ==State.Play)
      {
        // 下向きに加速
        velocityY += Gravity;

        // スペースキーでジャンプ
        if (Input.IsJumpButtonDown())
        {
          Jump();
        }
        // 上昇または下降
        position.Y += velocityY;
      }
      else if(state==State.Dead){
        //地面に落下させて死す
        velocityY += Gravity;
        position.Y+=velocityY;
      }
    }

    //スタート
    public void Start()
    {
      state = State.Play;
      Jump();
    }

    //死す
    public void Die()
    {
      state = State.Dead;
      velocityY=0;
      
    }

    // ジャンプ
    public void Jump()
    {
      // 速度を上向きに
      velocityY = -JumpVelocity;
      //効果音
      SoundManager.Jump.Play();
    }

    // 描画処理
    public void Draw()
    {
      Game1.spriteBatch.Draw(Texture.Character[characterIndex], position, Color.White);
    }

    // 当たり判定を返却する
    public Rectangle GetCollisionRect()
    {
      // 画像サイズを少し小さくして扱う
      return new Rectangle(
        (int)position.X + CollisionOffset,
        (int)position.Y + CollisionOffset,
        Width - CollisionOffset * 2,
        Height - CollisionOffset * 2);
    }

    //キャラクター状態のリセット
    public void Reset(){
      position = new Vector2(100,200);
      state=State.Ready;
      velocityY=0f;
      characterIndex = rand.Next(0,3);
    }

    //難易度別の設定
    private void AdjustDifficulty(){
      switch(difficulty){
        case Difficulty.Easy:
          JumpVelocity=4.6f;
          Gravity=0.1f;
          break;
        case Difficulty.Hard:
        JumpVelocity = 5f;
        Gravity = 0.1f; 
          break;
      }
    }
  }
}