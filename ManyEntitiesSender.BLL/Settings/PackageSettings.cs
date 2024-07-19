using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyEntitiesSender.BLL.Settings
{
    /// <summary>
    /// Значения хранятся в appsettings.json
    /// </summary>
    public class PackageSettings
    {
        /// <summary>
        /// Количество элементов, которые будут находится в базе при вызове POST Ensure
        /// </summary>
        public int PackageTotal { get; set; } = 100000;
        /// <summary>
        /// Количество элементов в одном пакете. Для стриминга (осталось самая малость - реализовать стриминг из REST)
        /// </summary>
        public int PackageCount { get; set; } = 5000;
        /// <summary>
        /// Если отсутствует стриминг, это максимальное количество объединённых отправляемых пакетов
        /// </summary>
        public int PackageLimit { get; set; } = 2;
    }
}
