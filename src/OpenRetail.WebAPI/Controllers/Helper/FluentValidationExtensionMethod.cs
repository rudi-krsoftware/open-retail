using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using OpenRetail.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace OpenRetail.WebAPI.Controllers.Helper
{
    public static class FluentValidationExtensionMethod
    {
        public static bool IsValidate<T>(this T obj, HttpRequestMessage request, ref IHttpActionResult response, bool isUseRuleSet = true)
        {
            var routeName = string.Empty;

            if (isUseRuleSet)
            {
                var arrRouteName = HttpContext.Current.Request.FilePath.ToLower().Split('/');
                routeName = arrRouteName[arrRouteName.Count() - 1];
            }

            var validatorResults = Validate(obj, routeName);
            if (validatorResults == null)
                return false;

            if (!validatorResults.IsValid)
            {
                var errors = new List<string>();

                foreach (var failer in validatorResults.Errors)
                {
                    errors.Add(failer.ErrorMessage);
                }

                response = new HttpActionResultHelper(request, HttpStatusCode.BadRequest, new ResponsePackage(errors));

                return false;
            }

            return true;
        }

        static ValidationResult Validate<T>(T obj, string ruleSetName)
        {
            ValidationResult validationResult = null;
            var vt = typeof(AbstractValidator<>);
            var et = obj.GetType();
            var evt = vt.MakeGenericType(et);

            var assemblyModel = Assembly.GetExecutingAssembly(); // Assembly.LoadFrom(HostingEnvironment.MapPath("~/bin") + "/api.stamp.com.model.dll");

            if (assemblyModel != null)
            {
                try
                {
                    ValidationContext validatorContext = null;

                    if (ruleSetName.Length > 0)
                    {
                        validatorContext = new ValidationContext<T>(obj, new PropertyChain(), new RulesetValidatorSelector(ruleSetName));
                    }
                    else
                    {
                        validatorContext = new ValidationContext<T>(obj);
                    }

                    var validatorType = FindValidatorType(assemblyModel, evt);
                    var validatorInstance = (IValidator)Activator.CreateInstance(validatorType);

                    validationResult = validatorInstance.Validate(validatorContext);
                }
                catch
                {
                }
            }

            return validationResult;
        }

        static Type FindValidatorType(Assembly assembly, Type evt)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (evt == null) throw new ArgumentNullException("evt");
            return assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(evt));
        }
    }
}