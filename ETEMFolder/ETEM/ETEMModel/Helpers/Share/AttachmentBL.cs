using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Models.Partial;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Share
{
    public class AttachmentBL : BaseClassBL<Attachment>
    {
        public AttachmentBL()
        {
            this.EntitySetName = "Attachments";
        }
        internal override void EntityToEntity(Attachment sourceEntity, Attachment targetEntity)
        {
            targetEntity.idModule = sourceEntity.idModule;
            targetEntity.idAttachmentType = sourceEntity.idAttachmentType;
            targetEntity.Description = sourceEntity.Description;
            targetEntity.AttachmentDate = sourceEntity.AttachmentDate;
            targetEntity.idUser = sourceEntity.idUser;
        }

        internal override Attachment GetEntityById(int idEntity)
        {
            return this.dbContext.Attachments.FirstOrDefault(s => s.idAttachment == idEntity);
        }

        internal List<AttachmentDataView> GetAccountingAttachmentList(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<AttachmentDataView> list = (from a in dbContext.Attachments
                                             join kvAttachmentType in dbContext.KeyValues on a.idAttachmentType equals kvAttachmentType.idKeyValue
                                             join u in dbContext.Users on a.idUser equals u.idUser
                                             join p in dbContext.Persons on u.idPerson equals p.idPerson
                                             select new AttachmentDataView
                                             {
                                                 idAttachment = a.idAttachment,
                                                 idAttachmentType = a.idAttachmentType,
                                                 idModule = a.idModule,
                                                 Description = a.Description,
                                                 AttachmentDate = a.AttachmentDate,
                                                 idUser = a.idUser,
                                                 Title = p.Title,
                                                 FirstName = p.FirstName,
                                                 LastName = p.LastName,
                                                 AttachmentTypeName = kvAttachmentType.Name,
                                                 idAttachmentTypeKeyType = kvAttachmentType.idKeyType
                                             }).ApplySearchCriterias(searchCriteria).ToList();

            list = OrderByHelper.OrderBy<AttachmentDataView>(list, sortExpression, sortDirection).ToList<AttachmentDataView>();

            return list;
        }
    }
}