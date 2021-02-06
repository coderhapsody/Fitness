using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

namespace FitnessManagement.Providers
{
    public class ProspectProvider
    {
        private FitnessDataContext ctx;

        public ProspectProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void AddOrUpdateProspect(int id,
                                        int branchID,
                                        string firstName,
                                        string lastName,
                                        DateTime date,
                                        string identityNo,
                                        string phone1,
                                        string phone2,
                                        string email,
                                        DateTime? dateOfBirth,
                                        int consultantID,
                                        string source,
                                        string sourceRef,
                                        string notes,
                                        bool enableFreeTrial,
                                        DateTime? freeTrialFrom,
                                        DateTime? freeTrialTo)
        {
            var prospect = id == 0 ? new Prospect() : ctx.Prospects.Single(prosp => prosp.ID == id);

            prospect.BranchID = branchID;
            prospect.FirstName = firstName;
            prospect.LastName = lastName;
            prospect.Date = date;
            prospect.IdentityNo = identityNo;
            prospect.Phone1 = phone1;
            prospect.Phone2 = phone2;
            prospect.Email = email;
            prospect.DateOfBirth = dateOfBirth;
            prospect.ConsultantID = consultantID;
            prospect.ProspectSource = source;
            prospect.ProspectRef = sourceRef;
            prospect.Notes = notes;
            prospect.FreeTrialValidFrom = enableFreeTrial
                ? freeTrialFrom.GetValueOrDefault(DateTime.Today)
                : (DateTime?) null;
            prospect.FreeTrialValidTo = enableFreeTrial
                ? freeTrialTo.GetValueOrDefault(DateTime.Today)
                : (DateTime?) null;


            if (id == 0)
            {
                ctx.Prospects.InsertOnSubmit(prospect);
                EntityHelper.SetAuditFieldForInsert(prospect, HttpContext.Current.User.Identity.Name);
            }
            else
            {
                EntityHelper.SetAuditFieldForUpdate(prospect, HttpContext.Current.User.Identity.Name);
            }

            ctx.SubmitChanges();
        }

        public IEnumerable<string> GetProspectSources()
        {
            var sources = new List<string>()
                          {
                              "Flyer",
                              "Friend",
                              "Internet",
                              "TV Ad",
                              "Newspaper Ad",
                              "Walk-in",
                              "Drag-in"
                          };
            return sources.OrderBy(source => source);
        }

        public IEnumerable<string> GetFollowUpVias()
        {
            var sources = new List<string>()
                          {
                              "Phone",
                              "Text",
                              "Email",
                              "Letter",
                              "Gift",
                              "Visit"
                          };
            return sources.OrderBy(source => source);
        }

        public IEnumerable<string> GetFollowUpOutcomes()
        {
            var outcomes = new List<string>()
                           {                               
                               "Appointment",
                               "Appointment Show",
                               "Need Followup",
                               "Dead Prospect",
                               "Hot Prospect",
                               "Become Customer",                               
                           };
            return outcomes;
        }

        public void DeleteProspect(int id)
        {
            var followUps = ctx.ProspectFollowUps.Where(prosp => prosp.ProspectID == id).ToList();
            ctx.ProspectFollowUps.DeleteAllOnSubmit(followUps);

            var prospect = ctx.Prospects.Single(prosp => prosp.ID == id);
            ctx.Prospects.DeleteOnSubmit(prospect);

            ctx.SubmitChanges();
        }

        public Prospect GetProspect(int id)
        {
            return ctx.Prospects.SingleOrDefault(prosp => prosp.ID == id);
        }

        public ProspectFollowUp GetProspectFollowUp(int id)
        {
            return ctx.ProspectFollowUps.SingleOrDefault(prosp => prosp.ID == id);
        }

        public void AddOrUpdateFollowUp(int id,
                                        int prospectID,
                                        int followUpByConsultantID,
                                        DateTime date,
                                        string followUpVia,
                                        string result,
                                        string outcome)
        {
            var followUp = id == 0
                ? new ProspectFollowUp()
                : ctx.ProspectFollowUps.Single(prosp => prosp.ID == id);

            followUp.ConsultantID = followUpByConsultantID;
            followUp.ProspectID = prospectID;
            followUp.Date = date;
            followUp.FollowUpVia = followUpVia;
            followUp.Result = result;
            followUp.Outcome = outcome;

            if (id == 0)
                EntityHelper.SetAuditFieldForInsert(followUp, HttpContext.Current.User.Identity.Name);
            else
                EntityHelper.SetAuditFieldForUpdate(followUp, HttpContext.Current.User.Identity.Name);

            if (id == 0)
                ctx.ProspectFollowUps.InsertOnSubmit(followUp);

            ctx.SubmitChanges();
        }

        public void DeleteFollowUp(int id)
        {
            var prospectFollowUp = ctx.ProspectFollowUps.Single(prosp => prosp.ID == id);
            ctx.ProspectFollowUps.DeleteOnSubmit(prospectFollowUp);
            ctx.SubmitChanges();
        }

        public bool IsDeadProspect(int prospectID)
        {
            var lastFollowUp = ctx.ProspectFollowUps
                               .Where(followUp => followUp.ProspectID == prospectID)
                               .OrderByDescending(followUp => followUp.Date)
                               .FirstOrDefault();
            if(lastFollowUp != null)
                return lastFollowUp.Outcome == "Dead Prospect";

            return false;
        }

        public void ValidateFollowUp(int prospectID, string outcome)
        {
            if (outcome == "Appointment Show")
            {
                bool hasAppointment = ctx.ProspectFollowUps.Any(
                    followup => followup.Outcome == "Appointment" 
                             && followup.ProspectID == prospectID);
                if(!hasAppointment)
                    throw new Exception("Appointment Show can only be created if previous follow up has Appointment entry.");
            }
        }
    }
}