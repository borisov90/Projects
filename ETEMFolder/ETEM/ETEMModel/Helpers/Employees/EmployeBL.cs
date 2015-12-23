using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Models.DataView.Employees;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Employees
{
    public class EmployeBL : BaseClassBL<Employe>
    {

        public EmployeBL()
        {
            this.EntitySetName = "Employees";
        }

        internal override void EntityToEntity(Employe sourceEntity, Employe targetEntity)
        {

            targetEntity.idPerson = sourceEntity.idPerson;
        }

        internal override Employe GetEntityById(int idEntity)
        {
            return this.dbContext.Employees.Where(e => e.idEmploye == idEntity).FirstOrDefault();
        }

        internal Employe GetEmployeByPersonId(int idEntity)
        {
            return this.dbContext.Employees.Where(e => e.idPerson == idEntity).FirstOrDefault();
        }

        internal List<EmployeDataView> GetEmployeDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {

            List<EmployeDataView> listView = (from p in dbContext.Persons
                                              join e in dbContext.Employees on p.idPerson equals e.idPerson
                                              orderby p.FirstName, p.LastName ascending

                                              select new EmployeDataView
                                                   {
                                                       idEmploye = e.idEmploye,
                                                       idPerson = p.idPerson,
                                                       IdentityNumber = p.IdentityNumber,
                                                       FirstName = p.FirstName,
                                                       SecondName = p.SecondName,
                                                       LastName = p.LastName,
                                                       Address = p.Address,
                                                       EMail = p.EMail,
                                                       MobilePhone = p.MobilePhone,
                                                       Phone = p.Phone,
                                                       Title = p.Title,
                                                       EGN = p.EGN,
                                                       IDN = p.IDN
                                                   }
                                              ).ApplySearchCriterias(searchCriteria).ToList();

            listView = BaseClassBL<EmployeDataView>.Sort(listView, sortExpression, sortDirection).ToList();
            return listView;
        }

        internal bool DoesEmployeExistByPersonId(int personId)
        {
            bool doesExist = dbContext.Employees.Any(l => l.idPerson == personId);
            return doesExist;
        }

        internal string GetEmployeNameById(int employeId)
        {
            Employe employe = dbContext.Employees.FirstOrDefault(l => l.idEmploye == employeId);
            Person person = dbContext.Persons.FirstOrDefault(p => p.idPerson == employe.idPerson);
            return person.FullName;

        }

    }
}