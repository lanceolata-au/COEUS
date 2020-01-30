using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using carbon.core.domain.model.registration.medical;
using carbon.core.Features;

namespace carbon.core.domain.model.registration
{
    public class ApplicationMedical : Entity<int>
    {
        [Column("Id")]
        public int ApplicationId { get; private set; }
        public virtual IEnumerable<Allergy> Allergies { get; private set; }
        public virtual IEnumerable<Condition> Conditions { get; private set; }
        public virtual IEnumerable<MedicalAid> MedicalAids { get; private set; }
    }
}