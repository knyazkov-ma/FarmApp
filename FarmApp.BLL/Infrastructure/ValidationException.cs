using System;
//TODO: удалить не используемые
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: переименовать в FarmApp.BLL.Exceptions
namespace FarmApp.BLL.Infrastructure
{
    //TODO: см. замечание в FarmService.AddFarmCrop
    public class ValidationException : Exception
    {
        public string Property { get; protected set; }
        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
