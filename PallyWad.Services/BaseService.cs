using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Services
{
    public class BaseService//: IBaseService
    {

        static string basePath = AppContext.BaseDirectory;
        public IConfigurationRoot configuration;
        public BaseService()
        {
            configuration = new ConfigurationBuilder()
        .SetBasePath(basePath)
        //.AddJsonFile("config.json")
        .Build();
        }
    }
}
