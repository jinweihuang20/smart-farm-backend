using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Smart_farm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        Model.DBContext db;

        public DataController(IConfiguration config)
        {
            db = new Model.DBContext(config);
        }

        [HttpGet]
        public async Task<Model.RecordData> Get()
        {
            return db.recordDatas.OrderBy(r => r.datetime).Last();
        }

        [HttpGet("Push")]
        public async Task<Model.RecordData> Push(double humidity,int raw)
        {
            return db.recordDatas.OrderBy(r => r.datetime).Last();
        }

        [HttpGet("Query")]
        public async Task<Model.RecordData[]> Query(DateTime from , DateTime to)
        {
            return db.recordDatas.OrderBy(r => r.datetime).Where(r => r.datetime >= from && r.datetime <= to).ToArray();
        }
    }
}
