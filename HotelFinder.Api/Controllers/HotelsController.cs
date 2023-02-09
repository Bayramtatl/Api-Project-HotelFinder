using HotelFinder.Business.Abstact;
using HotelFinder.Business.Concrete;
using HotelFinder.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Eğer bu yazılıysa validationu otomatik yapar yani modelstate.isvalid kullanmana
    // gerek kalmaz. Model valid değilse zaten datayı eklemez.
    public class HotelsController : ControllerBase
    {
        private IHotelService _hotelService;
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }
        /// <summary>
        /// Tüm otelleri döndürür.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return  Ok(await _hotelService.GetAllHotels()); // 200 + data
        }
        /// <summary>
        /// Id değerine göre oteli döndürür.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (await _hotelService.GetHotelById(id) != null)
            {
                return Ok(await _hotelService.GetHotelById(id)); //200 + data
            }
            return NotFound(); //404
        }
        /// <summary>
        /// Otel ekler
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateHotel([FromBody]Hotel hotel)
        {
            var createdHotel = await _hotelService.CreateHotel(hotel);
            return CreatedAtAction("Get", new {id = createdHotel.Id, createdHotel}); //201 + data
        }
        /// <summary>
        /// Id değerine göre Otel Günceller
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateHotel([FromBody] Hotel hotel)
        {
            if(await _hotelService.GetHotelById(hotel.Id) != null)
            {
                return Ok(await _hotelService.UpdateHotel(hotel));//200 + data
            }
            return NotFound();// 404
        }
        /// <summary>
        /// Id değerine göre Otel siler
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("[action]/id")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_hotelService.GetHotelById(id) != null)
            {
                await _hotelService.DeleteHotel(id);
                return Ok(); //200
            }
            return NotFound();//404
        }

        [HttpGet]
        [Route("[action]/{id}/{name}")]
        //[Rote("[action]")] bu da querystring olarak yollar urlde şu şekilde gözükür:
        //.../api/hotels/GetHotelByIdAndName?id=1&name=blabla
        public async Task<IActionResult> GetHotelByIdAndName(int id,string name)
        {
            return Ok();
        }
    }
}
