using System.Threading.Tasks;

namespace Neurocita.Reactive
{
    public interface IPipelineInvoker
    {
        Task Invoke<T>(IPipeline<T> pipeline);
    }
}
