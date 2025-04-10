using System.Threading;
using System.Threading.Tasks;

namespace Scripts.BaseSystems.Core
{
    public interface ICommand
    {
        //     Best practice would be to use an Enum and cast it to the int

        public int CategoryId { get; }

        //     Same as categoryId best practice would be to use an Enum and cast it to the int
        public int CommandId { get; }

        public int MillisecondsSinceStart { get; }

        //      Client could be local user, network user, Ai
        //      should be used for simulation and cheating virification
        public string ClientId { get; } 

        public Task Execute(CancellationToken token);

        //      After canceling current command if this flag is TRUE next commnd will also be canceled.
        public bool CancelNextCommandAsWell { get; }

        public void Stop();
        public void Cancel(); 
    }
}
