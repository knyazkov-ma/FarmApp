using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL
{
	//TODO: Entity унаследовать от абстрактного BaseEntity, у которого есть ствойство Id
	//TODO: - лучше Id типа long
	//TODO: - еще лучше Entity<TKey> (учитывая второе замечание FarmContext)

	/// <summary>
	/// Регион
	/// </summary>
	public class Region
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Фермы
        /// </summary>
        public virtual List<Farm> Farms { get; set; }

        public Region()
        {
            Farms = new List<Farm>();
        }
    }
}
