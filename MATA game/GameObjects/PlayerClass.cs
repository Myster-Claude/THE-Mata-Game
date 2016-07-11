using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MATA_game
{
    class PlayerClass : AnimatedSprite
    {
        #region Properties
        public Game1 Game;

        #endregion

        #region Collectors
        public PlayerClass(Game1 game, Vector2 position)
            : base(game, position)
        {
            
            framesPerSecond = 1;
            AddAnimation(1, 0, 0, "Idle", 60, 72, new Vector2(0, 0));
            AddAnimation(3, 0, 1, "Down", 60, 72, new Vector2(0,0));
            AddAnimation(3, 212 , 1, "Up", 60, 72, new Vector2(0, 0));
            AddAnimation(3, 70, 1, "Left", 60, 72, new Vector2(0, 0));
            AddAnimation(3, 140, 1, "Right", 60, 72, new Vector2(0, 0));

          //  AddAnimation(3, 48, 1, "Left", 32, 48, new Vector2(0, 0));
            //AddAnimation(1, 48, 0, "LeftIdle", 32, 48, new Vector2(0, 0));
           // AddAnimation(3, 96, 1, "Right", 32, 48, new Vector2(0, 0));
            //AddAnimation(1, 96, 0, "RightIdle", 32, 48, new Vector2(0, 0));
            //AddAnimation(1, 0, 0, "Idle", 32, 48, new Vector2(0, 0));

            //AddAnimation(3, 170, 1, "Left", 71, 48, new Vector2(0, 0));
            //AddAnimation(1, 48, 0, "LeftIdle", 71, 80, new Vector2(0, 0));
            //AddAnimation(3, 48, 1, "Right", 32, 48, new Vector2(0, 0));
            //AddAnimation(1, 48, 1, "RightIdle", 32, 48, new Vector2(0, 0));
            //AddAnimation(3, 48, 1, "Up", 32, 48, new Vector2(0, 0));
            //AddAnimation(1, 48, 1, "UpIdle", 32, 48, new Vector2(0, 0));
            //AddAnimation(3, 48, 1, "Down", 32, 48, new Vector2(0, 0));
            //AddAnimation(1, 48, 1, "DownIdle", 32, 48, new Vector2(0, 0));

            PlayAnimation("Idle");
        }

        void Initialize()
        {
            maxLimit = new Vector2(Game.CurrentScreenSize.X + (m_size.X / 2), Game.CurrentScreenSize.Y + (m_size.Y / 2));
            minLimit = new Vector2(0 - (m_size.X / 2), 0 - (m_size.Y / 2));
        }
        
        public override void Update(GameTime gameTime)
        {

            sPosition += sDirection;
            
            m_position += m_velocity;
            if (m_texture == null)
            {
                
            }
        }

        public void GetInput(GameTime gameTime)
        {
            m_velocity = new Vector2(0);
            sDirection = new Vector2(0);
            

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-3, 0);
                sDirection += new Vector2(-3, 0);
                PlayAnimation("Left");
               
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(3, 0);
                sDirection += new Vector2(3, 0);
                PlayAnimation("Right");
               
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                m_velocity = new Vector2(0, -3);
                sDirection += new Vector2(0, 3);
                PlayAnimation("Up");
               
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                m_velocity = new Vector2(0, 3);
                sDirection += new Vector2(0, 3);
                PlayAnimation("Down");
        
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-3, -3);
                sDirection += new Vector2(-3, -3);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(3, -3);
                sDirection += new Vector2(3, -3);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                m_velocity = new Vector2(-3, 3);
                sDirection += new Vector2(-3, 3);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                m_velocity = new Vector2(3, 3);
                sDirection += new Vector2(3, 3);
            }
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
            {
                
            }
        }
        #endregion
    }
}