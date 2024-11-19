using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic; // Listを使うのに必要
using System; // Randomを使うのに必要

namespace Flooper
{
    enum GameState{
        Titele,
        Setup,
        Play,
        GameOver,
        Ranking,
    }
    public class Game1 : Game
{
    
    private GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public static readonly int WindowWidth = 500;//ウィンドウの幅
    public static readonly int WindowHeight = 550;//高さ
    public static readonly int FloorHeight = 40; // 地面の高さ
    public static readonly int FloorImageWidth = 40; // 地面の画像の幅
    public static readonly float ScrollSpeed = 2f; // スクロール速度
    float scrollX = 0f; // 初期のスクロール位置
    Character character = new Character(); //キャラクターオブジェクト
    static readonly int DokanInterval = 250; // 土管の出現間隔(ピクセル)
    List<Dokan> dokans = new List<Dokan>(); // 土管のリスト
    int nextDokanSpawn = 50; // 次に土管が発生するスクロール位置
    Random rand = new Random();
    GameState state = GameState.Titele;//初期状態設定
    int titleIndex=0; //titleの配列要素を0
    double titleTimer=0; //titleの切り替え用タイマーの初期委
    double titleInterval = 0.5;//1秒で切り替え
    ScoreManager scoreManager = new ScoreManager();
    string playerName=" "; //プレイヤー名を格納する変数
    RankingManager rankingManager = new RankingManager();
    int selectedDifficulty=0; //難易度選択のインデックス
    string[] difficultyOptions = {"Easy","Hard"};
    double Cooldown = 0;  // 入力のクールダウン時間
    double CooldownTime = 0.3;  // 0.3秒で再入力できるように
    bool isCharacterVfisible = true; //キャラの表示
    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        //ウィンドウサイズの変更
        graphics.PreferredBackBufferWidth = WindowWidth;
        graphics.PreferredBackBufferHeight=WindowHeight;
        graphics.ApplyChanges();
        //マウスカーソルの表示
        IsMouseVisible = true;
    }

