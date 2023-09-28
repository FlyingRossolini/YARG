using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;
using YARG.Common_Types;

namespace YARG.Services
{
    public class MqttClientProvider
    {
        private readonly IConfiguration _config;
        private bool _isReconnecting;

        public MqttClientProvider(IConfiguration config)
        {
            _config = config;
        }

        public IMqttClient CreateMqttClient()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            mqttClient.UseDisconnectedHandler(async e =>
            {
                // Handle disconnection here
                Console.WriteLine("MQTT client disconnected. Reconnecting...");
                await ReconnectMqttClient(mqttClient);
            });

            ConnectMqttClient(mqttClient);

            return mqttClient;
        }

        private async Task ReconnectMqttClient(IMqttClient mqttClient)
        {
            if (mqttClient.IsConnected || _isReconnecting)
            {
                return; // Return if already connected or reconnecting
            }

            try
            {
                _isReconnecting = true;

                int reconnectAttempts = 0;
                int maxReconnectAttempts = 25; // You can adjust this value

                while (!mqttClient.IsConnected && reconnectAttempts < maxReconnectAttempts)
                {
                    try
                    {
                        // Attempt to reconnect with exponential backoff
                        await mqttClient.ConnectAsync(CreateMqttClientOptions());
                        Console.WriteLine("MQTT client reconnected.");

                        // Resubscribe to MQTT topics after reconnecting
                        await Global.SubscribeToTopics(mqttClient);
                        _isReconnecting = false;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"MQTT client reconnection failed: {ex.Message}");
                        _isReconnecting = false;
                    }

                    // Calculate exponential backoff delay
                    int delaySeconds = (int)Math.Pow(2, reconnectAttempts); // 2^0, 2^1, 2^2, ...
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));

                    reconnectAttempts++;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"MQTT client reconnection errored out: {e.Message}");
                _isReconnecting = false;
                throw;
            }        
        }

        private void ConnectMqttClient(IMqttClient mqttClient)
        {
            //var caCrt = new X509Certificate2(File.ReadAllBytes("ca.crt"));
            //var options = CreateMqttClientOptions();

            mqttClient.ConnectAsync(CreateMqttClientOptions()).GetAwaiter().GetResult();
        }

        private IMqttClientOptions CreateMqttClientOptions()
        {
            {
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
                        CertificateValidationHandler = (certContext) =>
                        {
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
                    })
                    .Build();

                return options;
            }
        }
    }
}
