﻿using System;

namespace ManagedDoom
{
    public sealed class Mobj : Thinker
    {
        public static readonly Fixed OnFloorZ = Fixed.MinValue;
        public static readonly Fixed OnCeilingZ = Fixed.MaxValue;

        // Info for drawing: position.
        public Fixed X;
        public Fixed Y;
        public Fixed Z;

        // More list: links in sector (if needed)
        public Mobj SNext;
        public Mobj SPrev;

        //More drawing info: to determine current sprite.
        public Angle Angle; // orientation
        public Sprite Sprite; // used to find patch_t and flip value
        public int Frame; // might be ORed with FF_FULLBRIGHT

        // Interaction info, by BLOCKMAP.
        // Links in blocks (if needed).
        public Mobj BNext;
        public Mobj BPrev;

        public Subsector Subsector;

        // The closest interval over all contacted Sectors.
        public Fixed FloorZ;
        public Fixed CeilingZ;

        // For movement checking.
        public Fixed Radius;
        public Fixed Height;

        // Momentums, used to update position.
        public Fixed MomX;
        public Fixed MomY;
        public Fixed MomZ;

        // If == validcount, already checked.
        public int ValidCount;

        public MobjType Type;
        public MobjInfo Info; // &mobjinfo[mobj->type]

        public int Tics; // state tic counter
        public StateDef State;
        public MobjFlags Flags;
        public int Health;

        // Movement direction, movement generation (zig-zagging).
        public int MoveDir; // 0-7
        public int MoveCount; // when 0, select a new dir

        // Thing being chased/attacked (or NULL),
        // also the originator for missiles.
        public Mobj Target;

        // Reaction time: if non 0, don't attack yet.
        // Used by player to freeze a bit after teleporting.
        public int ReactionTime;

        // If >0, the target will be chased
        // no matter what (even if shot)
        public int Threshold;

        // Additional info record for player avatars only.
        // Only valid if type == MT_PLAYER
        public Player Player;

        // Player number last looked for.
        public int LastLook;

        // For nightmare respawn.
        public Thing SpawnPoint;

        // Thing being chased/attacked for tracers.
        public Mobj Tracer;



        public Mobj(World world) : base(world)
        {
        }


        public override void Run()
        {
            // momentum movement
            if (MomX != Fixed.Zero
                || MomY != Fixed.Zero
                || (Flags & MobjFlags.SkullFly) != 0)
            {
                World.P_XYMovement(this);

                // FIXME: decent NOP/NULL/Nil function pointer please.
                if (Removed)
                {
                    // mobj was removed
                    return;
                }
            }

            if ((Z != FloorZ) || MomZ != Fixed.Zero)
            {
                World.P_ZMovement(this);

                // FIXME: decent NOP/NULL/Nil function pointer please.
                if (Removed)
                {
                    // mobj was removed
                    return;
                }
            }


            // cycle through states,
            // calling action functions at transitions
            if (Tics != -1)
            {
                Tics--;

                // you can cycle through multiple states in a tic
                if (Tics == 0)
                {
                    if (!World.SetMobjState(this, State.Next))
                    {
                        return;     // freed itself
                    }
                }
            }
            else
            {
                /*
                // check for nightmare respawn
                if (!(mobj->flags & MF_COUNTKILL))
                    return;

                if (!respawnmonsters)
                    return;

                mobj->movecount++;

                if (mobj->movecount < 12 * 35)
                    return;

                if (leveltime & 31)
                    return;

                if (P_Random() > 4)
                    return;

                P_NightmareRespawn(mobj);
                */
            }
        }
    }
}