using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using FuzzyLogicWebService.Models;
using FuzzyLogicModel;

namespace FuzzyLogicWebService.Controllers
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<FuzzyLogicDBEntities>().To<FuzzyLogicDBEntities>().InThreadScope();
            ninjectKernel.Bind<IDatabaseRepository>().To<FuzzyLogicDbRepository>().WithConstructorArgument<FuzzyLogicDBEntities>(ninjectKernel.Get<FuzzyLogicDBEntities>());
            ninjectKernel.Bind<Controller>().To<HigherController>().WithConstructorArgument<IDatabaseRepository>(ninjectKernel.Get<IDatabaseRepository>());


        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return  controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

    }
}
