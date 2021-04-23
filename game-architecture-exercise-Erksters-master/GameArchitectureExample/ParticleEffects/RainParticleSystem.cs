using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace ParticleSystemExample
{
    public class RainParticleSystem : ParticleSystem
    {
        Rectangle _source ;

        public bool IsRaining = true;

        public RainParticleSystem(Game game , Rectangle source) : base (game , 1000)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "drop";
            minNumParticles = 1;
            maxNumParticles = 3;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitY * 260, new Vector2(-20,0), Color.SkyBlue, scale: RandomHelper.NextFloat(0.1f, 0.4f), lifetime: 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsRaining)
            {
                AddParticles(_source);
            }
            
        }
    }
}
