using FarmApp.DAL.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL.EF
{
    //TODO: переместить в namespace FarmApp.DAL.Entities и папку Entities
    //TODO: для Entities не использовать маппинг по конвенкции, для каждой Entity 
    //      маппинг задать в отдельном классе/файле, максимально настроить маппинг для
    //      генерации адекватной схемы БД: типы полей, ограничения, индексы, FK и т.п.
    public class FarmContext : DbContext
	{
		public DbSet<Agriculture> Agricultures { get; set; }

		public DbSet<Crop> Crops { get; set; }

		public DbSet<Farm> Farms { get; set; }

		public DbSet<Farmer> Farmers { get; set; }

		public DbSet<Region> Regions { get; set; }

		//TODO: не подходящее место (магия языка, может выйти боком). 
		//      Все миграции лучше запускать явно, вызовом метода, при старте приложения
		static FarmContext()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<FarmContext, Configuration>());
		}

		//TODO: если требуется 2 конструктора, то явно описать use-case
		public FarmContext()
			: base("FarmContext")
		{
            
		}

		public FarmContext(string connectionString)
			: base(connectionString)
		{
		}
	}
}