    public void PlayerNameInput(GameTime gameTime){
        if(Cooldown>0){
            Cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
        }
        var keyboardState=Keyboard.GetState();
        if(Cooldown<=0){
            if (keyboardState.IsKeyDown(Keys.Back) && playerName.Length > 0){
                playerName = playerName.Substring(0,playerName.Length-1);
                Cooldown=CooldownTime;
            }
            else{
                //アルファベット（大文字）と数字の入力許可
                for (Keys key = Keys.A; key <= Keys.Z; key++){
                    if (keyboardState.IsKeyDown(key) && !keyboardState.IsKeyDown(Keys.Enter)){
                        playerName+=key.ToString();
                        Cooldown=CooldownTime;
                    }
                }
                for (Keys key = Keys.D0; key <= Keys.D9; key++){
                    if (keyboardState.IsKeyDown(key) && !keyboardState.IsKeyDown(Keys.Enter)){
                        playerName += key.ToString().Substring(1); 
                        Cooldown=CooldownTime;
                    }
                }
            }
        }
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        base.Initialize();
        
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        //画像の読み込み
        Texture.Load(Content);

        //効果音の読み込み
        SoundManager.Load(Content);
        // TODO: use this.Content to load your game content here
    }
    //難易度選択
    public void SetDifficulty(){
        if(selectedDifficulty==0){
            character.difficulty = Difficulty.Easy; // Easy
        }
        else if(selectedDifficulty==1){
            character.difficulty = Difficulty.Hard; // Hard
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // TODO: Add your update logic here

        Input.Update();

        Cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

        //mouseの状態取得
        MouseState mouseState = Mouse.GetState();

        //タイトル状態のときの処理
        if(state == GameState.Titele){
            isCharacterVfisible=true;
            // 難易度が変更された場合、設定を反映させる
            SetDifficulty();
            //Scroll位置を進める
            scrollX += ScrollSpeed;
            titleTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(titleTimer>=titleInterval){
                titleIndex=(titleIndex+1)%Texture.TitleImages.Length;
                titleTimer=0;
            }
            //エンターキーが押された場合、ゲームを開始
            if(Keyboard.GetState().IsKeyDown(Keys.Enter)&&Cooldown <= 0){
                character.Start();
                nextDokanSpawn=(int)scrollX+50;
                state=GameState.Play;
                Cooldown = CooldownTime;
                SoundManager.Start.Play();
            }
            //シフト押したら設定
            if((Keyboard.GetState().IsKeyDown(Keys.LeftShift)||Keyboard.GetState().IsKeyDown(Keys.RightShift))&&Cooldown<=0){
                state=GameState.Setup;
                Cooldown = CooldownTime;
                isCharacterVfisible = false; 
                SoundManager.Click.Play();
            }            
            //設定アイコンの範囲チェック
            Rectangle settingsIconRect = new Rectangle(430, 10, Texture.seteiIcon.Width, Texture.seteiIcon.Height);
            if (settingsIconRect.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && Cooldown <= 0)
            {
                state = GameState.Setup;
                isCharacterVfisible = false;
                Cooldown = CooldownTime;
                SoundManager.Click.Play();
            }

            //ランキングアイコンの範囲チェック
            Rectangle rankingIconRect = new Rectangle(360, 2, Texture.rankingIcon.Width, Texture.rankingIcon.Height);
            if (rankingIconRect.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && Cooldown <= 0){
                
                state = GameState.Ranking;
                isCharacterVfisible = false; 
                Cooldown = CooldownTime;
                SoundManager.Click.Play();
            }

        }
        else if(state==GameState.Setup){

            PlayerNameInput(gameTime); //プレイヤー名の入力処理

            if(Keyboard.GetState().IsKeyDown(Keys.Up)&&Cooldown<=0){//矢印⇧を押したら
                selectedDifficulty=(selectedDifficulty-1+difficultyOptions.Length)%difficultyOptions.Length;
                Cooldown = CooldownTime; //連続入力防止
                SoundManager.Click.Play();
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Down)&& Cooldown<=0){//矢印⇧を押したら
                selectedDifficulty = (selectedDifficulty + 1) % difficultyOptions.Length;
                Cooldown = CooldownTime; //連続入力防止
                SoundManager.Click.Play();
            }
            //戻る
            if((Keyboard.GetState().IsKeyDown(Keys.LeftShift)||Keyboard.GetState().IsKeyDown(Keys.RightShift))&&Cooldown<=0){
                state=GameState.Titele;
                Cooldown = CooldownTime;
                SoundManager.Click.Play();
            }
            //マウスでも反応する戻る
            Rectangle backRect = new Rectangle(300, 460, Texture.sifutBack.Width, Texture.sifutBack.Height);
            if (backRect.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && Cooldown <= 0)
            {
                state=GameState.Titele;
                Cooldown = CooldownTime;
                SoundManager.Click.Play();
            }
            //マウスでも反応するeasy
            Rectangle easyRect = new Rectangle(155, 280, Texture.Easy.Width, Texture.Easy.Height);
            if (easyRect.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && Cooldown <= 0)
            {
                selectedDifficulty=(selectedDifficulty-1+difficultyOptions.Length)%difficultyOptions.Length;
                Cooldown = CooldownTime;
                SoundManager.Click.Play();
            }
            //マウスでも反応するhard
            Rectangle hardRect = new Rectangle(220, 280, Texture.Hard.Width, Texture.Hard.Height);
            if (hardRect.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && Cooldown <= 0)
            {
                selectedDifficulty=(selectedDifficulty-1+difficultyOptions.Length)%difficultyOptions.Length;
                Cooldown = CooldownTime;
                SoundManager.Click.Play();
            }
            
        }
        else if(state==GameState.Play){ //あそぶときの処理
            //スクロール位置を進める
            scrollX += ScrollSpeed;

            //キャラクターの処理
            character.Update();

            // 土管の処理
            foreach (Dokan dokan in dokans)
            {
                dokan.Update();
            }

            //障害物にぶつかったかの判定
            if(IsCollision()){
                state = GameState.GameOver;
                SoundManager.Clash.Play();
                character.Die();
            }

            //スコアの計測
            foreach(Dokan dokan in dokans){//foreach_配列の要素を順位取得してく
                if(!dokan.Score && dokan.GetUpCollisionRect().Right < character.GetCollisionRect().Left){
                    scoreManager.Score++; //スコアの加算
                    dokan.Score=true;
                }
            }
            
            // 新しい土管の生成
            if (scrollX >= nextDokanSpawn)
            {
                Vector2 position = new Vector2(WindowWidth + Dokan.Width / 2, 240);
                dokans.Add(new Dokan(WindowWidth, rand.Next(-60, 61)));
                nextDokanSpawn += DokanInterval;
            }
        
            //画面外の土管の削除
            dokans.RemoveAll(dokan => dokan.ScrolledOut());
        }
        else if(state ==GameState.GameOver){//ゲームオーバーの処理
            character.Update();
            if(Keyboard.GetState().IsKeyDown(Keys.Enter)&&Cooldown<=0){
                Cooldown = CooldownTime;
                GameOver();
                SoundManager.Click.Play();
            }
        }
        else if(state==GameState.Ranking){
            isCharacterVfisible = false; 
            if(Keyboard.GetState().IsKeyDown(Keys.Enter)&&Cooldown<=0){
                Cooldown = CooldownTime;
                RestartGame();
                SoundManager.Click.Play();
            }
            //マウスでも反応するようにする
            Rectangle enterRestartRect = new Rectangle(300, 510, Texture.enterRestart.Width, Texture.enterRestart.Height);
            if (enterRestartRect.Contains(mouseState.X, mouseState.Y) && mouseState.LeftButton == ButtonState.Pressed && Cooldown <= 0)
            {
                Cooldown = CooldownTime;
                RestartGame();
                SoundManager.Click.Play();
            }
        }
            
        base.Update(gameTime);
    }

