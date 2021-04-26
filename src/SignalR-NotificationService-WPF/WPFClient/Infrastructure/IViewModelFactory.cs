using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobNotificationsClient.ViewModels;

namespace JobNotificationsClient.Infrastructure
{
    public interface IViewModelFactory
    {
        T Create<T>();
        T Create<T>(string key) where T : ViewModelBase;
    }
}
