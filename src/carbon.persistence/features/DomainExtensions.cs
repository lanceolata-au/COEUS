using System.Linq;
using carbon.core.domain.model.registration;
using carbon.core.execeptions;
using carbon.persistence.interfaces;

namespace carbon.persistence.features
{
    
    /*
     * This is likely going to be a very large file, it is for linking all virtual objects against their types
     * however I think it is prudent to leave a note in here such that the structure of this can be reviewed
     * at a later date.
     *
     * Therefore TODO review if this file needs to be broken up
     *
     * @Zeryter
     * 
     */
    
    public static class DomainExtensions
    {
        public static void LinkApplicationItems(this Application application, IReadOnlyRepository repository, bool hasWritePermission = false)
        {
            if (repository.Table<ApplicationMedical,int>().Any(am=> am.ApplicationId == application.Id))
            {
                application.ApplicationMedical = repository.Table<ApplicationMedical, int>()
                    .FirstOrDefault(am => am.ApplicationId == application.Id);
            }
            else
            {
                if (hasWritePermission)
                {

                    var medicalApplication = ApplicationMedical.Create(application.Id);
                    
                    ((IReadWriteRepository) repository).Create<ApplicationMedical, int>(medicalApplication);

                    application.ApplicationMedical = medicalApplication;

                }
                else
                {
                    throw new CarbonDomainException("Medical Section does not exist for event Id: " 
                                                    + application.Id + " and no write permission exists");
                    
                }
            }
        }
        
    }
}