    void RestartGame()
    {
        state=GameState.Titele;
        character.Reset();
        scoreManager.Reset();
        dokans.Clear();
        scrollX = 0f;
        nextDokanSpawn = 50; // 土管の初期出現位置を設定
    }

    bool IsCollision()
    {
        // 当たり判定
        Rectangle CollisionRect = character.GetCollisionRect();

        // 天井にぶつかったか
        if (CollisionRect.Top < 0){
            return true;
        }

        // 地面にぶつかったか
        if (CollisionRect.Bottom >= WindowHeight - FloorHeight){
            return true;
        }
            
        // 土管とぶつかったか
        foreach (Dokan dokan in dokans)
        {
            // 上側の土管とぶつかったか
            if (dokan.GetUpCollisionRect().Intersects(CollisionRect))
                return true;

            // 下側の土管とぶつかったか
            if (dokan.GetDownCollisionRect().Intersects(CollisionRect))
                return true;
        }

        // 何にもぶつかってない
        return false;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightSkyBlue);
        // 背景色⇧

        spriteBatch.Begin();

            // 地面を描画
            DrawFloor();

            // 土管を描画
            foreach (Dokan dokan in dokans)
            {
                dokan.Draw();
            }

            if(isCharacterVfisible&&(state!=GameState.Setup||state!=GameState.Ranking)){
                //キャラクターの描画
                character.Draw();
            }
            
