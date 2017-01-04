using System;
using System.Data;
 
namespace OpenRetail.Model
{    
    public class ValidationError
    {
        public string Message { get; set; }
        public string PropertyName { get; set; }
    }
}
