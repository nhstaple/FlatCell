using Geo.Command;

namespace DotBehaviour.Command
{
    public interface IAIBehaviour
    {
        void init(IGeo geo);

        void exec();

        void CheckScore();

        void Move();

        string GetType();

        void Shields();

        void Fire();
    }

    public interface IDotBehaviour : IAIBehaviour
    {

    }
}
