using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YARG.DAL;

namespace YARG.Models
{
    public class MixingFanJob : IJob
    {
        private readonly IConfiguration _config;
        private readonly MixingFanScheduleDAL _mixingFanScheduleDAL;

        public MixingFanJob(IConfiguration configuration)
        {
            _config = configuration;
            _mixingFanScheduleDAL = new(configuration);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (_config.GetValue<string>("QuartzStrings:ExecuteTaskServiceCallSchedulingStatus").Equals("ON"))
            {
                try
                {
                    Console.WriteLine("Searching for mixing fan event(s) for " + DateTime.Now);
                    IEnumerable<MixingFanScheduleCommand> mixingFanScheduleCommands = await _mixingFanScheduleDAL.AreWeThereYetAsync();

                    //Do whatever stuff you want
                    if (mixingFanScheduleCommands != null)
                    {

                        foreach (MixingFanScheduleCommand mfsc in mixingFanScheduleCommands)
                        {

                            StringBuilder sb = new();
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
                                X509Certificate2 caCrt = new(File.ReadAllBytes("ca.crt"));

                                var options = new MqttClientOptionsBuilder()
                                .WithClientId(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
                                .WithTcpServer(_config.GetValue<string>("MQTTStrings:MQTTConnectionIP"), _config.GetValue<int>("MQTTStrings:MQTTPort"))
                                .WithCredentials(_config.GetValue<string>("MQTTStrings:MQTTUsername"), _config.GetValue<string>("MQTTStrings:MQTTPassword"))
                                .WithCleanSession()
                                .WithTls(new MqttClientOptionsBuilderTlsParameters()
                                {
                                    UseTls = true,
                                    SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
                                    CertificateValidationHandler = (certContext) => {
                                        X509Chain chain = new();
                                        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
                                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
                                        chain.ChainPolicy.VerificationTime = DateTime.Now;
                                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 0);
                                        chain.ChainPolicy.CustomTrustStore.Add(caCrt);
                                        chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;

                                        // convert provided X509Certificate to X509Certificate2
                                        var x5092 = new X509Certificate2(certContext.Certificate);

                                        return chain.Build(x5092);
                                    }
                                }
    )
                                .Build();

                                using (var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                                {
                                    try
                                    {
                                        await mqttClient.ConnectAsync(options, cancellationToken.Token).ConfigureAwait(false);
                                    }
                                    catch(Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }

                                    Console.WriteLine("Connected to MQTT broker.");
                                }

                                var message = new MqttApplicationMessageBuilder()
                                .WithTopic("mixingFan/" + MQTT_Topic_Suffix)
                                .WithPayload(sb.ToString())
                                .WithExactlyOnceQoS()
                                .WithRetainFlag()
                                .Build();

                                Console.WriteLine("Broadcasting MQTT " + sb.ToString() + " for mixingFan/" + MQTT_Topic_Suffix);
                                try
                                {
                                    await mqttClient.PublishAsync(message);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                try
                                {
                                    await mqttClient.DisconnectAsync();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
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
        }
    }
}
