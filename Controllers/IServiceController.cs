using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Controllers
{
    public interface IServiceController
    {
        int SpotCount();
        void TakeSlot(int id);
        void LeaveSlot(int id);
        int Difference();
        void AddIn();
        void AddOut();
    }
}
