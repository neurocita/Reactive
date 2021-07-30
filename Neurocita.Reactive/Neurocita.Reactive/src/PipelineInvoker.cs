using System.Threading.Tasks;

namespace Neurocita.Reactive
{
    internal class PipelineInvoker : IPipelineInvoker
    {
        public Task Invoke<T>(IPipeline<T> pipeline)
        {
            return Task.Run(() =>
            {
                IPipelineContext pipelineContext = pipeline.Producer.Invoke();
                foreach (IPipelineStep pipelineStep in pipeline.Steps)
                {
                    pipelineContext = pipelineStep.Invoke(pipelineContext);
                }
                pipeline.Consumer.Invoke(pipelineContext);
            });
        }
    }
}
