﻿using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using OdeToFood.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OdeToFood.Web
{
    public class ContainerConfig
    {
        internal static void RegisterContainer(HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();

            // scan for all abstractions in my app (find all controller types)
            // so the autofac is aware about them
            // MvcApplication is the class that represents this app (Global.asax.cs)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // register also ApiController types
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            // let the builder know, whenever someone asks to implement the IRestaurantData
            // use the type InMemoryRestaurantData
            builder.RegisterType<InMemoryResaurantData>()
                .As<IRestaurantData>()
                .SingleInstance(); //lifetime of instance, singleton (never use for multiple users)

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // dependency resolver for ApiController
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}