            //タイトル画面の描画
            if(state==GameState.Titele){
                double titleWidth = (WindowWidth-(Texture.TitleImages[0].Width/1.7))/2;
                double startwidth = (WindowWidth-(Texture.start.Width/1.7))/2;
                float scale = 0.6f; //サイズを2倍にする
                Vector2 position = new Vector2((float)titleWidth, 60); // 位置
                //タイトルの描画
                spriteBatch.Draw(Texture.TitleImages[titleIndex], position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                //ランキングアイコン
                spriteBatch.Draw(Texture.rankingIcon,new Vector2(360,2),null,Color.White,0f,Vector2.Zero,0.25f,SpriteEffects.None,0f);
                //歯車アイコン
                spriteBatch.Draw(Texture.seteiIcon,new Vector2(430,10),null,Color.White,0f,Vector2.Zero,0.2f,SpriteEffects.None,0f);
                //startの点滅
                if(gameTime.TotalGameTime.TotalSeconds%1<0.5){
                    spriteBatch.Draw(Texture.start, new Vector2((float)startwidth,300), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }

            //難易度の画面
            if(state==GameState.Setup){
                float scale = 0.5f;
                Vector2 position = new Vector2(155, 280); // 位置を調整して描画
                //設定
                spriteBatch.Draw(Texture.setei,new Vector2(130,10),null,Color.White,0f,Vector2.Zero,scale,SpriteEffects.None,0f);
                //名前
                spriteBatch.Draw(Texture.name,new Vector2(60,90),null,Color.White,0f,Vector2.Zero,scale,SpriteEffects.None,0f);
                //名前入力場所
                spriteBatch.DrawString(Texture.Font, playerName, new Vector2(190, 110), Color.White);
                //--難易度--
                spriteBatch.Draw(Texture.level,new Vector2(110,210),null,Color.White,0f,Vector2.Zero,scale,SpriteEffects.None,0f);
                // Easy の画像を描画
                spriteBatch.Draw(Texture.Easy, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                position.Y+=65;
                // Hard の画像を描画
                spriteBatch.Draw(Texture.Hard, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                //シフトで戻るの画像
                spriteBatch.Draw(Texture.enterRestart,new Vector2(290,460),null,Color.White,0f,Vector2.Zero,0.4f,SpriteEffects.None,0f);
                if(selectedDifficulty==0){
                    //easyなら
                    position.Y-=65;
                    spriteBatch.Draw(Texture.Easy, position, null,Color.Yellow, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                else if(selectedDifficulty==1){
                    //hardなら
                    spriteBatch.Draw(Texture.Hard, position,null, Color.Yellow, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }

            //ゲームオーバー画面の描画
            if(state == GameState.GameOver){
                // 「げーむおーばー」を点滅させて描画
                if (gameTime.TotalGameTime.TotalSeconds % 1 < 0.7)
                {
                    float scale = 0.5f; //サイズを2倍にする
                    Vector2 position = new Vector2(20, 55); // 位置を適宜調整
                    spriteBatch.Draw(Texture.GameOver, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }
            //ランキング画面の描画
            if(state==GameState.Ranking){
                int rankingWidth = Texture.ranking.Width;
                int i=0; //カウント
                float rw = (WindowWidth-(rankingWidth/2))/2;
                float yPosition =150f;

                //ランキングの取得
                List<RankingEntry> topRankings = rankingManager.GetTopRankings(character.difficulty); 

                //ランキング表の描画
                spriteBatch.Draw(Texture.ranking,new Vector2(rw,30),null,Color.White,0f,Vector2.Zero,0.5f,SpriteEffects.None,0f);

                //ランキングリストの表示
                foreach(var entry in topRankings){
                    //アイコン
                    spriteBatch.Draw(Texture.rank[i],new Vector2(90,yPosition-5),null,Color.White,0f,Vector2.Zero,0.4f,SpriteEffects.None,0f);
                    //スコア更新の場合は黄色で表示
                    Color entryColor = entry.IsNewRanking ? Color.Yellow:Color.White;
                    //名前
                    spriteBatch.DrawString(Texture.Font, $"{entry.PlayerName,-10}", new Vector2(140, yPosition), entryColor);
                    //スコア
                    spriteBatch.DrawString(Texture.Font, $"{entry.Score,3}", new Vector2(300, yPosition), entryColor);
                    yPosition += 35f;
                    i++;
                }
                //エンターでリスタート
                spriteBatch.Draw(Texture.enterRestart,new Vector2(300,510),null,Color.White,0f,Vector2.Zero,0.35f,SpriteEffects.None,0f);

            }

            //数字の表示
            if(state==GameState.Play||state==GameState.GameOver)
            {
                scoreManager.Draw();
            }

            spriteBatch.End();

        base.Draw(gameTime);
    }
    // 地面を描画する
    void DrawFloor()
    {
        float y = WindowHeight - FloorHeight; //地面の描画位置
        float x = scrollX % FloorImageWidth; // スクロールに基づくオフセット

        // 画面幅に基づいて必要なタイル数を計算
        int tileCount = (WindowWidth / FloorImageWidth) + 2; 

        for (int i = 0; i < tileCount; i++)
        {
            float positionX = FloorImageWidth * i - x;
            spriteBatch.Draw(Texture.Floor, new Vector2(positionX, y), Color.White);
        }
    }

    void GameOver(){    //ゲームオーバーのときの処理
        if(!string.IsNullOrEmpty(playerName)){
            rankingManager.AddRanking(character.difficulty,playerName,scoreManager.Score);
        }
        state = GameState.Ranking;
    }
}
}


