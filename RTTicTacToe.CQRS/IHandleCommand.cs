using System.Collections;

namespace RTTicTacToe.CQRS
{
    public interface IHandleCommand<TCommand>
    {
        IEnumerable Handle(TCommand c);
    }
}
