using YARG.DAL;
using Quartz;
using System;
using System.Configuration;
using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Client.Options;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace YARG.Models
{
    public class ExecuteTaskServiceCallJob : IJob
    {
        private readonly IConfiguration _config;
        private readonly WateringScheduleDAL _wateringScheduleDAL;
        private readonly MixingFanScheduleDAL _mixingFanScheduleDAL;


        public ExecuteTaskServiceCallJob(IConfiguration configuration)
        {
            _config = configuration;
            _wateringScheduleDAL = new(configuration);
            _mixingFanScheduleDAL = new(configuration);
        }

        public Task Execute(IJobExecutionContext context)
        {
            //TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            Task task = Task.Run(async () =>
            {
                if (_config.GetValue<string>("QuartzStrings:ExecuteTaskServiceCallSchedulingStatus").Equals("ON"))
                {
                    try
                    {
                        Console.WriteLine("Searching for mixing fan event(s) for " + DateTime.Now);
                        IEnumerable<MixingFanScheduleCommand> mixingFanScheduleCommands = _mixingFanScheduleDAL.AreWeThereYet();

                        //Do whatever stuff you want
                        if(mixingFanScheduleCommands != null)
                        {

                            foreach(MixingFanScheduleCommand mfsc in mixingFanScheduleCommands)
                            {
                           
                            StringBuilder sb = new StringBuilder();
                                string MQTT_Topic_Suffix = "MF" + mfsc.FanNumber.ToString();
                                Console.WriteLine("Found mixing fan event for " + MQTT_Topic_Suffix);

                                sb.Append(mfsc.PumpSpeed);
                                sb.Append(':');
                                sb.Append(mfsc.OverSpeed);
                                sb.Append(':');
                                sb.Append(mfsc.Duration);
                                sb.Append(':');
                                sb.Append(mfsc.MixingFanScheduleID);

                                try
                                {
                                    var factory = new MqttFactory();
                                    var mqttClient = factory.CreateMqttClient();

                                    var options = new MqttClientOptionsBuilder()
                                    .WithClientId(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
                                    .WithTcpServer(_config.GetValue<string>("MQTTStrings:MQTTConnectionIP"), _config.GetValue<int>("MQTTStrings:MQTTPort"))
                                    .WithCredentials(_config.GetValue<string>("MQTTStrings:MQTTUsername"), _config.GetValue<string>("MQTTStrings:MQTTPassword"))
                                    .WithCleanSession()
                                    .Build();

                                    using (var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
                                    {
                                        await mqttClient.ConnectAsync(options, cancellationToken.Token).ConfigureAwait(false);
                                        Console.WriteLine("Connected to MQTT broker.");
                                    }

                                    var message = new MqttApplicationMessageBuilder()
                                    .WithTopic("mixingFan/" + MQTT_Topic_Suffix)
                                    .WithPayload(sb.ToString())
                                    .WithExactlyOnceQoS()
                                    .WithRetainFlag()
                                    .Build();

                                    Console.WriteLine("Broadcasting MQTT "+ sb.ToString() + " for mixingFan/" + MQTT_Topic_Suffix);
                                    await mqttClient.PublishAsync(message);

                                    await mqttClient.DisconnectAsync();
                                    Console.WriteLine("Disconnected from MQTT broker.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error!");
                                    Console.WriteLine(ex.ToString() + " " + ex.Message);
                                }
                            }


                           // tcs.SetResult();
                        }                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error!");
                        Console.WriteLine(ex.ToString() + " " + ex.Message);
                    }


                }

                //tcs.SetResult(1);
            });
            return Task.CompletedTask;
            //return task;
            //return tcs.Task;
            
        }

        //private async void SendMQTT(ConnectionStringManager connectionStringManager,string strTopic, string strMessage)
        //{
        //    try
        //    {
        //        var factory = new MqttFactory();
        //        var mqttClient = factory.CreateMqttClient();

        //        var options = new MqttClientOptionsBuilder()
        //        .WithClientId(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
        //        .WithTcpServer(connectionStringManager.GetMQTTServerIP(), Int32.Parse(connectionStringManager.GetMQTTPort()))
        //        .WithCredentials(connectionStringManager.GetMQTTUsername(), connectionStringManager.GetMQTTPassword())
        //        .WithCleanSession()
        //        .Build();

        //        using (var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(1)))
        //        {
        //            await mqttClient.ConnectAsync(options, cancellationToken.Token).ConfigureAwait(false);
        //        }


        //        var message = new MqttApplicationMessageBuilder()
        //        .WithTopic(strTopic)
        //        .WithPayload(strMessage)
        //        .WithExactlyOnceQoS()
        //        .WithRetainFlag()
        //        .Build();

        //        await mqttClient.PublishAsync(message);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error!");
        //        Console.WriteLine(ex.ToString() + " " + ex.Message);
        //    }

        //}

    }
}