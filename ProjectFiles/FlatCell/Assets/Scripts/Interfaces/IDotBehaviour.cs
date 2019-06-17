/*
 * 
\* IDotBehaviour.cs
 *
\* Nick S.
\* Game Logic - AI
 *
*/

using Geo.Command;

/*
 * IAIBehaviour - AI Behaviour Interface
 * 
 * This interface controls all behaviour for AI. To make an AI, define it's Action functions
 * and call them successively in Update().
 * Actions as of v0.1.0
   - Move
   - Shoot
   - Shield
 * 
*/

namespace DotBehaviour.Command
{
    public interface IAIBehaviour
    {
        // Sets the parent.
        void Init(IGeo geo);

        // Updates the parent's stats according to it's kill record.
        void CheckScore();

        // Move Logic for AI.
        void Move();

        // Returns the type of behaviour script.
        EDot_Behaviour GetType();

        // Shields Logic for AI.
        void Shields();

        // Shooting Logic for AI.
        void Fire();
    }

    // Specifically Dot AI. Uses inheritance. 
    public interface IDotBehaviour : IAIBehaviour
    {

    }
}
