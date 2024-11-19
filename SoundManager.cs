using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Flooper
{
    static class SoundManager
    {
      public static SoundEffect Jump;
      public static SoundEffect Start;
      public static SoundEffect Clash;
      public static SoundEffect Click;
      public static SoundEffect ScoreUp;
      public static SoundEffect RankUp;

      public static void Load(ContentManager contentManager){
        Jump=contentManager.Load<SoundEffect>("jump");
        Start=contentManager.Load<SoundEffect>("start_sw");
        Clash=contentManager.Load<SoundEffect>("clash");
        Click=contentManager.Load<SoundEffect>("click");
        ScoreUp=contentManager.Load<SoundEffect>("score");
        RankUp=contentManager.Load<SoundEffect>("rankup");
      }
    }
}