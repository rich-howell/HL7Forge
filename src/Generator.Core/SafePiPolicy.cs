// src/Generator.Core/SafePiPolicy.cs
namespace HL7Forge.Core
{
    public class SafePiPolicy
    {
        public string AssignAuthority { get; set; } = "DUMMY.FAC";
        public string DefaultSendingApplication { get; set; } = "DummyGen";
        public string DefaultSendingFacility { get; set; } = "DUMMY.FAC";
        public string DefaultReceivingApplication { get; set; } = "Rhapsody";
        public string DefaultReceivingFacility { get; set; } = "TEST";

        public string SafePhone() => "07000 000000";
        public string SafePostcode() => "ZZ1 1ZZ";

        public void Apply(ConstantsStore constants)
        {
            if (constants?.Root is not null)
            {
                AssignAuthority = constants.GetString("assigningAuthority", AssignAuthority);
                DefaultSendingApplication = constants.GetString("sendingApplication", DefaultSendingApplication);
                DefaultSendingFacility = constants.GetString("sendingFacility", DefaultSendingFacility);
                DefaultReceivingApplication = constants.GetString("receivingApplication", DefaultReceivingApplication);
                DefaultReceivingFacility = constants.GetString("receivingFacility", DefaultReceivingFacility);
            }
        }
    }
}
