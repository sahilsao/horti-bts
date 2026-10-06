using System;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using HortiBts.Shared.Common;
using HortiBts.Shared.Models.Soil;
using Microsoft.Extensions.Options;

namespace HortiBts.Api.Services.Soil;

public sealed class SoilServiceOptions
{
    public string Endpoint { get; set; } = "https://bhuiyan.cg.nic.in/GetSoilDetails.asmx";
    public string DeptId { get; set; } = "fooddept";
    public string Key { get; set; } = "food@^652!192";
}

public interface ISoilDetailsService
{
    Task<Result<List<SoilDetailsDto>>> GetAsync(string villageCode, string khasraNo, CancellationToken ct);
}

public sealed class SoilDetailsService(
    HttpClient http,
    IOptions<SoilServiceOptions> options,
    ILogger<SoilDetailsService> logger) : ISoilDetailsService
{
    private const string Operation = "GetSoilDetailsWSJSon";
    private static readonly XNamespace Soap = "http://schemas.xmlsoap.org/soap/envelope/";
    private static readonly XNamespace Tns = "http://cg.nic.in/bhuiyan/";
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<Result<List<SoilDetailsDto>>> GetAsync(string villageCode, string khasraNo, CancellationToken ct)
    {
        try
        {
            var o = options.Value;

            var envelope = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement(Soap + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", Soap.NamespaceName),
                    new XElement(Soap + "Body",
                        new XElement(Tns + Operation,
                            new XElement(Tns + "VillageCensuscode", villageCode),
                            new XElement(Tns + "KhasraNo", khasraNo),
                            new XElement(Tns + "deptid", o.DeptId),
                            new XElement(Tns + "key", o.Key)))));

            using var content = new StringContent(
                envelope.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", $"\"{Tns.NamespaceName}{Operation}\"");

            using var response = await http.PostAsync(o.Endpoint, content, ct);
            var body = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("{Op} returned {Status}: {Body}", Operation, (int)response.StatusCode, body);
                return Result<List<SoilDetailsDto>>.Failure($"Land record service returned {(int)response.StatusCode}.");
            }

            var json = XDocument.Parse(body)
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == $"{Operation}Result")?.Value;

            if (string.IsNullOrWhiteSpace(json))
                return Result<List<SoilDetailsDto>>.Failure("Land record service returned an empty result.");

            var records = JsonSerializer.Deserialize<List<SoilDetailsDto>>(json, JsonOptions) ?? [];

            // Status "0" means no record found
            if (records.Count > 0 && records[0].Status == "0")
                records = [];

            return Result<List<SoilDetailsDto>>.Success(records);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Op} failed for village {Village}, khasra {Khasra}", Operation, villageCode, khasraNo);
            return Result<List<SoilDetailsDto>>.Failure($"Soil lookup failed: {ex.Message}");
        }
    }
}


