using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Castle.Windsor;
using Castle.Windsor.Installer;
using SharpSentinel.UI.Views;

namespace SharpSentinel.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        private WindsorContainer _container;

        public Bootstrapper()
        {
            this.Initialize();
        }

        #region Startup

        protected override void Configure()
        {
            this._container = new WindsorContainer();
            this._container.Install(FromAssembly.This());

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            IoC.Get<IWindowManager>().ShowWindow(IoC.Get<ShellViewModel>());
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            this._container.Dispose();
        }

        #endregion

        #region IoC

        protected override object GetInstance(Type service, string key)
        {
            return this._container.Resolve(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return new List<object> { this._container.ResolveAll(service) };
        }

        #endregion
    }
}