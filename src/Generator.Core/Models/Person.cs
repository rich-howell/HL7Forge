using System.Text.Json.Serialization;

namespace HL7Forge.Core.Models;

public class Person
{
    [JsonPropertyName("mrn")] public string MRN { get; set; } = "";
    [JsonPropertyName("identifiers")] public List<PersonIdentifier> Identifiers { get; set; } = new();
    [JsonPropertyName("names")] public List<PersonName> Names { get; set; } = new(); // repetitions -> PID-5
    [JsonPropertyName("addresses")] public List<PersonAddress> Addresses { get; set; } = new(); // repetitions -> PID-11
    [JsonPropertyName("telecoms")] public List<PersonTelecom> Telecoms { get; set; } = new();
    [JsonPropertyName("dob")] public string Dob { get; set; } = "19800101"; // YYYYMMDD
    [JsonPropertyName("sex")] public string Sex { get; set; } = "U";

    // New demographic fields
    [JsonPropertyName("race")] public List<PersonCode> Race { get; set; } = new();         // -> PID-10 (repeating CE)
    [JsonPropertyName("ethnicity")] public List<PersonCode> Ethnicity { get; set; } = new(); // -> PID-22 (repeating CE)
    [JsonPropertyName("language")] public string Language { get; set; } = "en";            // -> PID-15 (CE), ISO639-1 suggested
}

public class PersonIdentifier
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("assigningAuthority")] public string AssigningAuthority { get; set; } = "DUMMY.FAC";
    [JsonPropertyName("typeCode")] public string TypeCode { get; set; } = "MR";
}

public class PersonName
{
    [JsonPropertyName("family")] public string Family { get; set; } = "";
    [JsonPropertyName("given")] public string Given { get; set; } = "";
    [JsonPropertyName("middle")] public string Middle { get; set; } = "";
    [JsonPropertyName("suffix")] public string Suffix { get; set; } = "";
    [JsonPropertyName("use")] public string Use { get; set; } = "L"; // L=legal, A=alias, M=maiden, etc.
}

public class PersonAddress
{
    [JsonPropertyName("line1")] public string Line1 { get; set; } = "";
    [JsonPropertyName("line2")] public string Line2 { get; set; } = "";
    [JsonPropertyName("city")] public string City { get; set; } = "";
    [JsonPropertyName("state")] public string State { get; set; } = "";
    [JsonPropertyName("postcode")] public string Postcode { get; set; } = "ZZ1 1ZZ";
    [JsonPropertyName("country")] public string Country { get; set; } = "GBR";
    [JsonPropertyName("use")] public string Use { get; set; } = "H"; // H=home, T=temporary, B=business
}

public class PersonTelecom
{
    [JsonPropertyName("value")] public string Value { get; set; } = "07000 000000";
    [JsonPropertyName("use")] public string Use { get; set; } = "H"; // home
}

public class PersonCode
{
    [JsonPropertyName("code")] public string Code { get; set; } = "";
    [JsonPropertyName("text")] public string Text { get; set; } = "";
    [JsonPropertyName("system")] public string System { get; set; } = "LCL";
}
