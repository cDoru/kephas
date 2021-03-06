﻿namespace SignalRChat.WebApp.Application
{
    using System.Threading;
    using System.Threading.Tasks;

    using Kephas.Services;
    using Kephas.Threading.Tasks;
    using Kephas.Web.Owin.Application;

    using Owin;

    /// <summary>
    /// A SignalR application initializer.
    /// </summary>
    [ProcessingPriority(Priority.High)]
    public class SignalRAppInitializer : OwinAppInitializerBase
    {
        /// <summary>Initializes the application asynchronously.</summary>
        /// <param name="appContext">Context for the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        protected override Task InitializeCoreAsync(IOwinAppContext appContext, CancellationToken cancellationToken)
        {
            // Any connection or hub wire up and configuration should go here
            var app = appContext.AppBuilder;
            app.MapSignalR();

            return TaskHelper.CompletedTask;
        }
    }
}