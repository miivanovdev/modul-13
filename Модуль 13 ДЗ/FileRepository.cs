using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib;

namespace Модуль_13_ДЗ
{
    public class FileClientRepository : IRepository<Client>
    {
        private string DepartmentsFile { get; set; }
        private string AccountsFile { get; set; }
        private string ClientsFile { get; set; }
        private string LogFile { get; set; }

        private readonly string SelectAllCommand;
        private readonly string SelectOneCommand;
        private readonly string UpdateCommand;
        private readonly string DeleteCommand;
        private readonly string InsertCommand;
        private readonly string ConnectionString;

        public FileClientRepository()
        {
            /*
            DepartmentsFile = ConfigurationManager.AppSettings["DepartmentsFile"];
            AccountsFile = ConfigurationManager.AppSettings["AccountsFile"];
            ClientsFile = ConfigurationManager.AppSettings["ClientsFile"];
            LogFile = ConfigurationManager.AppSettings["LogFile"];
            */
        }
        /*
        /// <summary>
        /// Метод сохранения данных в файл
        /// </summary>
        /// <param name="args"></param>
        private void SaveData(object args)
        {

            string jsonClients = JsonConvert.SerializeObject(Clients);
            File.WriteAllText(ClientsFile, jsonClients);

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            };

            string jsonDepartments = JsonConvert.SerializeObject(BankDepartments, settings);
            File.WriteAllText(DepartmentsFile, jsonDepartments);

            string jsonAccounts = JsonConvert.SerializeObject(Accounts, settings);
            File.WriteAllText(AccountsFile, jsonAccounts);

            WriteLogAsync();
        }

        public async void WriteLogAsync()
        {
            using (StreamWriter sw = new StreamWriter(File.OpenWrite(LogFile)))
            {
                foreach (var l in Log)
                {
                    await sw.WriteLineAsync(JsonConvert.SerializeObject(l));
                }
            }
        }

        private void InitFromFiles()
        {
            if (File.Exists(ClientsFile))
            {
                string jsonClients = File.ReadAllText(ClientsFile);
                Clients = JsonConvert.DeserializeObject<ObservableCollection<Client>>(jsonClients);
            }
            else
            {
                Clients = new ObservableCollection<Client>()
                {
                    new Client("Кулибяка", "Вадим", "Натанович", 27450, true),
                    new Client("Пыпырин", "Владимир", "Юльевич", 38850),
                    new Client("Прокопов", "Алексей", "Александрович", 40450),
                    new Client("Никишин", "Олег", "Викторович", 120250),
                    new Client("Крупская", "Анна", "Сергеевна", 117300),
                    new Client("Коняев", "Станислав", "Валерьевич", 301200),
                    new Client("Чизмар", "Валентина", "Витальевна", 178500)
                };
            }

            if (File.Exists(LogFile))
            {
                string jsonLog = File.ReadAllText(LogFile);
                Log = JsonConvert.DeserializeObject<ObservableCollection<LogMessage>>(jsonLog);
            }
            else
            {
                Log = new ObservableCollection<LogMessage>();
            }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            };

            if (File.Exists(DepartmentsFile))
            {
                string jsonDepartments = File.ReadAllText(DepartmentsFile);
                BankDepartments = JsonConvert.DeserializeObject<List<BankDepartment<BankAccount>>>(jsonDepartments, settings);

                foreach (var b in BankDepartments)
                    b.Log = Log;
            }
            else
            {
                BankDepartments = new List<BankDepartment<BankAccount>>()
                {
                    new BankDepartment<BankAccount>(Log, "Не выбрано!", 0, 0, 0, true),
                    new PhysicalDepartment(Log, "Отдел по работе с физическими лицами", 50000, 6, 15),
                    new IndividualDepartment(Log, "Отдел по работе с юридическими лицами", 30000, 12, 15),
                    new PrivilegedDepartment(Log, "Отдел по работе с привелигированными клиентами", 100000, 18, 20)
                };
            }

            if (File.Exists(AccountsFile))
            {
                string jsonAccounts = File.ReadAllText(AccountsFile);
                Accounts = JsonConvert.DeserializeObject<List<BankAccount>>(jsonAccounts, settings);
            }
            else
            {
                Accounts = new List<BankAccount>()
                {
                    new PhysicalAccount(40000, 10, Clients[0].ClientId, Clients[0].Name, 1 , 6, new DateTime(2020, 11, 05)),
                    new PhysicalAccount(78540, 12, Clients[0].ClientId, Clients[0].Name, 1, 6, new DateTime(2019, 10, 23)),
                    new PhysicalAccount(63400, 10, Clients[1].ClientId, Clients[1].Name, 1, 6, new DateTime(2020, 09, 04)),
                    new PhysicalAccount(-48900, 10, Clients[1].ClientId, Clients[1].Name, 1, 6, new DateTime(2020, 06, 12)),
                    new IndividualAccount(34000, 10, Clients[2].ClientId, Clients[2].Name, 2, 12, new DateTime(2019, 01, 24), 2),
                    new IndividualAccount(-500000, 10, Clients[2].ClientId, Clients[2].Name, 2, 12, new DateTime(2021, 01, 13), 2),
                    new IndividualAccount(1240000, 10, Clients[3].ClientId, Clients[3].Name, 2, 12, new DateTime(2020, 08, 07), 2),
                    new IndividualAccount(12700, 10, Clients[3].ClientId, Clients[3].Name, 2, 12, new DateTime(2020, 02, 14), 2),
                    new IndividualAccount(481500, 15, Clients[4].ClientId, Clients[4].Name, 2, 12, new DateTime(2020, 07, 15), 2),
                    new PrivilegedAccount(3012250, 20, Clients[5].ClientId, Clients[5].Name, 3, 18, new DateTime(2018, 10, 12)),
                    new PrivilegedAccount(2012250, 15, Clients[6].ClientId, Clients[6].Name, 3, 18, new DateTime(2019, 11, 21)),
                };
            }
        }
        */

        public void Create(Client item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetList()
        {
            throw new NotImplementedException();
        }

        public Client GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Client item)
        {
            throw new NotImplementedException();
        }
    }
}
