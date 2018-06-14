using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL
{
    /// <summary>
    /// Ферма
    /// </summary>
    public class Farm
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
        /// Площадь
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// Id фермера
        /// </summary>
        public int FarmerId { get; set; }

        /// <summary>
        /// Id региона
        /// </summary>
        public int RegionId { get; set; }


		//TODO: "Farmer Owner" запутывает, нет смысла использовать для одной роли имена, отличающиеся от типа,
		//      здесь и далее - переименовать Farmer Farmer
		/// <summary>
		/// Фермер-владелец
		/// </summary>
		public virtual Farmer Owner { get; set; }

        /// <summary>
        /// Регион-расположение
        /// </summary>
        public virtual Region Location { get; set; }        
    }
}
