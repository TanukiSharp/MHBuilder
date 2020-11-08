using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHBuilder.Core;

namespace MHBuilder.WPF
{
    public interface IManagedWindow
    {
        void OnOpening(bool isAlreadyOpened, object? argument)
        {
        }

        void OnOpened(bool isAlreadyOpened, object? argument)
        {
        }

        void OnCancel(CancellableOperationParameter cancellableOperationParameter)
        {
        }
    }
}
