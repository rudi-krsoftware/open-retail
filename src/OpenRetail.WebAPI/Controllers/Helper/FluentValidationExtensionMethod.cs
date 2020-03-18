/**
 * Copyright (C) 2017 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/rudi-krsoftware/open-retail
 */

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

        private static ValidationResult Validate<T>(T obj, string ruleSetName)
        {
            ValidationResult validationResult = null;
            var vt = typeof(AbstractValidator<>);
            var et = obj.GetType();
            var evt = vt.MakeGenericType(et);

            var assemblyModel = Assembly.GetExecutingAssembly();

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

        private static Type FindValidatorType(Assembly assembly, Type evt)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (evt == null) throw new ArgumentNullException("evt");
            return assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(evt));
        }
    }
}