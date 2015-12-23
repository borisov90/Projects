using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Admin
{
    public class PersonBL : BaseClassBL<Person>
    {
        public PersonBL()
        {
            this.EntitySetName = "Persons";
        }

        internal override Person GetEntityById(int idEntity)
        {
            return this.dbContext.Persons.Where(e => e.idPerson == idEntity).FirstOrDefault();
        }

        public Person GetPersonByPersonID(string idEntity)
        {
            return GetEntityById(Int32.Parse(idEntity));
        }

        internal Person GetPersonByIdentityNumber(string identityNumber)
        {
            return this.dbContext.Persons.Where(e => e.IdentityNumber == identityNumber).FirstOrDefault();
        }

        public Person GetPersonByNames(string firstName, string secondName, string lastName, CallContext resultContext)
        {
            Person person = null;

            string fullName = string.Empty;

            IQueryable<Person> personResult = null;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(secondName) && !string.IsNullOrEmpty(lastName))
            {
                personResult = this.dbContext.Persons.Where(p => p.FirstName == firstName && p.SecondName == secondName && p.LastName == lastName);

                fullName = firstName + " " + secondName + " " + lastName;
            }
            else if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                personResult = this.dbContext.Persons.Where(p => p.FirstName == firstName && p.LastName == lastName);

                fullName = firstName + " " + lastName;
            }
            else if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(secondName))
            {
                personResult = this.dbContext.Persons.Where(p => p.FirstName == firstName && p.SecondName == secondName);

                fullName = firstName + " " + secondName;
            }
            else if (!string.IsNullOrEmpty(secondName) && !string.IsNullOrEmpty(lastName))
            {
                personResult = this.dbContext.Persons.Where(p => p.SecondName == secondName && p.LastName == lastName);

                fullName = secondName + " " + lastName;
            }
            else if (!string.IsNullOrEmpty(firstName))
            {
                personResult = this.dbContext.Persons.Where(p => p.FirstName == firstName);

                fullName = firstName;
            }
            else if (!string.IsNullOrEmpty(secondName))
            {
                personResult = this.dbContext.Persons.Where(p => p.SecondName == secondName);

                fullName = secondName;
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                personResult = this.dbContext.Persons.Where(p => p.LastName == lastName);

                fullName = lastName;
            }

            if (personResult.Count() == 1)
            {
                person = personResult.FirstOrDefault();
                resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
            }
            else if (personResult.Count() > 1)
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                resultContext.Message = string.Format(BaseHelper.GetCaptionString("Entity_Person_Found_More_Than_One_By_Names"), fullName);
            }
            else
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                resultContext.Message = string.Format(BaseHelper.GetCaptionString("Entity_Person_Not_Found_By_Names"), fullName);
            }

            return person;
        }

        public Person GetPersonByEGN(string _EGN, CallContext resultContext)
        {
            Person person = null;

            IQueryable<Person> personResult = null;

            personResult = this.dbContext.Persons.Where(e => e.EGN == _EGN);

            if (personResult.Count() == 1)
            {
                person = personResult.FirstOrDefault();
                resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
            }
            else if (personResult.Count() > 1)
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                resultContext.Message = string.Format(BaseHelper.GetCaptionString("Entity_Person_Found_More_Than_One_By_EGN"), _EGN);
            }
            else
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                resultContext.Message = string.Format(BaseHelper.GetCaptionString("Entity_Person_Not_Found_By_EGN"), _EGN);
            }

            return person;
        }

        public List<string> BreakFullName(string fullName)
        {
            string[] allNames = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return allNames.ToList();
        }

        internal override void EntityToEntity(Person sourceEntity, Person targetEntity)
        {

            targetEntity.IdentityNumber = sourceEntity.IdentityNumber;

            targetEntity.FirstName = sourceEntity.FirstName;
            targetEntity.SecondName = sourceEntity.SecondName;
            targetEntity.LastName = sourceEntity.LastName;


            targetEntity.FirstNameEN = sourceEntity.FirstNameEN;
            targetEntity.SecondNameEN = sourceEntity.SecondNameEN;
            targetEntity.LastNameEN = sourceEntity.LastNameEN;

            targetEntity.idSex = sourceEntity.idSex;
            targetEntity.idBirthPlace = sourceEntity.idBirthPlace;
            targetEntity.idCitizenship = sourceEntity.idCitizenship;
            targetEntity.idSecondCitizenship = sourceEntity.idSecondCitizenship;
            targetEntity.idMunicipality = sourceEntity.idMunicipality;
            targetEntity.idLocation = sourceEntity.idLocation;

            targetEntity.EGN = sourceEntity.EGN;
            targetEntity.ForeignerIdentityNumber = sourceEntity.ForeignerIdentityNumber;
            targetEntity.IDN = sourceEntity.IDN;
            targetEntity.IdentityNumber = sourceEntity.IdentityNumber;
            targetEntity.IdentityNumberDate = sourceEntity.IdentityNumberDate;
            targetEntity.IdentityNumberIssueBy = sourceEntity.IdentityNumberIssueBy;
            targetEntity.Title = sourceEntity.Title;
            targetEntity.IDCard = sourceEntity.IDCard;

            targetEntity.Address = sourceEntity.Address;
            targetEntity.BirthDate = sourceEntity.BirthDate;
            targetEntity.EMail = sourceEntity.EMail;
            targetEntity.MobilePhone = sourceEntity.MobilePhone;
            targetEntity.Phone = sourceEntity.Phone;

            targetEntity.idPermanentAdressCity = sourceEntity.idPermanentAdressCity;
            targetEntity.idPermanentAdressCountry = sourceEntity.idPermanentAdressCountry;
            targetEntity.idPermanentAdressMunicipality = sourceEntity.idPermanentAdressMunicipality;
            targetEntity.idPlaceOfBirthCity = sourceEntity.idPlaceOfBirthCity;
            targetEntity.idPlaceOfBirthCountry = sourceEntity.idPlaceOfBirthCountry;
            targetEntity.idPlaceOfBirthMunicipality = sourceEntity.idPlaceOfBirthMunicipality;
            targetEntity.idCorespondationCity = sourceEntity.idCorespondationCity;
            targetEntity.idCorespondationCountry = sourceEntity.idCorespondationCountry;
            targetEntity.idCorespondationMunicipality = sourceEntity.idCorespondationMunicipality;
            targetEntity.ImagePath = sourceEntity.ImagePath;

            targetEntity.PermanentPostCode = sourceEntity.PermanentPostCode;
            targetEntity.CorespondationPostCode = sourceEntity.CorespondationPostCode;
            targetEntity.idMaritalstatus = sourceEntity.idMaritalstatus;
            targetEntity.CorespondationAddress = sourceEntity.CorespondationAddress;


            targetEntity.AccessCard1 = sourceEntity.AccessCard1;
            targetEntity.AccessCard2 = sourceEntity.AccessCard1;
            targetEntity.idPosition = sourceEntity.idPosition;

        }

        public List<PersonDataView> GetPersonDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<PersonDataView> listView = (from u in dbContext.Persons
                                             join Nationality in dbContext.KeyValues on u.idCitizenship equals Nationality.idKeyValue into grNationality
                                             from subNationality in grNationality.DefaultIfEmpty()
                                             select new PersonDataView
                                             {
                                                 idPerson = u.idPerson,
                                                 FirstName = u.FirstName,
                                                 SecondName = u.SecondName,
                                                 LastName = u.LastName,
                                                 IdentityNumber = u.IdentityNumber,
                                                 NationalityStr = (subNationality == null ? string.Empty : subNationality.Name)

                                             }).ApplySearchCriterias(searchCriteria).ToList();
            listView = BaseClassBL<PersonDataView>.Sort(listView, sortExpression, sortDirection).ToList();

            return listView;
        }



      

        internal Person GetPersonByUserID(int idUser)
        {
            User user = dbContext.Users.FirstOrDefault(u => u.idUser == idUser);
            Person person = dbContext.Persons.FirstOrDefault(p => p.idPerson == user.idPerson);
            return person;

        }



        

        
    }
}