﻿
﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using TrebleGameUtils;

namespace Apocalyptic_Sunrise
{
    public class Level
    {

        public TiledMap map = null;
        public TiledTileLayer collisionLayer;
        public Player player;

        public GameStates gameState;
        public Vector2 playerSpawningPosition;
        public int levelIndex = 0;

        public DevLogging Debug;

        private const int numberOfLevels = 3;
        GraphicsDeviceManager graphics;

        public static int tile = 32;
        public static float meter = tile;



        public int ScreenWidth
        {
            get { return graphics.GraphicsDevice.Viewport.Width; }
        }

        public int ScreenHeight
        {
            get { return graphics.GraphicsDevice.Viewport.Width; }
        }

        public void LoadNextMap(ContentManager Content)
        {
            levelIndex++;
            if (levelIndex == 1)
            {

                map = Content.Load<TiledMap>("Level1");
                playerSpawningPosition = new Vector2(96, 96);

                map = Content.Load<TiledMap>("Level1");
                Debug.WriteToFile("Level " + levelIndex + " has been loaded", true, false);

            }
            else if (levelIndex == 2)
            {
                player.isVisible = true;
                gameState.isVisible = true;
                map = Content.Load<TiledMap>("Level2");
                Debug.WriteToFile("Level " + levelIndex + " has been loaded", true, false);
            }
            foreach (TiledTileLayer layer in map.TileLayers)
            {
                if (layer.Name == "Collisions")
                {
                    collisionLayer = layer;
                }
            }
        }


        public static int PixelToTile(float pixelCoord)
        {
            return (int)Math.Floor(pixelCoord / tile);
        }

        public static int TileToPixel(int tileCoord)
        {
            return tile * tileCoord;
        }

        public int CellAtPixelCoord(Vector2 pixelCoords)
        {
            if (pixelCoords.X < 0 ||
           pixelCoords.X > map.WidthInPixels || pixelCoords.Y < 0)
                return 1;
            // let the player drop of the bottom of the screen (this means death)
            if (pixelCoords.Y > map.HeightInPixels)
                return 0;
            return CellAtTileCoord(
           PixelToTile(pixelCoords.X), PixelToTile(pixelCoords.Y));
        }

        public int CellAtTileCoord(int tx, int ty)
        {
            if (tx < 0 || tx >= map.Width || ty < 0)
                return 1;
            // let the player drop of the bottom of the screen (this means death)
            if (ty >= map.Height)
                return 0;
            TiledTile tile = collisionLayer.GetTile(tx, ty);
            return tile.Id;
        }

        public void Update(GameTime gameTime)
        {
            int tx = PixelToTile(player.sPosition.X);
            int ty = PixelToTile(player.sPosition.Y);
            bool nx = (player.sPosition.X) % tile != 0;

            bool ny = (player.sPosition.Y) % tile != 0;
            bool cell = CellAtTileCoord(tx, ty) != 0;
            bool cellright = CellAtTileCoord(tx + 1, ty) != 0;
            bool celldown = CellAtTileCoord(tx, ty + 1) != 0;
            bool celldiag = CellAtTileCoord(tx + 1, ty + 1) != 0;

            if (player.sDirection.Y > 0)
            {
                if ((celldown && !cell) || (celldiag && !cellright && nx))
                {
                    // clamp the y position to avoid falling into platform below
                    player.sPosition.Y = TileToPixel(ty) - 5;
                    player.sDirection.Y = 0; // stop downward velocity
                    ny = false; // - no longer overlaps the cells below
                }
            }
            else if (player.sDirection.Y < 0)
            {
                if ((cell && !celldown) || (cellright && !celldiag && nx))
                {
                    // clamp the y position to avoid jumping into platform above
                    player.sPosition.Y = TileToPixel(ty + 1) - 25;
                    player.sDirection.Y = 0; // stop upward velocity
                                             // player is no longer really in that cell, we clamped them
                                             // to the cell below
                    cell = celldown;
                    cellright = celldiag; // (ditto)
                    ny = false; // player no longer overlaps the cells below
                }
            }

            if (player.sDirection.X > 0)
            {
                if ((cellright && !cell) || (celldiag && !celldown && ny))
                {
                    // clamp the x position to avoid moving into the platform
                    // we just hit
                    player.sPosition.X = TileToPixel(tx);
                    player.sDirection.X = 0; // stop horizontal velocity
                }
            }
            else if (player.sDirection.X < 0)
            {
                if ((cell && !cellright) || (celldown && !celldiag && ny))
                {
                    // clamp the x position to avoid moving into the platform
                    // we just hit
                    player.sPosition.X = TileToPixel(tx + 1);
                    player.sDirection.X = 0; // stop horizontal velocity
                }
            }
        }
    }
}

            

 
    


