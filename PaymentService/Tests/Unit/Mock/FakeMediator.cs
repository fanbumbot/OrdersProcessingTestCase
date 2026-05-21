using MediatR;

namespace PaymentService.Tests.Unit.Mock
{
    public class FakeMediator : IMediator
    {
        private object? _result;

        public void SetResult(object? result)
        {
            _result = result;
        }
        public Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult((TResponse)_result!);
        }

        public Task Send<TRequest>(
            TRequest request,
            CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            return Task.CompletedTask;
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification => Task.CompletedTask;
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_result);
        }
    }
}
