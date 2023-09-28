using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace YARG.Data.Services
{


    public class MqttPublisherService
    {
        private readonly IMqttClient _mqttClient;

        public MqttPublisherService(IMqttClient mqttClient)
        {
            _mqttClient = mqttClient;
        }

        public async Task PublishMessageAsync(string topic, string message, MqttQualityOfServiceLevel qosLevel = MqttQualityOfServiceLevel.AtLeastOnce)
        {
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithQualityOfServiceLevel(qosLevel)
                .Build();

            await _mqttClient.PublishAsync(mqttMessage);
        }
    }
}