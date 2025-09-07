using System.Security.Cryptography;

namespace HL7Forge.Core;

public class DataFaker
{
    private readonly SafePiPolicy _policy;
    private readonly ConstantsStore? _constants;
    public DataFaker(SafePiPolicy policy, ConstantsStore? constants = null) { _policy = policy; _constants = constants; }

    private static Random CreateRng(int seed) => new(seed);
    private static T Pick<T>(Random rng, T[] items) => items[rng.Next(items.Length)];

    public FakePatient CreatePatient(int seed)
    {
        var rng = CreateRng(seed);
        var family = Pick(rng, new[]{"FALCON","RIVET","SPROCKET","WINTER","MAPLE","COPPER"});
        var given = Pick(rng, new[]{"ALBA","JASPER","ROWAN","EDEN","BLAKE","RIVER"});
        var middle = Pick(rng, new[]{"A.","B.","C.","D.","E.","F."});
        var suffix = "";
        var line1 = $"{rng.Next(1,199)} Example Street";
        var line2 = "";
        var city = Pick(rng, new[]{"Testford","Mockham","Faketon"});
        var state = "ZZ";
        var postcode = _policy.SafePostcode();
        var country = "GBR";

        var dob = DateTime.UtcNow.Date.AddYears(-rng.Next(1, 90)).AddDays(-rng.Next(0,365));
        var sex = Pick(rng, new[]{"M","F","U"});
        var mrn = $"MRN{rng.Next(100000,999999)}";
        var phone = _policy.SafePhone();

        return new FakePatient(
            MRN: mrn,
            Family: family, Given: given, Middle: middle, Suffix: suffix,
            Address: new FakeAddress(line1, line2, city, state, postcode, country),
            Dob: dob, Sex: sex, Phone: phone
        );
    }

    public FakeVisit CreateVisit(int seed)
    {
        var rng = CreateRng(seed);
        var poc = Pick(rng, new[]{"WARD1","WARD2","ED","OPD"});
        var room = $"{rng.Next(1,20)}";
        var bed = $"{rng.Next(1,30)}";
        var fac = "DUMMY.FAC";
        var visitNumber = $"V{rng.Next(10000,99999)}";
        return new FakeVisit(new FakePl(poc, room, bed, fac), visitNumber);
    }

    public FakeOrder CreateOrder(int seed)
    {
        var rng = CreateRng(seed);
        var placer = $"P{rng.Next(100000,999999)}";
        var filler = $"F{rng.Next(100000,999999)}";
        var priority = Pick(rng, new[]{"R","S","A"}); // routine/stat/asap (demo)
        var ordDt = DateTime.UtcNow.AddMinutes(-rng.Next(0, 120));
        var providerId = $"PROV{rng.Next(1000,9999)}";
        var providerName = Pick(rng, new[]{"SMITH^ALEX","TAYLOR^SAM","MORGAN^JAY"});
        return new FakeOrder(placer, filler, priority, ordDt, providerId, providerName);
    }

    public FakeObservationRequest CreateObr(int seed)
    {
        var rng = CreateRng(seed);
        var setId = "1";
        var universalService = Pick(rng, new[]{"GLU^Glucose^LCL","HB^Hemoglobin^LCL","WBC^WhiteBloodCount^LCL"});
        var obsDt = DateTime.UtcNow.AddMinutes(-rng.Next(0, 90));
        var resultDt = obsDt.AddMinutes(rng.Next(5, 30));
        return new FakeObservationRequest(setId, universalService, obsDt, resultDt);
    }

    public List<FakeObservation> CreateObxPanel(int seed)
    {
        var rng = CreateRng(seed);
        var candidates = new List<FakeObservation>
        {
            new("1","NM","GLU","Glucose","5."+rng.Next(0,9),"mmol/L","4.0-7.0","N"),
            new("2","NM","HB","Hemoglobin",$"{rng.Next(120,160)}","g/L","120-160","N"),
            new("3","NM","WBC","White Blood Count",$"{rng.Next(4,11)}","10^9/L","4-11","N")
        };
        // Pick 1-3 randomly
        int take = rng.Next(1, 4);
        return candidates.OrderBy(_=>rng.Next()).Take(take).ToList();
    }

    public FakeDiagnosis CreateDiagnosis(int seed)
    {
        var rng = CreateRng(seed);
        var code = Pick(rng, new[]{"DIA1","DIA2","DIA3"});
        var text = Pick(rng, new[]{"Test Diagnosis One","Sample Diagnosis Two","Mock Condition Three"});
        var diagDt = DateTime.UtcNow.AddDays(-rng.Next(0,30));
        return new FakeDiagnosis(code, text, diagDt);
    }

    public FakeAllergy CreateAllergy(int seed)
    {
        var rng = CreateRng(seed);
        var code = Pick(rng, new[]{"ALG1","ALG2","ALG3"});
        var text = Pick(rng, new[]{"Peanuts","Penicillin","Latex"});
        var severity = Pick(rng, new[]{"MI","MO","SE"}); // mild/moderate/severe demo
        return new FakeAllergy(code, text, severity);
    }

    public string CreateNote(int seed)
    {
        var rng = CreateRng(seed);
        var sentences = new[]{
            "This is a demonstration note.",
            "Data generated is synthetic.",
            "For testing non-production interfaces."
        };
        return Pick(rng, sentences);
    }
}

public record FakeAddress(string Line1, string Line2, string City, string State, string Postcode, string Country);
public record FakePatient(string MRN, string Family, string Given, string Middle, string Suffix, FakeAddress Address, DateTime Dob, string Sex, string Phone);
public record FakePl(string PointOfCare, string Room, string Bed, string Facility);
public record FakeVisit(FakePl Location, string VisitNumber);

public record FakeOrder(string PlacerOrderNumber, string FillerOrderNumber, string Priority, DateTime OrderDateTime, string OrderingProviderId, string OrderingProviderName);
public record FakeObservationRequest(string SetId, string UniversalService, DateTime ObservationDateTime, DateTime ResultDateTime);
public record FakeObservation(string SetId, string ValueType, string Code, string Text, string Value, string Units, string ReferenceRange, string AbnormalFlags);
public record FakeDiagnosis(string Code, string Text, DateTime DiagnosisDateTime);
public record FakeAllergy(string Code, string Text, string Severity);
