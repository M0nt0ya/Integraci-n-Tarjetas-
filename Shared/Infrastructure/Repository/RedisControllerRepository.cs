using StackExchange.Redis;
using System;

public class RedisControllerRepository
{
    private readonly string hostRedis;
    private readonly ConnectionMultiplexer redis;
    private readonly IDatabase db;

    public RedisControllerRepository(IConfiguration configuration)
    {
        hostRedis = configuration["Redis:Host"] ?? "localhost";
        var options = new ConfigurationOptions
        {
            EndPoints = { hostRedis },
            AbortOnConnectFail = false
        };
        redis = ConnectionMultiplexer.Connect(options);
        db = redis.GetDatabase();
    }


    public void SetData(string key, string value)
    {
        // Almacenar un valor simple
        db.StringSet(key, value);
    }

    public string GetData(string key)
    {
        var data = db.StringGet(key);
        string dataString = data.IsNullOrEmpty ? "" : data.ToString();
        return dataString;
    }



    public bool KeyExists(string key)
    {
        // Implementación del patrón Criteria
        return db.KeyExists(key);
    }
}