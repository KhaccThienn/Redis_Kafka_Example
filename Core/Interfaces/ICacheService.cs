﻿namespace Remake_Kafka_Example_01.Core.Interfaces
{
    public interface ICacheService
    {
        T GetData<T>(string key);

        bool SetData<T>(string key, T value, DateTimeOffset? expirationTime);

        object RemoveData(string key);
    }
}
