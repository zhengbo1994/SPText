using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web;

namespace SPApiText.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 测试api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private IUserService _IUserService = new UserService();
        #region
        //#region HttpGet
        //// GET api/Users
        //[HttpGet]

        //public string GetText()
        //{
        //    return "123";
        //}

        //// GET api/Users/5
        //[HttpGet]
        //public Users GetUserByID(int id)
        //{
        //    //throw new Exception("1234567");
        //    string idParam = HttpContext.Current.Request.QueryString["id"];
        //    var user = _userList.FirstOrDefault(users => users.UserID == id);
        //    if (user == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return user;

        //}

        ////GET api/Users/?username=xx
        //[HttpGet]
        ////[CustomBasicAuthorizeAttribute]
        ////[CustomExceptionFilterAttribute]
        //public IEnumerable<Users> GetUserByName(string userName)
        //{

        //    //throw new Exception("1234567");

        //    string userNameParam = HttpContext.Current.Request.QueryString["userName"];

        //    return _userList.Where(p => string.Equals(p.UserName, userName, StringComparison.OrdinalIgnoreCase));
        //}

        ////GET api/Users/?username=xx&id=1
        //[HttpGet]
        //public IEnumerable<Users> GetUserByNameId(string userName, int id)
        //{
        //    string idParam = HttpContext.Current.Request.QueryString["id"];
        //    string userNameParam = HttpContext.Current.Request.QueryString["userName"];

        //    return _userList.Where(p => string.Equals(p.UserName, userName, StringComparison.OrdinalIgnoreCase));
        //}

        //[HttpGet]
        //public IEnumerable<Users> GetUserByModel(Users user)
        //{
        //    string idParam = HttpContext.Current.Request.QueryString["id"];
        //    string userNameParam = HttpContext.Current.Request.QueryString["userName"];
        //    string emai = HttpContext.Current.Request.QueryString["email"];

        //    return _userList;
        //}

        //[HttpGet]
        //public IEnumerable<Users> GetUserByModelUri([FromUri]Users user)
        //{
        //    string idParam = HttpContext.Current.Request.QueryString["id"];
        //    string userNameParam = HttpContext.Current.Request.QueryString["userName"];
        //    string emai = HttpContext.Current.Request.QueryString["email"];

        //    return _userList;
        //}

        //[HttpGet]
        //public IEnumerable<Users> GetUserByModelSerialize(string userString)
        //{
        //    Users user = JsonConvert.DeserializeObject<Users>(userString);
        //    return _userList;
        //}

        ////[HttpGet]
        //public IEnumerable<Users> GetUserByModelSerializeWithoutGet(string userString)
        //{
        //    Users user = JsonConvert.DeserializeObject<Users>(userString);
        //    return _userList;
        //}
        ///// <summary>
        ///// 方法名以Get开头，WebApi会自动默认这个请求就是get请求，而如果你以其他名称开头而又不标注方法的请求方式，那么这个时候服务器虽然找到了这个方法，但是由于请求方式不确定，所以直接返回给你405——方法不被允许的错误。
        ///// 最后结论：所有的WebApi方法最好是加上请求的方式（[HttpGet]/[HttpPost]/[HttpPut]/[HttpDelete]），不要偷懒，这样既能防止类似的错误，也有利于方法的维护，别人一看就知道这个方法是什么请求。
        ///// </summary>
        ///// <param name="userString"></param>
        ///// <returns></returns>
        //public IEnumerable<Users> NoGetUserByModelSerializeWithoutGet(string userString)
        //{
        //    Users user = JsonConvert.DeserializeObject<Users>(userString);
        //    return _userList;
        //}
        //#endregion HttpGet

        //#region HttpPost
        ////POST api/Users/RegisterNone
        //[HttpPost]
        //public Users RegisterNone()
        //{
        //    return _userList.FirstOrDefault();
        //}

        //[HttpPost]
        //public Users RegisterNoKey([FromBody]int id)
        //{
        //    string idParam = HttpContext.Current.Request.Form["id"];

        //    var user = _userList.FirstOrDefault(users => users.UserID == id);
        //    if (user == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return user;
        //}

        ////POST api/Users/register
        ////只接受一个参数的需要不给key才能拿到
        //[HttpPost]
        //public Users Register([FromBody]int id)//可以来自FromBody   FromUri
        //                                       //public Users Register(int id)//可以来自url
        //{
        //    string idParam = HttpContext.Current.Request.Form["id"];

        //    var user = _userList.FirstOrDefault(users => users.UserID == id);
        //    if (user == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return user;
        //}

        ////POST api/Users/RegisterUser
        //[HttpPost]
        //public Users RegisterUser(Users user)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["UserID"];
        //    string nameParam = HttpContext.Current.Request.Form["UserName"];
        //    string emailParam = HttpContext.Current.Request.Form["UserEmail"];

        //    //var userContent = base.ControllerContext.Request.Content.ReadAsFormDataAsync().Result;
        //    var stringContent = base.ControllerContext.Request.Content.ReadAsStringAsync().Result;
        //    return user;
        //}


        ////POST api/Users/register
        //[HttpPost]
        //public string RegisterObject(JObject jData)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["User[UserID]"];
        //    string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
        //    string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
        //    string infoParam = HttpContext.Current.Request.Form["info"];
        //    dynamic json = jData;
        //    JObject jUser = json.User;
        //    string info = json.Info;
        //    var user = jUser.ToObject<Users>();

        //    return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        //}

        //[HttpPost]
        //public string RegisterObjectDynamic(dynamic dynamicData)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["User[UserID]"];
        //    string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
        //    string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
        //    string infoParam = HttpContext.Current.Request.Form["info"];
        //    dynamic json = dynamicData;
        //    JObject jUser = json.User;
        //    string info = json.Info;
        //    var user = jUser.ToObject<Users>();

        //    return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        //}
        //#endregion HttpPost

        //#region HttpPut
        ////POST api/Users/RegisterNonePut
        //[HttpPut]
        //public Users RegisterNonePut()
        //{
        //    return _userList.FirstOrDefault();
        //}

        //[HttpPut]
        //public Users RegisterNoKeyPut([FromBody]int id)
        //{
        //    string idParam = HttpContext.Current.Request.Form["id"];

        //    var user = _userList.FirstOrDefault(users => users.UserID == id);
        //    if (user == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return user;
        //}

        ////POST api/Users/registerPut
        ////只接受一个参数的需要不给key才能拿到
        //[HttpPut]
        //public Users RegisterPut([FromBody]int id)//可以来自FromBody   FromUri
        //                                          //public Users Register(int id)//可以来自url
        //{
        //    string idParam = HttpContext.Current.Request.Form["id"];

        //    var user = _userList.FirstOrDefault(users => users.UserID == id);
        //    if (user == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return user;
        //}

        ////POST api/Users/RegisterUserPut
        //[HttpPut]
        //public Users RegisterUserPut(Users user)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["UserID"];
        //    string nameParam = HttpContext.Current.Request.Form["UserName"];
        //    string emailParam = HttpContext.Current.Request.Form["UserEmail"];

        //    //var userContent = base.ControllerContext.Request.Content.ReadAsFormDataAsync().Result;
        //    var stringContent = base.ControllerContext.Request.Content.ReadAsStringAsync().Result;
        //    return user;
        //}


        ////POST api/Users/registerPut
        //[HttpPut]
        //public string RegisterObjectPut(JObject jData)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["User[UserID]"];
        //    string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
        //    string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
        //    string infoParam = HttpContext.Current.Request.Form["info"];
        //    dynamic json = jData;
        //    JObject jUser = json.User;
        //    string info = json.Info;
        //    var user = jUser.ToObject<Users>();

        //    return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        //}

        //[HttpPut]
        //public string RegisterObjectDynamicPut(dynamic dynamicData)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["User[UserID]"];
        //    string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
        //    string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
        //    string infoParam = HttpContext.Current.Request.Form["info"];
        //    dynamic json = dynamicData;
        //    JObject jUser = json.User;
        //    string info = json.Info;
        //    var user = jUser.ToObject<Users>();

        //    return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        //}
        //#endregion HttpPut

        //#region HttpDelete
        ////POST api/Users/RegisterNoneDelete
        //[HttpDelete]
        //public Users RegisterNoneDelete()
        //{
        //    return _userList.FirstOrDefault();
        //}

        //[HttpDelete]
        //public Users RegisterNoKeyDelete([FromBody]int id)
        //{
        //    string idParam = HttpContext.Current.Request.Form["id"];

        //    var user = _userList.FirstOrDefault(users => users.UserID == id);
        //    if (user == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return user;
        //}

        ////POST api/Users/registerDelete
        ////只接受一个参数的需要不给key才能拿到
        //[HttpDelete]
        //public Users RegisterDelete([FromBody]int id)//可以来自FromBody   FromUri
        //                                             //public Users Register(int id)//可以来自url
        //{
        //    string idParam = HttpContext.Current.Request.Form["id"];

        //    var user = _userList.FirstOrDefault(users => users.UserID == id);
        //    if (user == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return user;
        //}

        ////POST api/Users/RegisterUserDelete
        //[HttpDelete]
        //public Users RegisterUserDelete(Users user)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["UserID"];
        //    string nameParam = HttpContext.Current.Request.Form["UserName"];
        //    string emailParam = HttpContext.Current.Request.Form["UserEmail"];

        //    //var userContent = base.ControllerContext.Request.Content.ReadAsFormDataAsync().Result;
        //    var stringContent = base.ControllerContext.Request.Content.ReadAsStringAsync().Result;
        //    return user;
        //}


        ////POST api/Users/registerDelete
        //[HttpDelete]
        //public string RegisterObjectDelete(JObject jData)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["User[UserID]"];
        //    string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
        //    string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
        //    string infoParam = HttpContext.Current.Request.Form["info"];
        //    dynamic json = jData;
        //    JObject jUser = json.User;
        //    string info = json.Info;
        //    var user = jUser.ToObject<Users>();

        //    return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        //}

        //[HttpDelete]
        //public string RegisterObjectDynamicDelete(dynamic dynamicData)//可以来自FromBody   FromUri
        //{
        //    string idParam = HttpContext.Current.Request.Form["User[UserID]"];
        //    string nameParam = HttpContext.Current.Request.Form["User[UserName]"];
        //    string emailParam = HttpContext.Current.Request.Form["User[UserEmail]"];
        //    string infoParam = HttpContext.Current.Request.Form["info"];
        //    dynamic json = dynamicData;
        //    JObject jUser = json.User;
        //    string info = json.Info;
        //    var user = jUser.ToObject<Users>();

        //    return string.Format("{0}_{1}_{2}_{3}", user.UserID, user.UserName, user.UserEmail, info);
        //}
        //#endregion HttpDelete
        #endregion



    }
    public class Users
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
    public interface IUserService
    {
        object Query(int id);
    }

    public class UserService : IUserService
    {
        object IUserService.Query(int id)
        {
            throw new NotImplementedException();
        }
    }
}
