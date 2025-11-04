using MediatR;
using Microsoft.Extensions.Logging;
using StoreApp.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Common.BehaviorPipes
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        private readonly Stopwatch _timer;
        private readonly ICurrentUserService _currentUserService;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _timer = new Stopwatch();
            _currentUserService = currentUserService;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Performance (3. for Command) (4. for query)"); // temprory

            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= 500) return response;

            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? "Anonymous";
            var phoneNumber = _currentUserService.PhoneNumber;

            _logger.LogWarning(
                "⏱ Long running request: {Name} ({ElapsedMilliseconds}ms)",
                requestName, elapsedMilliseconds
            );

            return response;

        }
    }
}
