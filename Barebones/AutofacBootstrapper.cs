using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Barebones.Pages;
using Barebones.Pages.Validation;
using FluentValidation;
using Stylet;

namespace Barebones
{
    public class AutofacBootstrapper : BootstrapperBase
    {
        private IContainer _container;

        private object _rootViewModel;
        protected virtual object RootViewModel
        {
            get { return this._rootViewModel ?? (this._rootViewModel = this.GetInstance(typeof(ShellViewModel))); }
        }

        protected override void ConfigureBootstrapper()
        {
            var builder = new ContainerBuilder();
            this.DefaultConfigureIoC(builder);
            this.ConfigureIoC(builder);
            this._container = builder.Build();
        }

        /// <summary>
        /// Carries out default configuration of the IoC _container. Override if you don't want to do this
        /// </summary>
        protected virtual void DefaultConfigureIoC(ContainerBuilder builder)
        {
            var config = new ViewManagerConfig()
            {
                ViewAssemblies = new List<Assembly>() { this.GetType().Assembly },
                ViewFactory = this.GetInstance
            };

            //Mandatory framework stuff
            var viewManager = new ViewManager(config);
            builder.RegisterInstance<IViewManager>(viewManager);
            builder.RegisterInstance<IWindowManagerConfig>(this);
            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<MessageBoxViewModel>().As<IMessageBoxViewModel>();
            builder.RegisterAssemblyTypes(this.GetType().Assembly);

            //Adapter to FluentValidations
            builder.RegisterGeneric(typeof(FluentValidationAdapter<>)).As(typeof(IModelValidator<>));

	        builder.RegisterType<ShellViewModelValidator>().As(typeof(IValidator<ShellViewModel>));
        }

        /// <summary>
        /// Override to add your own types to the IoC _container.
        /// </summary>
        protected virtual void ConfigureIoC(ContainerBuilder builder) { }

        public override object GetInstance(Type type)
        {
            return this._container.Resolve(type);
        }

        protected override void Launch()
        {
            base.DisplayRootView(this.RootViewModel);
        }

        public override void Dispose()
        {
            base.Dispose();
            ScreenExtensions.TryDispose(this._rootViewModel);
            this._container?.Dispose();
        }
    }
}