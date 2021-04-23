using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleSystemExample
{
    public class FireworkParticleSystem : ParticleSystem
    {
        public FireworkParticleSystem(Game game, int maxExplosions) : base(game, maxExplosions * 50)
        {

        }

        Color[] colors = new Color[]
        {
            Color.Fuchsia,
            Color.Crimson,
            Color.Aqua,            
            Color.Yellow,
            Color.White,
            Color.Green 
        };

        private Color color;

        protected override void InitializeConstants()
        {
            base.InitializeConstants();
            textureFilename = "particle";
            minNumParticles = 500;
            maxNumParticles = 1000;

            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            base.InitializeParticle(ref p, where);
            
            var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(40, 500);

            var lifeTime = RandomHelper.NextFloat(0.5f, 1.0f);
            
            var acceleration = -velocity / lifeTime;

            var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

            var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver4);

            var scale = RandomHelper.NextFloat(1, 2);

            p.Initialize(where, velocity, acceleration, color, scale: scale, lifetime: lifeTime, rotation: rotation, angularVelocity: angularVelocity);
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalLifetime = particle.TimeSinceStart / particle.Lifetime;


            particle.Scale = .75f + .25f * normalLifetime;
        }

        public void PlaceFirework(Vector2 where) 
        {
            color = colors[RandomHelper.Next(colors.Length)];
            AddParticles(where);
        }



    }
}
