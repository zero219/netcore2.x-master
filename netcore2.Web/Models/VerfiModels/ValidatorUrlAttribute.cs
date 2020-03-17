using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.VerfiModels
{
    /// <summary>
    /// 自定义Url验证
    /// </summary>
    public class ValidatorUrlAttribute : Attribute, IModelValidator
    {
        public string ErrorMessage { get; set; }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            var url = context.Model as string;
            if (url != null && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return Enumerable.Empty<ModelValidationResult>();
            }
            return new List<ModelValidationResult>()
            {
                new  ModelValidationResult(string.Empty,ErrorMessage)
            };
        }
    }
}
