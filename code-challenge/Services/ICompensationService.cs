using challenge.Models;
using System;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation GetById(String id);
        Compensation Create(Compensation employee);
    }
}
