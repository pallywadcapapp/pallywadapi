using Amazon.Auth.AccessControlPolicy;
using Microsoft.EntityFrameworkCore;
using PallyWad.Domain;
using PallyWad.Infrastructure.Data;
using System.Globalization;

namespace PallyWad.Accounting.Helpers
{
    public class NumberSystem
    {
        private string _tenantId { get; set; }
        public IConfigurationRoot configuration;
        static string basePath = AppContext.BaseDirectory;

        CultureInfo culture = new CultureInfo("fr-FR");
        //MultiTenantIdentityDbContext dbcont;
        SetupDbContext dbcont;
        AppIdentityDbContext dbIdcont;

        //Dim urole As RoleRights

        public NumberSystem()
        {
            configuration = new ConfigurationBuilder()
           .SetBasePath(basePath)
           .AddJsonFile("appsettings.json")
           .AddJsonFile("config.json")
           .Build();

            var options = new DbContextOptionsBuilder<SetupDbContext>();
            options.UseSqlServer(configuration.GetConnectionString("Default"));

            var options2 = new DbContextOptionsBuilder<AppIdentityDbContext>();
            options.UseSqlServer(configuration.GetConnectionString("Default"));


            dbcont = new SetupDbContext(options.Options);
            dbIdcont = new AppIdentityDbContext(options2.Options);
            _tenantId = "";
        }

        public string GenPin(string type, string pol, int inc, string marketer, string subrisk)
        {
            int nos = getMaxPolNo(type);
            string pin = null;
            for (int i = 1; i <= nos; i++)
            {
                pin += getPol(type, pol, i, inc, marketer, subrisk);
            }
            //pin &= "/"
            //pin += getNumber(pol, inc).ToString("D6");
            //add2NumberingComponent(pol, inc);
            return pin;
        }

        public string getClaimsNo(string pol, int inc)
        {
            return getNumber(pol, inc).ToString("D6");
        }

        public string getClaimsRecoveryNo(string pol, int inc)
        {
            return "CLMR/" + getNumber(pol, inc).ToString("D6");
        }

        public int getMaxPolNo(string policy)
        {

            try
            {
                var tab = dbcont.ProductTracks.Where(u => u.productname == policy).Select(u => u.productrange).FirstOrDefault();
                return tab;
            }
            catch
            {
                return 100;
            }
        }

        public string getCode(string pols)
        {
            var val = dbcont.NumbComps.Where(u => u.component == pols).Select(u => u.code).FirstOrDefault();
            return val.ToString();
        }

        public string getPol(string type, string polic, int pos, int inc, string marketer, string subrisk)
        {
            //MultitenantIdentityDbContext dbs = new MultitenantIdentityDbContext();
            var pre = dbcont.NumbCompOrders.Where(p => p.productname == type && p.position == pos).Select(p => p.productcode).FirstOrDefault();
            if (pre == "Year")
            {
                return DateTime.Now.Year.ToString();
            }
            else if (pre == "Month")
            {
                return DateTime.Now.Month.ToString();
            }
            else if (pre == "Number")
            {
                var number = getNumber(polic, inc).ToString("D6");
                add2NumberingComponent(polic, inc);
                return number;
            }
            else
            {
                var nval = dbcont.NumbComps.Where(u => u.component == pre).Select(u => u.code).FirstOrDefault();
                if (string.IsNullOrEmpty(nval))
                {
                    return null;
                }
                return nval.ToString();
            }

        }

        int getNumber(string com, int increament)
        {
            try
            {
                var vals = dbcont.ProductNos.Where(u => u.component == com).FirstOrDefault();
                var val = vals.number;
                val += increament;
                vals.number = val;
                dbcont.SaveChanges();
                return val;
            }
            catch
            {
                var numcomponent = new ProductNo();
                numcomponent.component = com;
                numcomponent.number = increament;
                dbcont.ProductNos.Add(numcomponent);
                dbcont.SaveChanges();
                return increament;
            }
        }

        private void updatePolicy(ProductNo vals, int val)
        {
            vals.number = val;
            dbcont.SaveChanges();
        }

        bool add2NumberingComponent(string com, int increament)
        {
            try
            {
                var val = dbcont.ProductNos.Where(u => u.component == com).FirstOrDefault();//ToList()(0);
                val.number = getNumber(com, increament) + increament;

                dbcont.SaveChanges();
                return true;
            }
            catch
            {
                try
                {
                    var polcy = new ProductNo();// numberingComponent;
                    polcy.component = com;
                    polcy.number = increament;
                    dbcont.ProductNos.Add(polcy);
                    dbcont.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public string getDNCNNo(string pol, int inc, string businessform)
        {
            var val = "";
            if (businessform.Equals("CREDIT NOTE") || businessform.Equals("CO-OUT") || businessform.Equals("FAC-OUT"))
            {
                val = getNumber(pol, inc).ToString("D6");

                return "CN" + val;
            }
            else
            {
                val = getNumber(pol, inc).ToString("D6");
                return "DN" + val;
            }
        }

        public string getReceiptNo(string locationCode)
        {
            //string code = getBranchCode(locationCode);
            string temp = getNumber("RECEIPT", 1).ToString("D6");
            string receipt = temp;// + "/" + code;
            return receipt;
        }
        public string getReceiptNo()
        {
            string receipt = getNumber("RECEIPT", 1).ToString("D6");
            return receipt;
        }

        public string getVoucherNo(int flag)
        {
            string code = "";
            if (flag == 0)
            {
                code = "PV";
            }
            else
            {
                code = "JV";
            }
            string temp = getNumber("VOUCHER", 1).ToString("D6");
            string receipt = code + temp;
            return receipt;
        }

        /*public string getBranchCode(string branch)
        {
            try
            {
                var code = dbcont.Tblbranches.Where(u => u.Branchname == branch).Select(u => u.code).FirstOrDefault();
                return code;
            }
            catch (Exception)
            {

                return "LV";
            }
        }*/
    }
}
