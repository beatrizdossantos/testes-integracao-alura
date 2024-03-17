
namespace Alura.CoisasAFazer.Services.Handlers
{
    public class CommandResult
    {
        public CommandResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get;  }
    }
}
