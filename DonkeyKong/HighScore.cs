using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace DonkeyKong
{
    internal class HighScore
    {
        Vector2 position;
        public List<int> scores = new List<int>();
        public HighScore(Vector2 position)
        {
            this.position = position;

            StreamReader sr = new StreamReader(@"HighScore.txt");
            for (int y = 0; y < 10; y++)
            {
                scores.Add(int.Parse(sr.ReadLine()));
            }
            sr.Close();
        }
        public void Add(Player player)
        {
            scores.Add(player.score + player.bonus);
            scores.Sort();
            scores.Reverse();
        }

        public void Save()
        {
            File.WriteAllText(@"HighScore.txt", String.Empty);
            StreamWriter outputFile = new StreamWriter(@"HighScore.txt");
            for (int i = 0; i < scores.Count; i++)
            {
                outputFile.WriteLine(scores[i]);
            }
            outputFile.Close();

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "scores", position, Color.White);
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(font, scores[i].ToString(), position + new Vector2(0, i * 40 + 40), Color.White);
            }
        }



    }
}