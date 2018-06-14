using AutoMapper;
using FarmApp.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using FarmApp.ViewModels;
using System.Web.UI;
using System.Runtime.Caching;
using FarmApp.BLL.DTO;
using FarmApp.BLL.Infrastructure;
using System.Threading.Tasks;

namespace FarmApp.Controllers
{
	/// <summary>
	/// Работа с фермами
	/// </summary>
    public class FarmController : Controller
    {
		private IFarmService farmService;

		private IMapper mapper;


		public FarmController(IFarmService service, IMapper map)
			: base()
		{
			farmService = service;
			mapper = map;
		}
		
		/// <summary>
		/// Список ферм
		/// </summary>
		/// <returns></returns>
		public ActionResult List()
        {
			//TODO: здесь и далее - по-факту проекция не нужна, достаточно DTO
			//TODO: ToList() лишний - farmService возвращает материализованную коллекцию
			//TODO: объектное представление записи журнала и записи карточки, а также, связанные с этим запросы к БД,
			//      почти всегда различаются, поэтому сразу это предусмотреть на всех слоях
			var farms = farmService.GetFarms().AsQueryable().ProjectTo<FarmViewModel>(mapper.ConfigurationProvider).ToList();
			
			return View(farms);
        }

        /// <summary>
        /// Добавление фермы
        /// </summary>
        /// <returns></returns>        
        public ActionResult Create()
        {
			//TODO: copy-past
			//TODO: не правильное архитектурное решение:
			//      - кэширование запросов к БД для справочников реализовать в слое BLL 
			//      - если в этом есть реальная необходимость, иначе оптимизация на спичках, и увеличение сложности
			//      - как вариант, реализовать решение, связанное с кэшированием, в стиле AOP, сконфигурировав 
			//        перехват вызовов методов в IoC 
			
			MemoryCache cache = MemoryCache.Default;
            //если есть в кэше - берём из него, в противном случае запрашиваем из IFarmService и помещаем в кэш
            var rc = cache.Get("regions") as IEnumerable<NamedItemViewModel>;
            var fc = cache.Get("farmers") as IEnumerable<NamedItemViewModel>;
            var ac = cache.Get("agricultures") as IEnumerable<NamedItemViewModel>;
            
            if(rc == null)
            {
                rc = farmService.GetRegions().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
                cache.Add("regions", rc, new DateTimeOffset(DateTime.Now.AddSeconds(120)));
            }

            if (fc == null)
            {
                fc = farmService.GetFarmers().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
                cache.Add("farmers", fc, new DateTimeOffset(DateTime.Now.AddSeconds(120)));
            }

            if (ac == null)
            {
                ac = farmService.GetAgricultures().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
                cache.Add("agricultures", ac, new DateTimeOffset(DateTime.Now.AddSeconds(120)));
            }

            ViewBag.Regions = rc;
            ViewBag.Farmers = fc;
            ViewBag.Agricultures = ac;

            return View(new FarmCropViewModel());
        }

        [HttpPost]
        public ActionResult Create(FarmCropViewModel model)
        {
            var cache = MemoryCache.Default;
            //если есть в кэше - берём из него, в противном случае запрашиваем из IFarmService
            var rc = (cache.Get("regions") as IEnumerable<NamedItemViewModel>) ?? farmService.GetRegions().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
            var fc = (cache.Get("farmers") as IEnumerable<NamedItemViewModel>) ?? farmService.GetFarmers().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
            var ac = cache.Get("agricultures") as IEnumerable<NamedItemViewModel> ?? farmService.GetAgricultures().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);

			//TODO: использовать полноценный тип для View
			ViewBag.Regions = rc;
            ViewBag.Farmers = fc;
            ViewBag.Agricultures = ac;

            if (model.Area <= 0)
            {
                ModelState.AddModelError("Area", "Площадь должна быть больше нуля");
            }

            if(model.Gather < 0)
            {
                ModelState.AddModelError("Gather", "Урожай не может быть меньше нуля");
            }

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var farmCrop = mapper.Map<FarmCropDto>(model);
                farmService.AddFarmCrop(farmCrop);
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View(model);
            }

            return RedirectToAction("List");
        }

        /// <summary>
        /// Удаление фермы
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Delete(int Id)
        {
            var farm = farmService.GetFarm(Id);

			//TODO: реализовать явный маппинг как метод ViewModel, пусть он внутри 
			//      себя использует для простых случаев, mapper.Map
			var farmModel = mapper.Map<FarmViewModel>(farm);

            return View(farmModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, FormCollection formCollection /*TODO: лишний параметр*/)
        {
            farmService.DeleteFarm(Id);

            return RedirectToAction("List");
        }
    }
}