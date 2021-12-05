using System.Collections.Generic;

namespace Neurocita.Reactive
{
    public interface IServiceBusConfiguration
    {
        IServiceBusBuilder CreateBuilder();
    }
}
