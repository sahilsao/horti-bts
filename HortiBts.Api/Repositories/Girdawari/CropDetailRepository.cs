using HortiBts.Shared.Common;
using HortiBts.Shared.Models.Girdawari;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace HortiBts.Api.Repositories.Girdawari
{
    public interface ICropDetailRepository
    {
        /// <summary>
        /// Looks up crop details for each search param, one at a time (matching
        /// the original async.eachSeries sequential behavior), and returns only
        /// the records whose status != 0.
        /// </summary>
        Task<Result<List<CropDetailRecord[]>>> GetCropDetailsAsync(
            IReadOnlyList<SearchParam> searchParams,
            CancellationToken cancellationToken);

    }

    public sealed class CropDetailRepository(HttpClient httpClient, ILogger<CropDetailRepository> logger) : ICropDetailRepository
    {
        // Confirmed against https://bhuiyan.cg.nic.in/GetSoilDetails.asmx?op=GetCropDetailWSJSon
        private const string WsdlEndpoint = "https://bhuiyan.cg.nic.in/GetSoilDetails.asmx";
        private const string SoapAction = "http://cg.nic.in/bhuiyan/GetCropDetailWSJSon";
        private const string DeptId = "fooddept";
        private const string Key = "food@^652!192";

        private static readonly XNamespace Soap = "http://schemas.xmlsoap.org/soap/envelope/";
        private static readonly XNamespace Tns = "http://cg.nic.in/bhuiyan/";

        public async Task<Result<List<CropDetailRecord[]>>> GetCropDetailsAsync(
            IReadOnlyList<SearchParam> searchParams,
            CancellationToken cancellationToken)
        {
            var results = new List<CropDetailRecord[]>();

            // Sequential, like the original async.eachSeries — the upstream
            // service likely can't handle concurrent calls per session/key.
            foreach (var param in searchParams)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Result<List<CropDetailRecord[]>>.Failure("Request was cancelled.");
                }

                CropDetailRecord[] parsed;
                try
                {
                    parsed = await CallGetCropDetailWSJSonAsync(param, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex,
                        "GetCropDetailWSJSon failed for village {VillageCode}, khasra {KhasraNo}",
                        param.VillageCode, param.KhasraNo);

                    // Fail the whole batch, matching the original's behavior of
                    // stopping the eachSeries loop on the first error.
                    return Result<List<CropDetailRecord[]>>.Failure(
                        $"Lookup failed for village {param.VillageCode}, khasra {param.KhasraNo}: {ex.Message}");
                }

                if (parsed.Length > 0 && parsed[0].Status != 0)
                {
                    results.Add(parsed);
                }
            }

            return Result<List<CropDetailRecord[]>>.Success(results);
        }

        private async Task<CropDetailRecord[]> CallGetCropDetailWSJSonAsync(
            SearchParam param,
            CancellationToken cancellationToken)
        {
            var envelope = new XDocument(
                new XElement(Soap + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", Soap.NamespaceName),
                    new XElement(Soap + "Body",
                        new XElement(Tns + "GetCropDetailWSJSon",
                            new XAttribute(XNamespace.Xmlns + "", Tns.NamespaceName),
                            new XElement(Tns + "VillageCensuscode", param.VillageCode),
                            new XElement(Tns + "KhasraNo", param.KhasraNo),
                            new XElement(Tns + "deptid", DeptId),
                            new XElement(Tns + "key", Key)))));

            using var content = new StringContent(envelope.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", SoapAction);

            using var response = await httpClient.PostAsync(WsdlEndpoint, content, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"SOAP call failed with status {(int)response.StatusCode}: {body}");
            }

            var resultXml = XDocument.Parse(body);
            var resultElement = resultXml
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "GetCropDetailWSJSonResult");

            if (resultElement is null || string.IsNullOrWhiteSpace(resultElement.Value))
            {
                throw new InvalidOperationException("SOAP response did not contain GetCropDetailWSJSonResult.");
            }

            // The service embeds a JSON string inside the XML result node.
            var jsonPayload = resultElement.Value;
            var records = JsonSerializer.Deserialize<CropDetailRecord[]>(jsonPayload)
                ?? throw new InvalidOperationException("Failed to parse GetCropDetailWSJSonResult JSON payload.");

            return records;
        }
    }

}
