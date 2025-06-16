namespace WebApplication1.Services
{
    using Google.Cloud.AIPlatform.V1;
    using Google.Protobuf;
    using System.Text.Json;

    public class GeminiVertexService
    {
        private readonly PredictionServiceClient _client;
        private readonly string _projectId = "monitor-pya";
        private readonly string _location = "us-central1"; // Puedes usar otro si es tu caso
        private readonly string _model = "gemini-pro"; // Modelo Gemini

        public GeminiVertexService()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "monitor-pya-229413e77f22.json");
            _client = PredictionServiceClient.Create();
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var endpoint = EndpointName.FromProjectLocationPublisherModel(
                _projectId, _location, "google", _model
            );

            // Crear instancia como un Struct
            var instance = new Google.Protobuf.WellKnownTypes.Struct
            {
                Fields =
            {
                { "prompt", Google.Protobuf.WellKnownTypes.Value.ForString(prompt) },
                { "temperature", Google.Protobuf.WellKnownTypes.Value.ForNumber(0.7) },
                { "maxOutputTokens", Google.Protobuf.WellKnownTypes.Value.ForNumber(512) }
            }
            };


            var request = new PredictRequest
            {
                EndpointAsEndpointName = endpoint
            };

            request.Instances.Add(Google.Protobuf.WellKnownTypes.Value.ForStruct(instance));

            var response = await _client.PredictAsync(request);

            // Aquí depende del contenido exacto de la respuesta
            var predictionStruct = response.Predictions[0].StructValue;

            if (predictionStruct.Fields.TryGetValue("content", out var contentValue))
            {
                return contentValue.StringValue;
            }

            return "[Sin respuesta del modelo]";
        }

    }
}
