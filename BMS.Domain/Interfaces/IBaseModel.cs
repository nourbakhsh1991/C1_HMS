using BMS.Domain.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.Interfaces
{
    public interface IBaseModel<T> where T : MBaseDBModel
    {
        public T GetM();
    }
}
