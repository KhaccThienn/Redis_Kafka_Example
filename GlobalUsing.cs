global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using System.Text.Json;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Distributed;

global using Confluent.Kafka;
global using static Confluent.Kafka.ConfigPropertyNames;
global using Manonero.MessageBus.Kafka.Settings;
global using Manonero.MessageBus.Kafka.Abstractions;
global using StackExchange.Redis;

global using Remake_Kafka_Example_01.BackgroundTasks;
global using Remake_Kafka_Example_01.Core.Domains;
global using Remake_Kafka_Example_01.Core.DTOs;
global using Remake_Kafka_Example_01.Core.Interfaces;
global using Remake_Kafka_Example_01.Core.Interfaces.DatabaseCommand;
global using Remake_Kafka_Example_01.Core.Interfaces.MemoryCommand;
global using Remake_Kafka_Example_01.Core.Producers;
global using Remake_Kafka_Example_01.Core.Request;
global using Remake_Kafka_Example_01.Core.Services;
global using Remake_Kafka_Example_01.DatabaseCommands;
global using Remake_Kafka_Example_01.DatabaseCommands.Handlers;
global using Remake_Kafka_Example_01.Extensions;
global using Remake_Kafka_Example_01.MemoryCommands;
global using Remake_Kafka_Example_01.MemoryCommands.Handlers;
global using Remake_Kafka_Example_01.Settings;