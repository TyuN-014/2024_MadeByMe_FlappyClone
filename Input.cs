using Microsoft.Xna.Framework.Input;

namespace Flooper
{
    static class Input
    {
        static KeyboardState currentState; // 現在の状態
        static KeyboardState prevState; // 1フレーム前の状態

        public static void Update()
        {
            prevState = currentState;
            currentState = Keyboard.GetState();
        }

        // ジャンプボタン（スペースキー）が押された瞬間か？
        public static bool IsJumpButtonDown()
        {
            return currentState.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space);
        }
    }
}