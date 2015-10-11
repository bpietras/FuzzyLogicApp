using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using FuzzyLogicWebService.Models;
using FuzzyLogicModel;
using FuzzyLogicWebService.Logging;

namespace FuzzyLogicWebService.Controllers
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel kernel;

        public NinjectControllerFactory(IKernel ninjectKernel)
        {
            kernel = ninjectKernel;
            AddBindings();
        }

        private void AddBindings()
        {
            kernel.Bind<FuzzyLogicDBEntities>().To<FuzzyLogicDBEntities>();
            kernel.Bind<IDatabaseRepository>().To<FuzzyLogicDbRepository>().WithConstructorArgument<FuzzyLogicDBEntities>(kernel.Get<FuzzyLogicDBEntities>());
            //kernel.Bind<ChartController>().ToSelf().InTransientScope().WithConstructorArgument<IDatabaseRepository> (kernel.Get<IDatabaseRepository>());
            kernel.Bind<HigherController>().ToSelf().WithConstructorArgument<IDatabaseRepository>(kernel.Get<IDatabaseRepository>())
                .WithConstructorArgument<ILogger>(kernel.Get<ILogger>());
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return  controllerType == null ? null : (IController)kernel.Get(controllerType);
        }

    }
}
