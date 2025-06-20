using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Alprog2.Models;

namespace Alprog2.Services
{
    class MongoService
    {
        private readonly IMongoCollection<TemperatureModel> _temperatureCollection;

        public MongoService()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Alprog");
            _temperatureCollection = database.GetCollection<TemperatureModel>("Temperatur");
        }

        public async Task<List<TemperatureModel>> GetAllDataAsync()
        {
            return await _temperatureCollection.Find(_ => true).SortByDescending(d => d.Waktu).ToListAsync();
        }
        public async Task<TemperatureModel?> GetLatestDataAsync()
        {
            return await _temperatureCollection.Find(_ => true)
                .SortByDescending(d => d.Waktu)
                .Limit(1)
                .FirstOrDefaultAsync();
        }
        public async Task DeleteDataAsync(string id)
        {
            await _temperatureCollection.DeleteOneAsync(d => d.Id == id);
        }
        public async Task UpdateDataAsync(TemperatureModel updated)
        {
            var filter = Builders<TemperatureModel>.Filter.Eq(d => d.Id, updated.Id);
            await _temperatureCollection.ReplaceOneAsync(filter, updated);
        }


        public static class AppState
        {
            public static string SelectedComPort { get; set; } = null;
        }


    }
}
