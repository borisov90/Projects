using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Models.DataView;

namespace ETEMModel.Helpers.Admin
{
    public class PersonHistoryBL : BaseClassBL<PersonHistory>
    {
        public PersonHistoryBL()
        {
            this.EntitySetName = "PersonHistories";
        }

        internal override PersonHistory GetEntityById(int idEntity)
        {
            return this.dbContext.PersonHistories.Where(e => e.idPersonHistory == idEntity).FirstOrDefault();
        }

        public List<PersonHistoryDataView> GetPersonHistoryDataViewByPersonID(int idPerson)
        {
            List<PersonHistoryDataView> list = (from p in this.dbContext.PersonHistories
                                                where p.idPerson == idPerson
                                                select new PersonHistoryDataView
                                                {
                                                    idPersonHistory = p.idPersonHistory,
                                                    idPerson = p.idPerson,
                                                    FirstName = p.FirstName,
                                                    SecondName = p.SecondName,
                                                    LastName = p.LastName,
                                                    IdentityNumber = p.IdentityNumber,
                                                    IdentityNumberDate = p.IdentityNumberDate,
                                                    IdentityNumberIssueBy = p.IdentityNumberIssueBy,
                                                    CreationDate = p.CreationDate,


                                                }).ToList();
            return list;
        }

        internal override void EntityToEntity(PersonHistory sourceEntity, PersonHistory targetEntity)
        {



            targetEntity.FirstName = sourceEntity.FirstName;
            targetEntity.SecondName = sourceEntity.SecondName;
            targetEntity.LastName = sourceEntity.LastName;
            targetEntity.IdentityNumber = sourceEntity.IdentityNumber;
            targetEntity.IdentityNumberDate = sourceEntity.IdentityNumberDate;
            targetEntity.IdentityNumberIssueBy = sourceEntity.IdentityNumberIssueBy;
            targetEntity.idCreateUser = sourceEntity.idCreateUser;
            targetEntity.CreationDate = sourceEntity.CreationDate;
            targetEntity.idModifyUser = sourceEntity.idModifyUser;
            targetEntity.ModifyDate = sourceEntity.ModifyDate;



        }


    }
}